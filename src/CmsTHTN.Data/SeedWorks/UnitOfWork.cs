using CmsTHTN.Core.SeedWorks;
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
        public UnitOfWork(CmsTHTNContext context)
        {
            _context = context;
        }
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
