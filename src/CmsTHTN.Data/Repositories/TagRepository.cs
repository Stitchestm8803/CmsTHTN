using AutoMapper;
using CmsTHTN.Core.Domain.Content;
using CmsTHTN.Core.Models.Content;
using CmsTHTN.Core.Repository;
using CmsTHTN.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;

namespace CmsTHTN.Data.Repositories
{
    public class TagRepository : RepositoryBase<Tag, Guid>, ITagRepository
    {
        private readonly IMapper _mapper;
        public TagRepository(CmsTHTNContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<TagDto?> GetBySlug(string slug)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Slug == slug);
            if (tag == null) return null;
            return _mapper.Map<TagDto?>(tag);
        }
    }
}
