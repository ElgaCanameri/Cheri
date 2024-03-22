using System.Security.Claims;
using FashionApp.BLL.Services;
using FashionApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin,User")]

    public class OrderController : Controller
    {
        public IOrderService _orderService;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(IOrderService orderService, UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }
        public int GetCurrentUserId()
        {
            int.TryParse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);
            return userId;
        }
        // GET: OrderController
        public ActionResult Index()
        {
           
            var orders = new List<Order>();
            if(User.IsInRole("Admin"))
            {
                orders = _orderService.GetAllOrders();
            }
            else
            {
                var userId = GetCurrentUserId();
                orders = _orderService.GetAllOrdersByUserId(userId);
            }
            return View(orders);
        }
    }
}
