using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.Models;

namespace CmsTHTN.WebApp.Models
{
    public class PostListByCategoryViewModel
    {
        public PostCategoryDto Category { get; set; }
        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
