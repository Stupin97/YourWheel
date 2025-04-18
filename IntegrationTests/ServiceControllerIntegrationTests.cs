namespace IntegrationTests
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.VisualStudio.TestPlatform.TestHost;
    using Moq;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;

    using YourWheel.Domain.Dto.Services;
    using YourWheel.Domain.Services;
    using YourWheel.Host.Services;

    public class ServiceControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        private readonly HttpClient _client;

        public ServiceControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this._factory = factory;

            this._client = this._factory.WithWebHostBuilder(builder =>
            {
                // Настройка клиента
                builder.ConfigureTestServices(services =>
                {
                    var mockService = new Mock<IServiceService>();

                    //Добавляем данные
                    mockService.Setup(s => s.GetAllMaterialAsync()).ReturnsAsync(this.GetTestMaterials());

                    // Подмена на mock
                    services.AddScoped(typeof(IServiceService), sp => mockService.Object);

                    services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        //Для теста без HTTPS
                        options.RequireHttpsMetadata = false;

                        //Настраиваем TokenValidationParameters
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,

                            ValidateAudience = false,

                            ValidateLifetime = true,

                            ValidateIssuerSigningKey = false,
                            
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("2313dasddqwd132d")),

                            ClockSkew = TimeSpan.Zero //Убираем ClockSkew (только для тестов!)
                        };
                    });
                });
            }).CreateClient();

            //Генерируем JWT для тестового пользователя
            var jwtService = _factory.Services.GetRequiredService<IJwtService>();

            var claimsIdentity = GetTestClaimsIdentity();

            var jwt = jwtService.GenerateToken(claimsIdentity.Claims);

            var tokenString = jwtService.GetTokenString(jwt);

            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
        }

        [Fact]
        public async Task GetAllMaterialsAsync_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var url = "/api/service/get-all-materials";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetAllMaterialsAsync_ReturnsCorrectMaterials()
        {
            // Arrange
            var url = "/api/service/get-all-materials";

            // Act
            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var materials = JsonSerializer.Deserialize<List<MaterialDto>>(responseString);

            // Assert
            Assert.NotNull(materials);

            Assert.Equal(2, materials.Count);

            //Проверка на соответствие
            Assert.Equal("Material1", materials[0].Name);

            Assert.Equal("Material2", materials[1].Name);
        }

        //Тестовые данные
        private List<MaterialDto> GetTestMaterials()
        {
            return new List<MaterialDto>
            {
                new MaterialDto 
                {
                    MaterialId = Guid.NewGuid(),
                    Name = "Material1",
                    Price = 10,
                    Color = new ColorDto()
                    {
                        ColorId = Guid.NewGuid(),
                        Name = "Color1"
                    },
                    Fabricator = new FabricatorDto()
                    {
                        FabricatorId = Guid.NewGuid(),
                        Country = "Country1",
                        Name = "Fabriator1"
                    }
                },
                new MaterialDto
                {
                    MaterialId = Guid.NewGuid(),
                    Name = "Material2",
                    Price = 10,
                    Color = new ColorDto()
                    {
                        ColorId = Guid.NewGuid(),
                        Name = "Color2"
                    },
                    Fabricator = new FabricatorDto()
                    {
                        FabricatorId = Guid.NewGuid(),
                        Country = "Country2",
                        Name = "Fabriator2"
                    }
                }
            };
        }

        private ClaimsIdentity GetTestClaimsIdentity()
        {
            var claims = new List<Claim>
            {
                    new Claim("UserId", Guid.NewGuid().ToString()),
                    new Claim("Role", "Admin")
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", "UserId",
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}