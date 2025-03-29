using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Modals;
using CmsTHTN.Core.Modals.Content;
using CmsTHTN.Core.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsTHTN.Core.Repository
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
        Task<List<Post>> GetPopularPostsAsync(int count);
        Task<PagedResult<PostInListDto>> GetPostsPagingAsync(string? keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 10);
    }
}
