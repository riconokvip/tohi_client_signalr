using MessagePack;
using Microsoft.EntityFrameworkCore;
using Tohi.Client.Signalr.Hubs;
using Tohi.Client.Signalr.MiddlewareExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

// Add cache
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

// Builder database
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Add config
ConfigAuthentication(builder);

ConfigMapping(builder);

ConfigServices(builder);

ConfigSignalR(builder);

var app = builder.Build();

// Setup cors
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Setup middleware
app.UseMiddleware<ApplicationMiddleware>();

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");
app.UseTableDependency<UserDependency>(connectionString);
app.UseTableDependency<CdnLiveDependency>(connectionString);

// Migration database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<ClientHub>("tohi/public");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// Setup auth
static void ConfigAuthentication(WebApplicationBuilder builder)
{
    var tokenConfigSection = builder.Configuration.GetSection(JwtConfig.ConfigName);
    if (tokenConfigSection == null)
        throw new Exception("Can not find Token Config in appsettings.json");

    builder.Services.Configure<JwtConfig>(tokenConfigSection);
    builder.Services.AddSingleton(tokenConfigSection.Get<JwtConfig>());
    // builder.Services.AddHttpContextAccessor();
    builder.Services.AddTransient<IJwtHepler, JwtHelper>();
}

// Setup mapping
static void ConfigMapping(WebApplicationBuilder builder)
{
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}

// Setup register service
static void ConfigServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<ApplicationMiddleware>();
    builder.Services.AddScoped<IDistributedCacheExtensionService, DistributedCacheExtensionService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IStreamService, StreamService>();
    builder.Services.AddScoped<ICdnLiveService, CdnLiveService>();

    builder.Services.AddScoped<IClientService, ClientService>();
}

// Setup signalr service
static void ConfigSignalR(WebApplicationBuilder builder)
{
    builder.Services.AddSignalR()
    .AddMessagePackProtocol(options =>
    {
        options.SerializerOptions = MessagePackSerializerOptions.Standard
            .WithSecurity(MessagePackSecurity.TrustedData);
    });

    builder.Services.AddSingleton<ClientHub>();

    builder.Services.AddSingleton<UserDependency>();
    builder.Services.AddSingleton<CdnLiveDependency>();
}