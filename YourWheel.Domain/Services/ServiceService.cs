namespace YourWheel.Domain.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;
    using System.Linq.Expressions;
    using System.Text.Json;
    using YourWheel.Domain.Dto.Orders;
    using YourWheel.Domain.Dto.Services;
    using YourWheel.Domain.Models;

    public class ServiceService : IServiceService
    {
        private readonly YourWheelDbContext _context;

        private readonly IDistributedCache _cache;

        private readonly Expression<Func<Material, MaterialDto>> _materialDto = material =>
            new MaterialDto()
            {
                MaterialId = material.MaterialId,
                Name = material.Name,
                Price = material.Price,
                Color = material.Color != null ? new ColorDto()
                {
                    ColorId = material.ColorId,
                    Name = material.Color.Name
                } : null,
                Fabricator = material.Fabricator != null ? new FabricatorDto()
                {
                    FabricatorId = material.FabricatorId,
                    Name = material.Fabricator.Name,
                    Country = material.Fabricator.Country
                } : null
            };

        public ServiceService(YourWheelDbContext context, IDistributedCache cache)
        {
            this._context = context;

            this._cache = cache;
        }

        public async Task<List<MaterialDto>> GetAllMaterialAsync()
        {
            string materialsKey = "GetAllMaterial";

            List<MaterialDto> materials;

            string materialsData = await this._cache.GetStringAsync(materialsKey);

            if (!String.IsNullOrEmpty(materialsData))
            {
                materials = JsonSerializer.Deserialize<List<MaterialDto>>(materialsData) ?? new List<MaterialDto>();
            }
            else
            {
                materials = await this._context.Materials
                    .Include(c => c.Fabricator)
                    .Include(c => c.Color)
                    .AsNoTracking()
                    .Select(this._materialDto)
                    .ToListAsync();

                if (materials != null)
                {
                    string serializedData = JsonSerializer.Serialize(materials);

                    var cacheOptions = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    await _cache.SetStringAsync(materialsKey, serializedData, cacheOptions);
                }
            }

            return materials;
        }

        public async Task<List<MaterialDto>> GetAllMaterialByColorsAndFabricatorsAsync(IEnumerable<Guid> colorIds, IEnumerable<Guid> fabricatorIds)
        {
            string materialsKey = "GetAllMaterial";

            List<MaterialDto> materials;

            string materialsData = await this._cache.GetStringAsync(materialsKey);

            if (!String.IsNullOrEmpty(materialsData))
            {
                materials = JsonSerializer.Deserialize<List<MaterialDto>>(materialsData)
                    .Where(c =>
                        (fabricatorIds == null || fabricatorIds.Count() == 0) ? true : fabricatorIds.Contains(c.Fabricator.FabricatorId)
                        && (colorIds == null || colorIds.Count() == 0) ? true : colorIds.Contains(c.Color.ColorId))
                    .ToList() ?? new List<MaterialDto>();
            }
            else
            {
                var query = this._context.Materials
                .Include(c => c.Fabricator)
                .Include(c => c.Color)
                .Where(c =>
                    (fabricatorIds == null || fabricatorIds.Count() == 0) ? true : fabricatorIds.Contains(c.FabricatorId)
                    && (colorIds == null || colorIds.Count() == 0) ? true : colorIds.Contains(c.ColorId));

                materials = await query
                    .OrderBy(c => c.Name)
                    .Select(this._materialDto)
                    .ToListAsync();
            }

            return materials;
        }

        public async Task<List<TypeWorkDto>> GetTypeWorksAsync()
        {
            return await this._context.TypeWorks
                .Select(c => new TypeWorkDto()
                {
                    TypeworkId = c.TypeworkId,
                    Name = c.Name,
                    Price = c.Price
                }).ToListAsync();
        }
    }
}
