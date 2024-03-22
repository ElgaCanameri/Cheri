using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Repositories
{
    public interface IFavouritesRepository : IBaseRepository<Favourite, int>{
        public List<Favourite> GetFavouritesByUserId(int id);
        public new void Delete(int productId, int userId);


    }
	public class FavouritesRepository : BaseRepository<Favourite, int>, IFavouritesRepository
    {
        public FavouritesRepository(AppDbContext dbContext) : base(dbContext){}

        public List<Favourite> GetFavouritesByUserId(int id)//merr listen me favourites ne baze te userit te loguar
        {
            return _set.Include(x => x.Product).Where(x => x.UserId == id).ToList();
        }

        public new void Delete(int productId, int userId)//i ben override delete fshin ne baze te id se produktit te tabeles favourite
        {
			var product = _set.FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);
			_set.Remove(product);
        }
    }
}
