using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.SeedWorks;

namespace CmsTHTN.Core.Repository
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
        Task<TagDto> GetBySlug(string slug);
    }
}
