using CmsTHTN.Core.Domain.Royalty;
using CmsTHTN.Core.Models;
using CmsTHTN.Core.Models.Royalty;
using CmsTHTN.Core.SeedWorks;

namespace CmsTHTN.Core.Repository
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
        Task<PagedResult<TransactionDto>> GetAllPaging(string? userName,
         int fromMonth, int fromYear, int toMonth, int toYear, int pageIndex = 1, int pageSize = 10);
    }
}
