using EmrCloudApi.Configs.Dependency;
using EmrCloudApi.Configs.Options;
using EmrCloudApi.Realtime;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEmrOptions(builder.Configuration);
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// This config is needed for EF Core Migrations to find the DbContext
builder.Services.AddDbContext<TenantDataContext>(options =>
{
    //var connectionStr = builder.Configuration["TenantDbSample"];
    var connectionStr = "host=develop-smartkarte-postgres.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com;port=5432;database=smartkarte;user id=postgres;password=Emr!23456789";
    options.UseNpgsql(connectionStr);
});

//Serilog 
builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));

var dependencySetup = new ModuleDependencySetup();
dependencySetup.Run(builder.Services);

var app = builder.Build();

// Run EF Core Migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TenantDataContext>();
    context.Database.Migrate();
}

//Add config from json file
string enviroment = "Development";
if (app.Environment.IsProduction() ||
    app.Environment.IsStaging())
{
    enviroment = "Staging";
}
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("env.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"env.{enviroment}.json", true, true);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() ||
    app.Environment.IsProduction() ||
    app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// serilog
var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("env.json", false, true)
                .AddJsonFile($"env.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true,
                    true)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(Configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

// SignalR Hub
app.MapHub<CommonHub>("/CommonHub");

//Serilog 
app.UseSerilogRequestLogging();

app.Run();
