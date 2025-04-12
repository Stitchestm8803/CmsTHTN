using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Modals.Content;
using CmsTHTN.Core.Modals;
using CmsTHTN.Core.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsTHTN.Core.Repository
{
    public interface IPostCategoryRepository : IRepository<PostCategory, Guid>
    {
        Task<PagedResult<PostCategoryDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
    }
}
