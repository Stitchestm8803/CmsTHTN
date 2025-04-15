using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Modals.Content;
using CmsTHTN.Core.Modals;
using CmsTHTN.Core.SeedWorks;

namespace CmsTHTN.Core.Repository
{
    public interface IPostCategoryRepository : IRepository<PostCategory, Guid>
    {
        Task<PagedResult<PostCategoryDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task<bool> HasPost(Guid categoryId);
        Task<PostCategoryDto> GetBySlug(string slug);

    }
}
