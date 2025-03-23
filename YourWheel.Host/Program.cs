using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using YourWheel.Domain;
using YourWheel.Domain.Services;
using YourWheel.Host;
using YourWheel.Host.AuthorizationPolitics;
using YourWheel.Host.Extensions;
using YourWheel.Host.Services;
using YourWheel.Host.Services.Registration;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

ConfigurationHelper.Initialize(builder.Configuration);

var authSettings = new AuthenticationSettings();

// Add services to the container.
builder.Services.AddDbContext<YourWheelDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_POSTGRES")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = authSettings.JwtValidationParameters;
    });

builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new IsAuthenticationEnabledRequirement())
                    .Build();

        options.AddPolicy("Admin",
                    policy => policy.Requirements.Add(new IsAdminRequirement()));

        //options.AddPolicy("Admin", policy =>
        //    policy.RequireClaim("Role", "Admin"));
    });

builder.Services.AddControllers();

builder.Services.AddSingleton<IObjectTitlesService>(с =>
            new ObjectTitlesService(Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid),
            Guid.Parse(ObjectTitles.Constants.EnglishLanguageGuid)));

builder.Services.AddSingleton<IAuthenticationSettings>(authSettings);

builder.Services.AddSingleton<IHelperRegistrationService, HelperRegistrationService>();

builder.Services.AddTransient<IAuthorizationHandler, IsAuthenticationEnabledRequirementHandler>();

builder.Services.AddTransient<IAuthorizationHandler, IsAdminRequirementHandler>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IAppUserService, AppUserService>();

builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "YourWheel API",
        Description = "ASP.NET Core Web API",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            // Позже вынести в константы
            Email = "stupin.il@yandex.ru",
            Name = "Ilya",
            Url = new Uri("https://github.com/Stupin97/YourWheel")
        }
    });

    var xmlFile = "documentationSwagger.xml";

    var xmlPath = Path.Combine(Environment.CurrentDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // Когда буду разворачивать - настроить
    app.UseHsts();
}

app.UseSwagger();

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "YourWheel API v1"); });

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict, // клиент должен отправлять файлы cookie только с запросами "того же сайта"

    HttpOnly = HttpOnlyPolicy.Always, // Этот файл cookie недоступен для API JavaScript document.cookie

    Secure = CookieSecurePolicy.Always // Все страницы, требующие проверку подлинности, являются HTTPS
});

app.UseStaticFiles();

app.UseRouting();

app.UseHandleJwtInHeaderMiddleware();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
