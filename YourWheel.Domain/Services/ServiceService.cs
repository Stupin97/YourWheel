namespace YourWheel.Domain.Services
{
    using Microsoft.EntityFrameworkCore;

    using YourWheel.Domain.Dto.Orders;
    using YourWheel.Domain.Dto.Services;

    public class ServiceService : IServiceService
    {
        private readonly YourWheelDbContext _context;

        public ServiceService(YourWheelDbContext context)
        {
            this._context = context;
        }

        public async Task<List<MaterialDto>> GetAllMaterialAsync()
        {
            return await this._context.Materials
                .Include(c => c.Fabricator)
                .Include(c => c.Color)
                .Select(c => new MaterialDto()
                {
                    MaterialId = c.MaterialId,
                    Name = c.Name,
                    Price = c.Price,
                    Color = new ColorDto()
                    {
                        ColorId = c.ColorId,
                        Name = c.Color.Name
                    },
                    Fabricator = new FabricatorDto()
                    {
                        FabricatorId = c.FabricatorId,
                        Name = c.Fabricator.Name,
                        Country = c.Fabricator.Country
                    }
                }).ToListAsync();
        }

        public async Task<List<MaterialDto>> GetAllMaterialByColorsAndFabricatorsAsync(IEnumerable<Guid> colorIds, IEnumerable<Guid> fabricatorIds)
        {
            var query = this._context.Materials
                .Include(c => c.Fabricator)
                .Include(c => c.Color)
                .Where(c => 
                    (fabricatorIds == null || fabricatorIds.Count() == 0) ? true : fabricatorIds.Contains(c.FabricatorId) 
                    && (colorIds == null || colorIds.Count() == 0) ? true : colorIds.Contains(c.ColorId));

            return await query
                .OrderBy(c => c.Name)
                .Select(c => new MaterialDto()
                {
                    MaterialId = c.MaterialId,
                    Name = c.Name,
                    Price = c.Price,
                    Color = new ColorDto()
                    {
                        ColorId = c.ColorId,
                        Name = c.Color.Name
                    },
                    Fabricator = new FabricatorDto()
                    {
                        FabricatorId = c.FabricatorId,
                        Name = c.Fabricator.Name,
                        Country = c.Fabricator.Country
                    }
                }).ToListAsync();
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
