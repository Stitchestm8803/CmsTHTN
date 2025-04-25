using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.Models;

namespace CmsTHTN.WebApp.Models
{
    public class AllPostViewModel
    {
        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
