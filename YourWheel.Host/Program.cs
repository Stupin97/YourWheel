using DotNetEnv;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

using YourWheel.Domain;
using YourWheel.Domain.Services;
using YourWheel.Host;
using YourWheel.Host.AuthorizationPolitics;
using YourWheel.Host.Extensions;
using YourWheel.Host.Services;
using YourWheel.Host.Services.Registration;

var builder = WebApplication.CreateBuilder(args);

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    // ��� ��������� ������
    //Env.Load();
}

builder.Configuration.AddEnvironmentVariables();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

ConfigurationHelper.Initialize(builder.Configuration);

var authSettings = new AuthenticationSettings();

// Add services to the container.
builder.Services.AddDbContext<YourWheelDbContext>(options =>
    options.UseNpgsql(
        ConfigurationHelper.Configuration.GetValue<string>("DB_CONNECTION_STRING_POSTGRES") 
        ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_POSTGRES"))
    );

builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = ConfigurationHelper.Configuration.GetValue<string>("RedisCacheOptions:Configuration");
        options.InstanceName = ConfigurationHelper.Configuration.GetValue<string>("RedisCacheOptions:InstanceName");
    });

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

builder.Services.AddSingleton<IObjectTitlesService>(c =>
            new ObjectTitlesService(Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid),
            Guid.Parse(ObjectTitles.Constants.EnglishLanguageGuid)));

builder.Services.AddSingleton<IAuthenticationSettings>(authSettings);

builder.Services.AddSingleton<IHelperRegistrationService, HelperRegistrationService>();

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
            ConnectionMultiplexer.Connect(ConfigurationHelper.Configuration.GetValue<string>("RedisCacheOptions:Configuration")));

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IAppUserService, AppUserService>();

builder.Services.AddScoped<IRegistrationService, LoginRegistrationService>();

builder.Services.AddScoped<IRegistrationService, EmailRegistrationService>();

builder.Services.AddScoped<IRegistrationServiceFactory, RegistrationServiceFactory>();

builder.Services.AddScoped<IUserInfoService, UserInfoService>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IServiceService, ServiceService>();

builder.Services.AddTransient<IAuthorizationHandler, IsAuthenticationEnabledRequirementHandler>();

builder.Services.AddTransient<IAuthorizationHandler, IsAdminRequirementHandler>();

builder.Services.AddTransient<YourWheel.Host.Services.RabbitMQ.EmailService.IEmailSender, YourWheel.Host.Services.RabbitMQ.EmailService.EmailSender>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "YourWheel API",
        Description = "ASP.NET Core Web API",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            // ����� ������� � ���������
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
    // ����� ���� ������������� - ���������
    app.UseHsts();
}

app.MapGet("/healthz", () => Results.Ok());

app.UseSwagger();

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "YourWheel API v1"); });

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict, // ������ ������ ���������� ����� cookie ������ � ��������� "���� �� �����"

    HttpOnly = HttpOnlyPolicy.Always, // ���� ���� cookie ���������� ��� API JavaScript document.cookie

    Secure = CookieSecurePolicy.Always // ��� ��������, ��������� �������� �����������, �������� HTTPS
});

app.UseStaticFiles();

app.UseRouting();

app.UseHandleJwtInHeaderMiddleware();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
