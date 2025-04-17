using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.Models;

namespace CmsTHTN.WebApp.Models
{
    public class SeriesDetailViewModel
    {
        public SeriesDto Series { get; set; }

        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
