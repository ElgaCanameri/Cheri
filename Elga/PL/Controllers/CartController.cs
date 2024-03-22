using System.Security.Claims;
using FashionApp.BLL.DTO;
using FashionApp.BLL.Services;
using FashionApp.DAL.Entities;
using FashionApp.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PL.Models;
using Stripe;
using Stripe.Checkout;

namespace PL.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        public ICartService _cartService;
        public IProductService _productService;
        public ICartProductService _cartProductService;
        public IOrderService _orderService;
        private readonly StripeSettings _stripeSettings;
        private readonly UserManager<AppUser> _userManager;

        public CartController(ICartService cartService, IProductService productService, ICartProductService cartProductService, IOrderService orderService, IOptions<StripeSettings> stripeSettings, UserManager<AppUser> userManager)
        {
            _cartService = cartService;
            _productService = productService;
            _cartProductService = cartProductService;
            _orderService = orderService;
            _stripeSettings = stripeSettings.Value;
            _userManager = userManager;
        }

        public int GetCurrentUserId()
        {
            int.TryParse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);
            return userId;
        }

        // GET: CartController
        public ActionResult Index()
        {
            var userId = GetCurrentUserId();
            var cartProducts = _cartProductService.GetAllCartProductsByUser(userId);
            return View(cartProducts);
        }

        // GET: CartController/Create
        public ActionResult Create(int id)
        {
            var userId = GetCurrentUserId();
            var cartProduct = _cartProductService.GetProductById(id, userId);
            var res = new StandardViewResponse<bool>(false, "Initial");
            if (cartProduct != null)
            {
                res = _cartService.UpdateCart(id, userId, cartProduct.Quantity, "plus");
            }
            else
            {
                res = _cartService.AddProduct(userId, id);
            }

            if (res.Status == FashionApp.BLL.DTO.ViewResponseStatus.OK)
            {
                return Json(new Dictionary<string, string>()
                {
                    {"status", "completed"}
                });
            }
            return Json(new Dictionary<string, string>()
            {
                {"status", "failed"}
            });
        }

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            var userId = GetCurrentUserId();
            var res = _cartProductService.DeleteCartProductByUserId(userId, id);

            if (res.Status == FashionApp.BLL.DTO.ViewResponseStatus.OK)
            {
                return Json(new Dictionary<string, string>()
                {
                    {"status", "completed"}
                });
            }
            return Json(new Dictionary<string, string>()
            {
                {"status", "failed"}
            });
        }

        public ActionResult UpdateCart(int prodId, int quantity, string op)
        {
            try
            {
                var userId = GetCurrentUserId();
                var res = _cartService.UpdateCart(prodId, userId, quantity, op);
                return Json(new Dictionary<string, string>(){
                    {"status", "completed"}
                });
            }
            catch (Exception)
            {
                return Json(new Dictionary<string, string>(){
                    {"status", "failed"}
                });
            }
        }
        //shtimi i api stripe
        public IActionResult CreateCheckoutSession(string amount)
        {

            var currency = "eur"; // Currency code
            var successUrl = "https://localhost:7018/Cart/OrderConfirmation";
            var cancelUrl = "https://localhost:7018/Cart/OrderConfirmation";
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = (long)Convert.ToDouble(amount) * 100,  // Amount in smallest currency unit (e.g., cents)
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Cheri",
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            var service = new SessionService();
            var session = service.Create(options);
            TempData["Session"] = session.Id;
            TempData["SessionAmount"] = amount;

            return Redirect(session.Url);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            var session = service.Get(TempData["Session"].ToString());
            double.TryParse(TempData["SessionAmount"].ToString(), out double sessionAmount);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                var userId = GetCurrentUserId();

                _orderService.AddOrder(new Order
                {
                    UserId = userId,
                    OrderItems = _cartService.GetAllProducts(userId).Select(x => new OrderItem
                    {
                        ProductId = x.Id,
                        Quantity = x.Quantity ?? 0,
                    }).ToList(),
                    TotalPrice = sessionAmount,
                    CreatedOn = DateTime.Now
                });

                _cartService.ClearUserCart(userId);

                return RedirectToAction("Success", "Cart");
            }
            return RedirectToAction("Fail", "Cart");
        }


		public ActionResult Success()
		{
			return View();
		}
        
        public ActionResult Fail()
		{
			return View();
		}
	}
}
