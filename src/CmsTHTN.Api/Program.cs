using CmsTHTN.Api;
using CmsTHTN.Api.Filters;
using CmsTHTN.Api.Services;
using CmsTHTN.Core.ConfigOptions;
using CmsTHTN.Core.Domain.Identity;
using CmsTHTN.Core.Modals.Content;
using CmsTHTN.Core.SeedWorks;
using CmsTHTN.Data;
using CmsTHTN.Data.Repositories;
using CmsTHTN.Data.SeedWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;

var culture = CultureInfo.InvariantCulture;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");
var CmsTHTNCorsPolicy = "CmsTHTNCorsPolicy";

builder.Services.AddCors(o => o.AddPolicy(CmsTHTNCorsPolicy, builder =>
{
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(configuration["AllowedOrigins"])
        .AllowCredentials();
}));

//Config DB Context and ASP.NET Core Identity
builder.Services.AddDbContext<CmsTHTNContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<CmsTHTNContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    //password setting
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    //lockout setting
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //user setting
    options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM-._@+";
    options.User.RequireUniqueEmail = false;
}
);

// Add services to the container.
builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//business services and repositories
var services = typeof(PostRepository).Assembly.GetTypes()
    .Where(x => x.GetInterfaces().Any(i => i.Name == typeof(IRepository<,>).Name)
    && !x.IsAbstract && x.IsClass && !x.IsGenericType);
foreach (var service in  services)
{
    var allInterfaces = service.GetInterfaces();
    var directInterface = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces())).FirstOrDefault();
    if (directInterface != null)
    {
        builder.Services.Add(new ServiceDescriptor(directInterface, service, ServiceLifetime.Scoped));
    }    
}

//Auto mapper
builder.Services.AddAutoMapper(typeof(PostInListDto));

//Authen and author
builder.Services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
builder.Services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
    c.SwaggerDoc("AdminAPI", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API for Administrator",
        Description = "Api for CMS core domain. This domain keeps track of campaigns, campaign rules"
    });
    c.ParameterFilter<SwaggerNullableParameterFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>
    {
        c.SwaggerEndpoint("AdminAPI/swagger.json", "Admin API");
        c.DisplayOperationId();
        c.DisplayRequestDuration();
    });
}

app.UseCors(CmsTHTNCorsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//seeding data
app.MigrateDatabase();

app.Run();
