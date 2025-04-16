using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.Models;
using CmsTHTN.Core.SeedWorks;
using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Models.Content;

namespace CmsTHTN.Core.Repository
{
    public interface ISeriesRepository : IRepository<Series, Guid>
    {
        Task<PagedResult<SeriesInListDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task AddPostToSeries(Guid seriesId, Guid postId, int sortOrder);
        Task RemovePostToSeries(Guid seriesId, Guid postId);
        Task<List<PostInListDto>> GetAllPostsInSeries(Guid seriesId);
        Task<bool> IsPostInSeries(Guid seriesId, Guid postId);
        Task<bool> HasPost(Guid seriesId);
    }
}
