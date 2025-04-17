using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.Models;

namespace CmsTHTN.WebApp.Models
{
    public class PostListByTagViewModel
    {
        public TagDto Tag { get; set; }
        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
