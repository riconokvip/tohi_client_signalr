using Microsoft.EntityFrameworkCore;

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



var app = builder.Build();

// Setup cors
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

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