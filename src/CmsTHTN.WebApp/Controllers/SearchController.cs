using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.Models;
using CmsTHTN.Core.SeedWorks;
using CmsTHTN.Data;
using CmsTHTN.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CmsTHTN.WebApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly CmsTHTNContext _context;
        private readonly IMapper _mapper;

        public SearchController(CmsTHTNContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Route("posts/search")]
        public async Task<IActionResult> Index([FromQuery] string keyword, [FromQuery] int page = 1, int pageSize = 5)
        {
            var query = _context.Posts
                    .Where(post => post.Status == PostStatus.Published && EF.Functions.Like(post.Name, $"%{keyword}%"))
                    .OrderByDescending(post => post.ViewCount)
                    .Take(20);

            var totalItems = await query.CountAsync();

            var pagedPosts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var mappedPosts = _mapper.Map<List<PostInListDto>>(pagedPosts);

            return View(new AllPostViewModel
            {
                Posts = new PagedResult<PostInListDto> { Results = mappedPosts, RowCount = totalItems, CurrentPage = page, PageSize = pageSize }
            });
        }
    }
}
