using System.Security.Claims;
using FashionApp.BLL.DTO;
using FashionApp.BLL.Services;
using FashionApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Authorize(Roles = "User")]
    public class FavouriteController : Controller
    {
        public IFavouritesService _favouriteService;
        public IProductService _productService;
        private readonly UserManager<AppUser> _userManager;

        public FavouriteController(IFavouritesService favouritesService, IProductService productService, UserManager<AppUser> userManager)
        {
            _favouriteService = favouritesService;
            _productService = productService;
            _userManager = userManager;
        }
        public int GetCurrentUserId()
        {
            int.TryParse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);
            return userId;
        }
        // GET: FavouriteController
        public ActionResult Index()
        {
            var userId = GetCurrentUserId(); ;
            return View(_favouriteService.GetAllFavourites(userId));//merr listen e favourites per userin e loguar
        }

        // GET: FavouriteController/Create

        public JsonResult Create(int id)
        {
            var userId = GetCurrentUserId();
            var userFavourites = _favouriteService.GetAllFavourites(userId);//merr favourites te userit aktual te loguar
            if (userFavourites.FirstOrDefault(x => x.ProductId == id) != null)
            {
                _favouriteService.DeleteFavourite(id, userId);
                return Json(new Dictionary<string, string>()
                {
                    {"status", "completed" },
                    {"message", "deleted" }
                });
            }
            var addedFavourite = new FashionApp.BLL.DTO.Favourite()
            {
                ProductId = id,
                UserId = userId
            };
            if (_favouriteService.AddFavourite(addedFavourite).Status == ViewResponseStatus.OK)
            {
                return Json(new Dictionary<string, string>()
                {
                    {"status", "completed" },
                    {"message", "added" }
                });
            }
            return Json(new Dictionary<string, string>()
                {
                    {"status", "failed" },
                    {"message", "Action could not be completed" }
                });
        }

        // GET: FavouriteController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (_favouriteService.DeleteFavourite(id, userId).Status == ViewResponseStatus.OK)
                {
                    return Json(new Dictionary<string, string>()//shkon si response te view/index i favourites
                    {
                        {"status", "completed" }
                    });
                }
            }
            catch (Exception){}
            return Json(new Dictionary<string, string>()
            {
                {"status", "failed" }
            });

        }
    }
}
