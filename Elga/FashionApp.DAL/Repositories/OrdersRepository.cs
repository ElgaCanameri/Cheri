using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Repositories
{
    public interface IOrdersRepository : IBaseRepository<Order, int>
    {
        public List<Order> GetOrdersByUserId(int userId);
        public new List<Order> GetAll();


    }
    public class OrdersRepository : BaseRepository<Order, int>, IOrdersRepository
    {
        public OrdersRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return _set.Include(x => x.OrderItems).ThenInclude(x => x.Product).Where(x => x.UserId == userId).ToList();
        }

        public new List<Order> GetAll()
        {
            return _set.Include(x => x.OrderItems).ThenInclude(x => x.Product).ToList();
        }
    }
}
