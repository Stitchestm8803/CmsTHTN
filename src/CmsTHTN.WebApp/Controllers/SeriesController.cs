using CmsTHTN.Core.SeedWorks;
using CmsTHTN.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CmsTHTN.WebApp.Controllers
{
    public class SeriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SeriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Route("/series")]
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            var series = await _unitOfWork.Series.GetAllPaging(string.Empty, page, 5);
            
            series.Results = series.Results.OrderBy(s => s.SortOrder).ToList();
            return View(series);
        }

        [Route("series/{slug}")]
        public async Task<IActionResult> Details([FromRoute] string slug)
        {
            var posts = await _unitOfWork.Series.GetAllPostsInSeries(slug);
            var series = await _unitOfWork.Series.GetBySlug(slug);
            return View(new SeriesDetailViewModel()
            {
                Posts = posts,
                Series = series
            });
        }
    }
}
