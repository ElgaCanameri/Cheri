using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.BLL.DTO;
using FashionApp.BLL.DTO.Requests;
using FashionApp.BLL.DTO.Responses;

namespace FashionApp.BLL.Services
{
    public interface ICategoryService
    {
        public StandardViewResponse<bool> AddCategory(CategoryAddModel model);
        public Task<List<CategoryIndexModel>> GetAllCategories();
    }
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IServiceProvider unitOfWork) : base(unitOfWork) { }


        public StandardViewResponse<bool> AddCategory(CategoryAddModel model)
        {
            try
            {
                var category = new DAL.Entities.Category()
                {
                    Name = model.Name,
                    Description = model.Description
                };
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Commit();

                if (category.Id > 0)
                {
                    return new StandardViewResponse<bool>(true);
                }
            }
            catch (Exception) { }
            return new StandardViewResponse<bool>(false, "The category could not be added");
        }

        public async Task<List<CategoryIndexModel>> GetAllCategories()
        {

            try
            {
                var categories = (await _unitOfWork.CategoryRepository.GetAll()).Select(x => new CategoryIndexModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return categories;
            }
            catch (Exception) { }
            return new List<CategoryIndexModel>();
        }
    }
}
