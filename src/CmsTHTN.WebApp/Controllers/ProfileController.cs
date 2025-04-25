using CmsTHTN.Core.Domain.Identity;
using CmsTHTN.Core.SeedWorks.Constants;
using CmsTHTN.Core.SeedWorks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CmsTHTN.WebApp.Models;
using CmsTHTN.WebApp.Extensions;
using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Helpers;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net;
using System.Text.Json;
using CmsTHTN.Core.ConfigOptions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using CmsTHTN.Data;
using Microsoft.EntityFrameworkCore;
using static CmsTHTN.Core.SeedWorks.Constants.Permissions;
using Microsoft.Extensions.Hosting;

namespace CmsTHTN.WebApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SystemConfig _config;
        private readonly CmsTHTNContext _context;
        public ProfileController(IUnitOfWork unitOfWork,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IOptions<SystemConfig> systemConfig,
            CmsTHTNContext context)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = systemConfig.Value;
            _context = context;
        }
        [Route("/profile")]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUser();
            return View(new ProfileViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [HttpGet]
        [Route("/profile/edit")]
        public async Task<IActionResult> ChangeProfile()
        {
            var user = await GetCurrentUser();
            return View(new ChangeProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [Route("/profile/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProfile([FromForm] ChangeProfileViewModel model)
        {
            var user = await GetCurrentUser();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData[SystemConsts.FormSuccessMsg] = "Cập nhật thành công.";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Cập nhật không thành công");
            }
            return View(model);

        }

        [Route("profile/change-password")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("profile/change-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userProfile = await GetCurrentUser();

            var isPasswordValid = await _userManager.CheckPasswordAsync(userProfile, model.OldPassword);
            if (!isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu cũ không đúng");
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(userProfile, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(userProfile);
                TempData[SystemConsts.FormSuccessMsg] = "Đổi mật khẩu thành công";
                return Redirect(UrlConsts.Profile);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();

            return Redirect(UrlConsts.Home);
        }
        private async Task<AppUser> GetCurrentUser()
        {
            var userId = User.GetUserId();
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        [HttpGet]
        [Route("/profile/posts/create")]
        public async Task<IActionResult> CreatePost()
        {
            return View(await SetCreatePostModel());
        }

        [Route("/profile/posts/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostViewModel model, IFormFile thumbnail)
        {
            if (!ModelState.IsValid)
            {
                return View(await SetCreatePostModel());
            }
            var user = await GetCurrentUser();
            var category = await _unitOfWork.PostCategories.GetByIdAsync(model.CategoryId);
            var post = new Post()
            {
                Name = model.Title,
                CategoryName = category.Name,
                CategorySlug = category.Slug,
                Slug = TextHelper.ToUnsignedString(model.Title),
                CategoryId = model.CategoryId,
                Content = model.Content,
                SeoDescription = model.SeoDescription,
                Status = PostStatus.Draft,
                AuthorUserId = user.Id,
                AuthorName = user.GetFullName(),
                AuthorUserName = user.UserName,
                Description = model.Description
            };
            _unitOfWork.Posts.Add(post);
            if (thumbnail != null)
            {
                await UploadThumbnail(thumbnail, post);
            }
            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                TempData[SystemConsts.FormSuccessMsg] = "Bài viết được tạo thành công.";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bài viết tạo thất tại");

            }
            return View(model);
        }

        [HttpGet]
        [Route("profile/posts/edit/{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
                return NotFound("Bài viết không tồn tại!");
            var thumbnailUrl = await GetThumbnail(post.Id);
            var model = new EditPostViewModel
            {
                Id = id,
                Title = post.Name,
                CategoryId = post.CategoryId,
                Content = post.Content,
                SeoDescription = post.SeoDescription,
                Description = post.Description,
                Categories = new SelectList(await _unitOfWork.PostCategories.GetAllAsync(), "Id", "Name"),
                ThumbnailImage = thumbnailUrl ?? post.Thumbnail
            };
            return View("EditPost", model);
        }

        [HttpPost]
        [Route("profile/posts/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([FromForm] EditPostViewModel model, IFormFile thumbnail)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(model.Id);
            var category = await _unitOfWork.PostCategories.GetByIdAsync(model.CategoryId);

            if (post == null)
            {
                return NotFound("Bài viết không tồn tại!");
            }
            post.CategoryId = model.CategoryId;
            post.CategoryName = category.Name;
            post.CategorySlug = category.Slug;
            post.Content = model.Content;
            post.SeoDescription = model.SeoDescription;
            post.Description = model.Description;
            post.DateModified = DateTime.Now;
            if (thumbnail != null)
            {
                await UploadThumbnail(thumbnail, post);
                post.Thumbnail = await GetThumbnail(post.Id) ?? post.Thumbnail;
            }
            await _unitOfWork.CompleteAsync();
            TempData[SystemConsts.FormSuccessMsg] = "Bài viết được cập nhật thành công.";

            return Redirect($"{UrlConsts.EditPost}/{post.Id}");
        }

        private async Task UploadThumbnail(IFormFile thumbnail, Post post)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.BackendApiUrl);

                byte[] data;
                using (var br = new BinaryReader(thumbnail.OpenReadStream()))
                {
                    data = br.ReadBytes((int)thumbnail.OpenReadStream().Length);
                }

                var bytes = new ByteArrayContent(data);

                var multiContent = new MultipartFormDataContent
                {
                    { bytes, "file", thumbnail.FileName }
                };

                var uploadResult = await client.PostAsync("api/admin/media?type=posts", multiContent);
                if (uploadResult.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.AddModelError("", await uploadResult.Content.ReadAsStringAsync());
                }
                else
                {
                    var path = await uploadResult.Content.ReadAsStringAsync();
                    var pathObj = JsonSerializer.Deserialize<UploadResponse>(path);
                    post.Thumbnail = pathObj?.Path;
                }

            }
        }

        private async Task<string?> GetThumbnail(Guid postId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.BackendApiUrl);

                // Gửi request GET để lấy thông tin thumbnail của bài viết theo ID
                var response = await client.GetAsync($"api/admin/media/thumbnail?postId={postId}");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"Lỗi khi lấy thumbnail: {await response.Content.ReadAsStringAsync()}");
                    return null; // Nếu có lỗi, trả về null
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var pathObj = JsonSerializer.Deserialize<UploadResponse>(jsonResponse);
                return pathObj?.Path; // Trả về đường dẫn thumbnail
            }
        }


        private async Task<CreatePostViewModel> SetCreatePostModel()
        {
            var model = new CreatePostViewModel()
            {
                Title = "Chưa có tiêu đề",
                Categories = new SelectList(await _unitOfWork.PostCategories.GetAllAsync(), "Id", "Name")
            };
            return model;
        }

        [HttpGet]
        [Route("/profile/posts/list")]
        public async Task<IActionResult> ListPosts(string keyword, int page = 1)
        {
            var userId = User.GetUserId();
            var posts = await _unitOfWork.Posts.GetPostByUserPaging(keyword, userId, page, 12);
            return View(new ListPostByUserViewModel()
            {
                Posts = posts,
                TotalPosts = await _context.Posts.CountAsync(x => x.AuthorUserId == userId),
                TotalDraftPosts = await _context.Posts.CountAsync(x => x.AuthorUserId == userId && x.Status == PostStatus.Draft),
                TotalWaitingApprovalPosts = await _context.Posts.CountAsync(x => x.AuthorUserId == userId && x.Status == PostStatus.WaitingForApproval),
                TotalPublishedPosts = await _context.Posts.CountAsync(x => x.AuthorUserId == userId && x.Status == PostStatus.Published),
                TotalUnpaidPosts = await _context.Posts.CountAsync(x => x.AuthorUserId == userId && x.Status == PostStatus.Published && x.IsPaid == false),
                TotalRejectedPost = await _context.Posts.CountAsync(x => x.AuthorUserId == userId && x.Status == PostStatus.Rejected),
                TotalPaidAmount = await _context.Posts.Where(x => x.AuthorUserId == userId && x.Status == PostStatus.Published && x.IsPaid == true).SumAsync(x => x.RoyaltyAmount)
            });
        }

        [HttpGet]
        [Route("/profile/posts/send-approve/{id:guid}")]
        public async Task<IActionResult> SendApprove(Guid id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            return View("SendApprove", post);
        }

        [HttpPost]
        [Route("/profile/posts/confirm-approve")]
        public async Task<IActionResult> ConfirmApprove(Guid id, [FromForm] bool approve)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (approve)
            {
                post.Status = PostStatus.WaitingForApproval;
                await _unitOfWork.CompleteAsync();
            }
            return Redirect("/profile/posts/list"); // Quay về danh sách bài viết 
        }
    }
}
