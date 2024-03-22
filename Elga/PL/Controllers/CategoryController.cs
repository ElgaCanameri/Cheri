using FashionApp.BLL.DTO.Requests;
using FashionApp.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
	{
		public ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
			_categoryService = categoryService;
        }
        // GET: CategoryController
        public ActionResult Index()
		{
			return View();
		}

		// GET: CategoryController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: CategoryController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: CategoryController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CategoryAddModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var res = _categoryService.AddCategory(model);
					if(res.Status == FashionApp.BLL.DTO.ViewResponseStatus.OK)
					{
						return RedirectToAction("Index", "Product");
					}
				}
			}
			catch
			{
				return View(model);
			}
			return View(model);
		}

		// GET: CategoryController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: CategoryController/Edit/5
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

		// GET: CategoryController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: CategoryController/Delete/5
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
	}
}
