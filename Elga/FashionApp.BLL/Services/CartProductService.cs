using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.BLL.DTO;
using FashionApp.DAL;
using FashionApp.DAL.Entities;

namespace FashionApp.BLL.Services
{
	public interface ICartProductService
	{
		public List<DAL.Entities.CartProduct> GetAllCartProductsByUser(int userId);
		public CartProduct GetProductById(int prodId, int userId);
        public StandardViewResponse<bool> DeleteCartProductByUserId(int userId, int productId);

    }
    public class CartProductService : BaseService, ICartProductService
	{
		public CartProductService(IServiceProvider unitOfWork) : base(unitOfWork) { }

        public List<CartProduct> GetAllCartProductsByUser(int userId)
		{
			return _unitOfWork.CartProductRepository.GetAllCartProductsByUser(userId).GroupBy(x => x.ProductId).Select(s => new DAL.Entities.CartProduct
			{
				ProductId = s.First().ProductId,
				Product = s.First().Product,
				Quantity = s.Sum(q => q.Quantity),
				CartId = s.First().CartId,
				Cart = s.First().Cart,
				Id = s.First().Id
			}).ToList();
		}

		public CartProduct GetProductById(int prodId, int userId)
		{
			 return _unitOfWork.CartProductRepository.GetProductById(prodId, userId);
		}
		public StandardViewResponse<bool> DeleteCartProductByUserId(int userId, int productId)
        {
			try
			{
                _unitOfWork.CartProductRepository.DeleteCartProductByUserId(userId, productId);
				_unitOfWork.Commit();
				return new StandardViewResponse<bool>(true);
            }
			catch (Exception)
			{
				return new StandardViewResponse<bool>(false, "The item cannot be deleted");
			}
        }
	}
}
