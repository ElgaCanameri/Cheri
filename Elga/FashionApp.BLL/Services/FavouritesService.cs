using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FashionApp.BLL.DTO;
using FashionApp.BLL.DTO.Responses;
using FashionApp.DAL;

namespace FashionApp.BLL.Services
{
	public interface IFavouritesService
	{
		public StandardViewResponse<bool> AddFavourite(DTO.Favourite model);
		public StandardViewResponse<bool> DeleteFavourite(int id, int userId);

		public Favourite GetById(int id);
		public List<Favourite> GetAllFavourites(int userId);
	}
	public class FavouritesService : BaseService, IFavouritesService
	{
		public FavouritesService(IServiceProvider unitOfWork) : base(unitOfWork) { }

		public StandardViewResponse<bool> AddFavourite(DTO.Favourite model)
		{
			try
			{
				var addedFavourite = new DAL.Entities.Favourite()
				{
					ProductId = model.ProductId,
					UserId = model.UserId
				};
				_unitOfWork.FavouritesRepository.Add(addedFavourite);
				_unitOfWork.Commit();
				return new StandardViewResponse<bool>(true);
			}
			catch (Exception) { }
			return new StandardViewResponse<bool>(false, "Produkti nuk mund te shtohet tek lista e te preferuarave.");
		}

		public StandardViewResponse<bool> DeleteFavourite(int id, int userId)
		{
			try
			{
				_unitOfWork.FavouritesRepository.Delete(id, userId);
				_unitOfWork.Commit();
				return new StandardViewResponse<bool>(true);
			}
			catch (Exception) { }
			return new StandardViewResponse<bool>(false, "Produkti nuk mund te fshihet nga lista e te preferuarave.");
		}

		public List<Favourite> GetAllFavourites(int userId)
		{
			var favouritesList = new List<Favourite>();
			try
			{
				favouritesList = _unitOfWork.FavouritesRepository.GetFavouritesByUserId(userId).Select(x => new DTO.Favourite
				{
					Id = x.Id,
					ProductId = x.ProductId,
					UserId = x.UserId,
					Product = x.Product,
				}).ToList();
				return favouritesList;
			}
			catch (Exception){}
			return favouritesList;
		}

		public Favourite GetById(int id)
		{
			try
			{
				var favourite = _unitOfWork.FavouritesRepository.GetById(id);
				var returnedFavourite = new DTO.Favourite()
				{
					ProductId = favourite.ProductId,
					UserId = favourite.UserId
				};
				return returnedFavourite;
			}
			catch (Exception) { }
			return null;
		}
	}
}
