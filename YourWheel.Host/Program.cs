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
            // Позже вынести в константы
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
    // Когда буду разворачивать - настроить
    app.UseHsts();
}

app.UseSwagger();

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rem API v1"); });

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict, // клиент должен отправлять файлы cookie только с запросами "того же сайта"

    HttpOnly = HttpOnlyPolicy.Always, // Этот файл cookie недоступен для API JavaScript document.cookie

    Secure = CookieSecurePolicy.Always // Все страницы, требующие проверку подлинности, являются HTTPS
});

app.UseStaticFiles();

app.UseRouting();

app.UseHandleJwtInHeaderMiddleware(); // ОСТАНОВИЛСЯ на переносе Jwt в header 

app.UseAuthorization();

app.MapRazorPages();

app.Run();
