using Microsoft.AspNetCore.Authorization;

namespace CmsTHTN.Api.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission {  get; set; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
