using FashionApp.BLL.DTO.Requests;
using FashionApp.BLL.DTO.Responses;
using FashionApp.BLL.Services;
using FashionApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productsService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment webHostEnvironment;
        //**************************
        public ProductController(IProductService productsService, ICategoryService categoryService, IWebHostEnvironment hostEnvironment)//DI - injects IProductService interface to ProductController
        {
            _productsService = productsService;
            _categoryService = categoryService;
            webHostEnvironment = hostEnvironment;
        }
        // GET: ProductController
        public async Task<IActionResult> Index(int categories, string search)
        {
            var products = await _productsService.Filter(categories, search);
			ViewBag.Categories = await _categoryService.GetAllCategories();
            return View(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View(_productsService.GetById(id));
        }

        // GET: ProductController/Add
        [HttpGet]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
		[Authorize(Roles = "Admin,Staff")]
		public IActionResult Add(BLL.DTO.Requests.ProductAddModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImagePath = UploadedFile(model);
                var res = _productsService.AddProduct(model);
                if(res.Status == BLL.DTO.ViewResponseStatus.OK)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: ProductController/Edit/5
        [HttpGet]
		[Authorize(Roles = "Admin,Staff")]
		public async Task<IActionResult> Edit(int id)
        {
            var existingById = _productsService.GetById(id);
            ViewBag.Categories = await _categoryService.GetAllCategories();
            if (existingById != null)
            {
                return View(new ProductAddModel
                {
                    Title = existingById.Title,
                    Description = existingById.Description, 
                    Price = existingById.Price,
                    CategoryId = existingById.CategoryId,
                    Quantity = existingById.Quantity,
                    ImagePath = existingById.ImagePath
                });
            }
			return RedirectToAction(nameof(Index));
		}

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Staff")]
		public ActionResult Edit(int id, ProductAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var res = _productsService.EditProduct(id, model);
			if (res.Status == BLL.DTO.ViewResponseStatus.OK)
            {
				return RedirectToAction("Index");
			}
            return View(model);
		}

        // GET: ProductController/Delete/5
        [HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
        {
            var existingById = _productsService.GetById(id);
            if (existingById != null)
            {
                return View(_productsService.GetById(id));
            }
            return RedirectToAction("Index");
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id, ProductAddModel model)
        {
            try
            {
                var product = _productsService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [Authorize(Roles = "Admin, Staff")]
        private string UploadedFile(ProductAddModel model)
        {
            string uniqueFileName = null;
            try
            {
                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception) { }

            return uniqueFileName;
        }
    }
}
