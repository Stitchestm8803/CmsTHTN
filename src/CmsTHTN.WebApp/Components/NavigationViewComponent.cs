using CmsTHTN.Core.SeedWorks;
using CmsTHTN.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CmsTHTN.WebApp.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public NavigationViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _unitOfWork.PostCategories.GetAllAsync();
            var navItems = model.Select(x => new NavigationItemViewModel()
            {
                Slug = x.Slug,
                Name = x.Name,
                IsActive = x.IsActive,
                SortOrder = x.SortOrder,
                Children = model.Where(x => x.ParentId == x.Id).Select(i => new NavigationItemViewModel()
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    SortOrder = i.SortOrder,
                }).OrderBy(c => c.SortOrder).ToList()
            }).OrderBy(n => n.SortOrder).ToList();
            return View(navItems);
        }
    }
}
