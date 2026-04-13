using Pd.Tasks.Application.Domain.Settings;
using Scalar.AspNetCore;
using Pd.Tasks.Application;
using Pd.Tasks.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

RegisterServices(builder.Configuration, builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Scalar endpoint at /scalar/v1
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static void RegisterServices(IConfiguration configuration, IServiceCollection services)
{
    var appSettingsSection = configuration.GetSection(AppSettings.SectionName);
    services.Configure<AppSettings>(appSettingsSection);

    services.AddApplicationModule().
        AddPersistenceModule(configuration);

    services.AddHttpClient();
    services.AddAutoMapper(typeof(ApplicationModule).Assembly);
}
