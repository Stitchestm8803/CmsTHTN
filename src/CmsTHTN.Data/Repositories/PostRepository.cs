using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Repository;
using CmsTHTN.Core.SeedWorks;
using CmsTHTN.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsTHTN.Data.Repositories
{
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        public PostRepository(CmsTHTNContext context) : base(context)
        {
        }
        public Task<List<Post>> GetPopularPostsAsync(int count)
        {
            return _context.Posts.OrderByDescending(x => x.ViewCount).Take(count).ToListAsync();
        }
    }
}
