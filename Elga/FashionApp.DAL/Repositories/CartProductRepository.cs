using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Repositories
{
    public interface ICartProductRepository : IBaseRepository<DAL.Entities.CartProduct, int>
    {
        public List<DAL.Entities.CartProduct> GetAllCartProductsByUser(int userId);
        public CartProduct GetProductById(int prodId, int userId);
        public bool DeleteCartProductByUserId(int userId, int productId);

    }
    public class CartProductRepository : BaseRepository<DAL.Entities.CartProduct, int>, ICartProductRepository
    {
        public CartProductRepository(AppDbContext dbContext) : base(dbContext) { }
        public List<DAL.Entities.CartProduct> GetAllCartProductsByUser(int userId)
        {
            return _set.Where(x => x.Cart.UserId == userId)
                .Include(cp => cp.Product) // Include the related Product entity
                .ToList();
        }

        public CartProduct GetProductById(int prodId, int userId)
        {
            return _set.FirstOrDefault(x => x.ProductId == prodId && x.Cart.UserId == userId);
        }
        public bool DeleteCartProductByUserId(int userId, int productId)
        {
            try
            {
                var cartProduct = _set.FirstOrDefault(x => x.Cart.UserId == userId && x.ProductId == productId);
                _set.Remove(cartProduct);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
