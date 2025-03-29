using AutoMapper;
using CmsTHTN.Core.Repository;
using CmsTHTN.Core.SeedWorks;
using CmsTHTN.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsTHTN.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CmsTHTNContext _context;
        public UnitOfWork(CmsTHTNContext context, IMapper mapper)
        {
            _context = context;
            Posts = new PostRepository(context, mapper);
        }
        public IPostRepository Posts { get; set; }
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
