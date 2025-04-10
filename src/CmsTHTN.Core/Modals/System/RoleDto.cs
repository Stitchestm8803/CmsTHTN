using AutoMapper;
using CmsTHTN.Core.Domain.Identity;

namespace CmsTHTN.Core.Modals.System
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles() 
            {
                CreateMap<AppRole, RoleDto>();
            }
        }
    }
}
