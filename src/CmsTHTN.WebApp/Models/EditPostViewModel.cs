using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CmsTHTN.WebApp.Models
{
    public class EditPostViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? ThumbnailImage { get; set; }
        public Guid CategoryId { get; set; }
        public SelectList? Categories { get; set; }
        public DateTime? DateModified { get; set; }
        public string? SeoDescription { get; set; }
    }
}
