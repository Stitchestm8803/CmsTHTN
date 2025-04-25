using CmsTHTN.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CmsTHTN.WebApp.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
