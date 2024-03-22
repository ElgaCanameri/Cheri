using FashionApp.BLL.DTO.Requests;
using FashionApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productsService;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IProductService productsService, IWebHostEnvironment hostEnvironment)//DI - injects IProductService interface to ProductController
        {
            _productsService = productsService;
            webHostEnvironment = hostEnvironment;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            return View(_productsService.GetAllProducts());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
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
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();
            }
            return View(model);
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string UploadedFile(ProductAddModel model)
        {
            string uniqueFileName = null;//vendos try catch

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
            return uniqueFileName;
        }
    }
}
