using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.SeedWorks;
using CmsTHTN.Data;
using CmsTHTN.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CmsTHTN.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CmsTHTNContext _context;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, CmsTHTNContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel()
            {
                LatestPosts = await _unitOfWork.Posts.GetLatestPublishPost(10),
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
