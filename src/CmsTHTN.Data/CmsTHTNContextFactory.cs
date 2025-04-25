using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CmsTHTN.Data
{
    public class CmsTHTNContextFactory : IDesignTimeDbContextFactory<CmsTHTNContext>
    {
        public CmsTHTNContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<CmsTHTNContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new CmsTHTNContext(builder.Options);
        }
    }
}
