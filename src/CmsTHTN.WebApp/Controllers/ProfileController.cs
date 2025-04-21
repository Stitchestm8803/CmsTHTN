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

namespace CmsTHTN.WebApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SystemConfig _config;
        public ProfileController(IUnitOfWork unitOfWork,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IOptions<SystemConfig> systemConfig)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = systemConfig.Value;
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
        public async Task<IActionResult> ListPosts()
        {
            return View(await SetCreatePostModel());
        }
    }
}
