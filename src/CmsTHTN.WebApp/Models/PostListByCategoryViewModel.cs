using CmsTHTN.Core.Modals.Content;
using CmsTHTN.Core.Modals;

namespace CmsTHTN.WebApp.Models
{
    public class PostListByCategoryViewModel
    {
        public PostCategoryDto Category { get; set; }
        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
