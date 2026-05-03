using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pd.Tasks.Application;
using Pd.Tasks.Application.Domain.Settings;
using Pd.Tasks.Application.Features.UsersManagement.Persistence;
using Pd.Tasks.Persistence;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
                var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await userRepository!.GetUserAsync(u => u.Id == userId);
                if (user == null || !user.IsActive)
                    context.Fail("Unauthorized");
            }
        };
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:SecretKey"] ?? "")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

RegisterServices(builder.Configuration, builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static void RegisterServices(IConfiguration configuration, IServiceCollection services)
{
    var appSettingsSection = configuration.GetSection(AppSettings.SectionName);
    services.Configure<AppSettings>(appSettingsSection);

    services.AddApplicationModule()
        .AddPersistenceModule(configuration);

    services.AddHttpClient();
    services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationModule).Assembly));
}
