using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

using System.Reflection;

using YourWheel.Domain;
using YourWheel.Host;
using YourWheel.Host.Extensions;
using YourWheel.Host.Services;

var builder = WebApplication.CreateBuilder(args);

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
        options.AddPolicy("RequireAdmin", policy =>
            policy.RequireClaim("Role", "Admin"));
    });

// https://andrewlock.net/custom-authorisation-policies-and-requirements-in-asp-net-core/
// ����� ��������� ������ � ���������������� ������������� �� ����� ����������, ����� ��������� �� ����������������!!!
// ����������� �����, ����:
// 1. � ������ �������� ���-�� true - ������ ����������� �����������
// 2. options.DefaultPolicy = �������� �������� � ���������� ���-�� �� 1.
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddSingleton<IObjectTitlesService, ObjectTitlesService>();

builder.Services.AddSingleton<IAuthenticationSettings>(authSettings);

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

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
