namespace YourWheel.Domain.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Dto.Orders;
    using YourWheel.Domain.Dto.Services;
    using YourWheel.Domain.Extensions;
    using YourWheel.Domain.Models;

    public class OrderService : IOrderService
    {
        private readonly YourWheelDbContext _context;

        private enum Statuses
        {
            [AmbientValue(typeof(Guid), "21e4bd82-89ea-48c1-8923-4fd9b3df7a17")]
            Draft,
            [AmbientValue(typeof(Guid), "c250e52a-e334-4064-b35b-9f08c89f8deb")]
            Created,
            [AmbientValue(typeof(Guid), "2b125e13-32d4-4e9a-9fe3-88d3d98a5602")]
            Confirmed,
            [AmbientValue(typeof(Guid), "240a277c-ba14-44bc-bbdd-fba7f5f7bc9d")]
            ReadyForIssue,
            [AmbientValue(typeof(Guid), "72239594-28a1-4be3-8b91-e633206c1178")]
            Finished,
            [AmbientValue(typeof(Guid), "aba36d1c-d0e7-4185-958b-bd0f65394122")]
            Cancelled
        }

        private readonly Expression<Func<Order, OrderDto>> _orderDto = order =>
            new OrderDto()
            {
                OrderId = order.OrderId,
                DateOrder = order.DateOrder,
                Discount = order.Discount,
                DateEnd = order.DateEnd,
                DateExecute = order.DateExecute,
                Description = order.Description,
                Masters = order.Masters.Select(m => new MasterDto()
                {
                    MasterId = m.MasterId,
                    WorkExperienceDate = m.WorkExperienceDate,
                    User = m.User != null ? new UserDto()
                    {
                        Name = m.User.Name,
                        Surname = m.User.Name,
                    } : null
                }).ToList(),
                Status = new StatusDto()
                {
                    StatusId = order.Status.StatusId,
                    Name = order.Status.Name
                },
                Works = order.Works.Select(work => new WorkDto()
                {
                    WorkId = work.WorkId,
                    Price = work.Price,
                    Discount = work.Discount,
                    Material = work.Material != null ? new MaterialDto()
                    {
                        MaterialId = work.Material.MaterialId,
                        Name = work.Material.Name,
                        Price = work.Material.Price,
                        Color = work.Material.Color != null ? new ColorDto()
                        {
                            ColorId = work.Material.Color.ColorId,
                            Name = work.Material.Color.Name
                        } : null,
                        Fabricator = work.Material.Fabricator != null ? new FabricatorDto()
                        {
                            FabricatorId = work.Material.Fabricator.FabricatorId,
                            Name = work.Material.Fabricator.Name,
                            Country = work.Material.Fabricator.Country
                        } : null
                    } : null,
                    Typework = work.Typework != null ? new TypeWorkDto()
                    {
                        TypeworkId = work.Typework.TypeworkId,
                        Name = work.Typework.Name,
                        Price = work.Typework.Price
                    } : null
                }).ToList()
            };

        private readonly Expression<Func<Work, WorkDto>> _workDto = work =>
            new WorkDto()
            {
                WorkId = work.WorkId,
                Price = work.Price,
                Discount = work.Discount,
                Material = work.Material != null ? new MaterialDto()
                {
                    MaterialId = work.Material.MaterialId,
                    Name = work.Material.Name,
                    Price = work.Material.Price,
                    Color = work.Material.Color != null ? new ColorDto()
                    {
                        ColorId = work.Material.Color.ColorId,
                        Name = work.Material.Color.Name
                    } : null,
                    Fabricator = work.Material.Fabricator != null ? new FabricatorDto()
                    {
                        FabricatorId = work.Material.Fabricator.FabricatorId,
                        Name = work.Material.Fabricator.Name,
                        Country = work.Material.Fabricator.Country
                    } : null
                } : null,
                Typework = work.Typework != null ? new TypeWorkDto()
                {
                    TypeworkId = work.Typework.TypeworkId,
                    Name = work.Typework.Name,
                    Price = work.Typework.Price
                } : null
            };

        public OrderService(YourWheelDbContext context) 
        {
            this._context = context;
        }

        public async Task AddWorkToOrderAsync(Guid userId, CreatedWorkByUserDto workDto)
        {
            // Пока не уверен в валидации введенных значений
            if (!await this._context.TypeWorks.AnyAsync(c => c.TypeworkId == workDto.TypeWorkId))
                throw new ArgumentOutOfRangeException("TypeWork not found");

            Material material = await this._context.Materials.FirstOrDefaultAsync(c => c.MaterialId == workDto.MaterialId);

            if (material == null)
                throw new ArgumentOutOfRangeException("Material not found");

            // Добавление и редактирование происходит только для Draft заказа
            Order order = await this._context.Orders
                .FirstOrDefaultAsync(c => c.UserId == userId && c.StatusId == (Guid)Statuses.Draft.GetAmbientValue());

            Guid orderId = order != null ? order.OrderId : Guid.NewGuid();

            if (order == null)
            {
                await this.CreateOrderIfNeedAsync(userId, orderId);
            }

            // Без запроса (Если уже был Draft заказ)
            order = await this._context.Orders.FirstOrDefaultAsync(c => c.OrderId == orderId);

            if (order != null)
            {
                Work work = new Work()
                {
                    WorkId = workDto.CreatedWorkByUserDtoId,
                    OrderId = order.OrderId,
                    MaterialId = workDto.MaterialId,
                    TypeWorkId = workDto.TypeWorkId,
                    Discount = 0,
                    Price = material.Price.HasValue ? material.Price.Value : 0
                };

                this._context.Works.Add(work);

                await this._context.SaveChangesAsync();
            }
        }

        public async Task CreateOrderIfNeedAsync(Guid userId, Guid orderId)
        {
            Order order = await this._context.Orders.FindAsync(orderId);

            if (order == null)
            {
                // Заказ изначально создается только при добавлении хотя бы одной работы
                order = new Order()
                {
                    OrderId = orderId,
                    UserId = userId,
                    DateOrder = DateTime.Now,
                    Discount = 0,
                    StatusId = (Guid)Statuses.Draft.GetAmbientValue()
                };

                this._context.Orders.Add(order);

                //await this._context.SaveChangesAsync();
            }
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync(Guid userId)
        {
            var user = await this._context.Users.FindAsync(userId);

            if (user == null) throw new ArgumentOutOfRangeException("User not found");

            List<OrderDto> orders = await this._context.Orders
                .Include(c => c.Status)
                .Include(c => c.Masters)
                    .ThenInclude(c => c.User)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Color)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Typework)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Fabricator)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Color)
                .Where(c => c.UserId == userId)
                .Select(this._orderDto)
                .ToListAsync();

            return orders;
        }

        public async Task<List<OrderDto>> GetAllOrdersByStatusAsync(Guid userId, Guid statusId)
        {
            var user = await this._context.Users.FindAsync(userId);

            if (user == null) throw new ArgumentOutOfRangeException("User not found");

            List<OrderDto> orders = await this._context.Orders
                .Include(c => c.Status)
                .Include(c => c.Masters)
                    .ThenInclude(c => c.User)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Color)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Typework)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Fabricator)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Color)
                .Where(c => c.UserId == userId && c.StatusId == statusId)
                .Select(_orderDto)
                .ToListAsync();

            return orders;
        }

        public async Task<OrderDto> GetShortOrderAsync(Guid userId, Guid orderId)
        {
            var user = await this._context.Users.FindAsync(userId);

            if (user == null) throw new ArgumentOutOfRangeException("User not found");

            OrderDto orderDto = await this._context.Orders
                .Include(c => c.Status)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                    .Include(c => c.Works)
                    .ThenInclude(c => c.Typework)
                .Select(this._orderDto)
                .FirstOrDefaultAsync(c => orderId == orderId);

            return orderDto;
        }

        public async Task<PaginationDto<OrderDto>> GetOrdersAsync(Guid userId, int count, int offset)
        {
            var user = await this._context.Users.FindAsync(userId);

            if (user == null) throw new ArgumentOutOfRangeException("User not found");

            List<OrderDto> orders = await this._context.Orders
                .Include(c => c.Status)
                .Include(c => c.Masters)
                    .ThenInclude(c => c.User)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Color)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Typework)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Fabricator)
                .Include(c => c.Works)
                    .ThenInclude(c => c.Material)
                        .ThenInclude(c => c.Color)
                .Where(c => c.UserId == userId)
                .Select(this._orderDto)
                .ToListAsync();

            int ordersCount = orders.Count();

            var response = orders
                .Skip(offset)
                .Take(count);

            return new PaginationDto<OrderDto>(ordersCount, count, offset, response);
        }

        public async Task<bool> HasDraftOrderAsync(Guid userId)
        {
            return await this._context.Orders
                .AnyAsync(c => c.UserId == userId && c.StatusId == (Guid)Statuses.Draft.GetAmbientValue());
        }

        public async Task<WorkDto> GetWorkAsync(Guid workId)
        {
            return await this._context.Works
                .Include(c => c.Material)
                    .ThenInclude(c => c.Fabricator)
                .Include(c => c.Material)
                    .ThenInclude(c => c.Color)
                .Where(c => c.WorkId == workId)
                .Select(this._workDto)
                .FirstOrDefaultAsync();
        }

        public async Task<List<WorkDto>> GetAllWorksForOrderAsync(Guid orderId)
        {
            return await this._context.Works
                .Include(c => c.Material)
                    .ThenInclude(c => c.Fabricator)
                .Include(c => c.Material)
                    .ThenInclude(c => c.Color)
                .Where(c => c.OrderId == orderId)
                .Select(this._workDto)
                .ToListAsync();
        }

        public async Task RemoveWorkFromOrderAsync(Guid orderId, Guid workId)
        {
            Order order = await this._context.Orders
                .Include(c => c.Works)
                .FirstOrDefaultAsync(c => c.OrderId == orderId);

            if (order == null) throw new ArgumentOutOfRangeException($"Order not found: {orderId}");

            Work work = order.Works.FirstOrDefault(c => c.WorkId == workId);

            if (work == null) throw new ArgumentOutOfRangeException($"Order not found: {workId}");

            this._context.Works.Remove(work);

            await this._context.SaveChangesAsync();
        }

        // Может orderId все таки
        public async Task ConfirmOrderAsync()
        {
            Order order = await this._context.Orders
                .FirstOrDefaultAsync(c => c.StatusId == (Guid)Statuses.Draft.GetAmbientValue());

            if (order == null) return;

            order.StatusId = (Guid)Statuses.Created.GetAmbientValue();

            await this._context.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            Order order = await this._context.Orders
                .FirstOrDefaultAsync(c => c.OrderId == orderId);

            if (order == null) return;

            order.StatusId = (Guid)Statuses.Cancelled.GetAmbientValue();

            await this._context.SaveChangesAsync();
        }
    }
}