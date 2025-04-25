using CmsTHTN.Core.Domain.Identity;
using CmsTHTN.Core.SeedWorks;

namespace CmsTHTN.Core.Repository
{
    public interface IUserRepository : IRepository<AppUser, Guid>
    {
        Task RemoveUserFromRoles(Guid userId, string[] roles);
    }
}
