using CmsTHTN.Core.SeedWorks;
using CmsTHTN.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CmsTHTN.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("posts")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("posts/older-posts")]
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            var posts = await _unitOfWork.Posts.GetAllPostPaging(page, 5); // Hiển thị tất cả bài viết với phân trang
            return View(new AllPostViewModel
            {
                Posts = posts
            });
        }


        [Route("posts/{categorySlug}")]
        public async Task<IActionResult> ListByCategory([FromRoute] string categorySlug, [FromQuery] int page = 1)
        {
            var posts = await _unitOfWork.Posts.GetPostByCategoryPaging(categorySlug, page, 2);
            var category = await _unitOfWork.PostCategories.GetBySlug(categorySlug);
            return View(new PostListByCategoryViewModel()
            {
                Posts = posts,
                Category = category
            });
        }

        [Route("tag/{tagSlug}")]
        public async Task<IActionResult> ListByTag([FromRoute] string tagSlug, [FromQuery] int page = 1)
        {
            var posts = await _unitOfWork.Posts.GetPostByTagPaging(tagSlug, page, 5);
            var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
            return View(new PostListByTagViewModel()
            {
                Posts = posts,
                Tag = tag,
            });
        }

        [Route("post/{slug}")]
        public async Task<IActionResult> Details([FromRoute] string slug)
        {
            var post = await _unitOfWork.Posts.GetBySlug(slug);
            var category = await _unitOfWork.PostCategories.GetBySlug(post.CategorySlug);
            var tags = await _unitOfWork.Posts.GetTagObjectsByPostId(post.Id);
            return View(new PostDetailViewModel()
            {
                Post = post,
                Category = category,
                Tags = tags
            });
        }
    }
}
