using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Repositories
{
	public interface ICartRepository : IBaseRepository<Cart, int>
	{
		public Cart GetByAccountId(int userId);
		public bool ClearUserCart(int userId);

    }
    public class CartRepository : BaseRepository<Cart, int>, ICartRepository
	{
		public CartRepository(AppDbContext dbContext) : base(dbContext) { }
		public Cart GetByAccountId(int userId)//kur merr shporten merr dhe produktet brenda asaj shporte
		{
			var k = _set.Include(x => x.CartProducts).FirstOrDefault(x => x.UserId == userId);
			return k;
		}

		public bool ClearUserCart(int userId)
		{
			try
			{
                var cart = _set.FirstOrDefault(x => x.UserId == userId);
                _set.Remove(cart);
				return true;
			}
			catch (Exception) { }
			return false;
		}
    }
}
