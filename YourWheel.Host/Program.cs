using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.OpenApi.Models;
using System.Reflection;
using YourWheel.Host.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

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

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rem API v1"); });

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict, // ������ ������ ���������� ����� cookie ������ � ��������� "���� �� �����"

    HttpOnly = HttpOnlyPolicy.Always, // ���� ���� cookie ���������� ��� API JavaScript document.cookie

    Secure = CookieSecurePolicy.Always // ��� ��������, ��������� �������� �����������, �������� HTTPS
});

app.UseStaticFiles();

app.UseRouting();

app.UseHandleJwtInHeaderMiddleware(); // ����������� �� �������� Jwt � header 

app.UseAuthorization();

app.MapRazorPages();

app.Run();
