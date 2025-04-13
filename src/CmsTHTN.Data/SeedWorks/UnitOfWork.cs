using AutoMapper;
using CmsTHTN.Core.Domain.Identity;
using CmsTHTN.Core.Repository;
using CmsTHTN.Core.SeedWorks;
using CmsTHTN.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CmsTHTN.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CmsTHTNContext _context;
        public UnitOfWork(CmsTHTNContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            Posts = new PostRepository(context, mapper, userManager);
            PostCategories = new PostCategoryRepository(context, mapper);
            Series = new SeriesRepository(context, mapper);
        }
        public IPostRepository Posts { get; set; }

        public IPostCategoryRepository PostCategories {  get; private set; }

        public ISeriesRepository Series { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
