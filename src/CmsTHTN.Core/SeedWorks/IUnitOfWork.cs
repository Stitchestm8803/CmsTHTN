using CmsTHTN.Core.Repository;

namespace CmsTHTN.Core.SeedWorks
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        Task<int> CompleteAsync();
    }
}
