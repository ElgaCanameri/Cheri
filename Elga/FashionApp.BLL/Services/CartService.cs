using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.BLL.DTO;
using FashionApp.BLL.DTO.Responses;
using FashionApp.DAL;
using FashionApp.DAL.Entities;

namespace FashionApp.BLL.Services
{
	public interface ICartService
	{
		public StandardViewResponse<bool> AddProduct(int userId, int productId);
		public StandardViewResponse<bool> DeleteProduct(int userId, int productId);
		public List<DTO.Responses.ProductIndexModel> GetAllProducts(int userId);
		public StandardViewResponse<bool> UpdateCart(int productId, int userId, int quantity, string op);
		public StandardViewResponse<bool> ClearUserCart(int userId);

    }
    public class CartService : BaseService, ICartService
	{
		public CartService(IServiceProvider unitOfWork) : base(unitOfWork) { }

		public StandardViewResponse<bool> AddProduct(int userId, int productId)
		{
			try
			{
				var cart = _unitOfWork.CartRepository.GetByAccountId(userId);
				var product = _unitOfWork.ProductsRepository.GetById(productId);
				if (cart == null)
				{
					cart = new Cart()
					{
						UserId = userId,
						CartProducts = new List<CartProduct>()
					};
					_unitOfWork.CartRepository.Add(cart);
				}
				cart.CartProducts.Add(new CartProduct()//shtohet te table cart products
				{
					Cart = cart,
					Product = product,
					Quantity = 1
				});
				_unitOfWork.Commit();
				return new StandardViewResponse<bool>(true);
			}
			catch (Exception) { }
			return new StandardViewResponse<bool>(false, "Produkti nuk mund te shtohet tek shporta.");
		}

		public StandardViewResponse<bool> DeleteProduct(int userId, int productId)
		{
			try
			{
				var cart = _unitOfWork.CartRepository.GetByAccountId(userId);
				var cartProductToRemove = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
				_unitOfWork.CartProductRepository.Delete(cartProductToRemove.Id);
				_unitOfWork.Commit();
				return new StandardViewResponse<bool>(true);
			}
			catch (Exception) { }
			return new StandardViewResponse<bool>(false, "Produkti nuk mund te fshihet nga shporta.");
		}

		public List<DTO.Responses.ProductIndexModel> GetAllProducts(int userId)
		{
			return _unitOfWork.CartProductRepository.GetAllCartProductsByUser(userId).GroupBy(x => x.Id).Select(g => new ProductIndexModel
			{
				Id = g.First().ProductId,
				Title = g.First().Product.Title,
				ImagePath = g.First().Product.ImagePath,
				Price = g.First().Product.Price,
				Quantity = g.Sum(j => j.Quantity)
			}).ToList();
		}

		public StandardViewResponse<bool> UpdateCart(int productId, int userId, int quantity, string op)
		{
			try
			{
				var cart = _unitOfWork.CartRepository.GetByAccountId(userId);
				var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
				if (op == "plus")
				{
					cartProduct.Quantity = quantity + 1;
				}
				else if (op == "minus")
				{
					if (cartProduct.Quantity > 1)
					{
						cartProduct.Quantity = quantity - 1;
					}
				}
				_unitOfWork.Commit();
				return new StandardViewResponse<bool>(true);
			}
			catch (Exception)
			{
			}
			return new StandardViewResponse<bool>(false);
		}

		public StandardViewResponse<bool> ClearUserCart(int userId)
		{
			try
			{
				_unitOfWork.CartRepository.ClearUserCart(userId);
				_unitOfWork.Commit();
				return new StandardViewResponse<bool>(true);
			}
			catch (Exception) { }
			return new StandardViewResponse<bool>(false, "The cart cannot be cleared!");
		}
	}
}
