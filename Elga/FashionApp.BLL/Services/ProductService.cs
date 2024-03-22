using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using FashionApp.BLL.DTO;
using FashionApp.BLL.DTO.Requests;
using FashionApp.BLL.DTO.Responses;
using FashionApp.DAL;
using FashionApp.DAL.Entities;

namespace FashionApp.BLL.Services
{
    public interface IProductService
    {
        StandardViewResponse<bool> AddProduct(DTO.Requests.ProductAddModel model);
        StandardViewResponse<bool> EditProduct(int id, DTO.Requests.ProductAddModel model);
        public IEnumerable<ProductIndexModel> GetAllProducts();
        public ProductDetailsModel GetById(int id);
        public StandardViewResponse<bool> DeleteProduct(int id);
        public Task<List<ProductIndexModel>> Filter(int categoryId, string name);


    }
    public class ProductService : BaseService, IProductService
    {
		public static event IWriteLog OnLogOccured;
		public ProductService(IServiceProvider unitOfWork) : base(unitOfWork)
        {

        }

        public ProductDetailsModel GetById(int id)
        {
            var product = new Product();
            try
            {
				product = _unitOfWork.ProductsRepository.GetById(id);
				var prod = new ProductDetailsModel()
				{
                    Id = id,
					Title = product.Title,
					Description = product.Description,
					Price = product.Price,
					CategoryId = product.CategoryId,
					ImagePath = product.ImagePath
				};
				OnLogOccured?.Invoke(new DAL.Entities.AuditLog
				{
					Description = "U pa nje produkt",
					Name = nameof(prod),
					ObjectId = prod.Id,
                    Type = "Debug"
				});
				return prod;
			}
			catch(Exception ex) {
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                {
                    Description = ex.Message,
                    Name = nameof(product),
                    ObjectId = product.Id,
                    Type = "Error"
                });
            }
            return null;
        }

        public IEnumerable<ProductIndexModel> GetAllProducts()
        {
            var products = new List<Product>();
            try
            {
                products = _unitOfWork.ProductsRepository.GetAll().Where(x => x.IsDeleted == false).ToList();
                var list = products.Select(x => new ProductIndexModel//e kthen nga produktet e repository ne nje productindexmodel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    ImagePath = x.ImagePath
                }).Reverse(); 
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
				{
					Description = "U shfaq lista e produkteve",
					Name = nameof(list),
					ObjectId = 0,
					Type = "Debug"
				});
				return list;
            }
            catch (Exception ex) {
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                {
                    Description = ex.Message,
                    Name = nameof(products),
                    ObjectId = 0,
                    Type = "Error"
                });
            }
            return null;
        }
        public StandardViewResponse<bool> AddProduct(ProductAddModel model)
        {
            var addedProduct = new Product();
            try
            {
                addedProduct = new DAL.Entities.Product
                {
                    Title = model.Title,
                    Price = model.Price,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    ImagePath = model.ImagePath
                };

                _unitOfWork.ProductsRepository.Add(addedProduct);
                _unitOfWork.Commit();

                if (addedProduct.Id > 0)
                {
                    OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                    {
                        Description = "U shtua nje produkt",
                        Name = nameof(addedProduct),
                        ObjectId = addedProduct.Id,
                        Type = "Debug"
                    });
                    return new StandardViewResponse<bool>(true);
                }
			}
            catch (Exception ex) {
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                {
                    Description = ex.Message,
                    Name = nameof(addedProduct),
                    ObjectId = addedProduct.Id,
                    Type = "Error"
                });
            }
            return new StandardViewResponse<bool>(false, "Nuk u shtua dot produkti.");
        }
        
        public StandardViewResponse<bool> EditProduct(int id, ProductAddModel model)
        {
            var product = new Product();
            try
            {
                product = _unitOfWork.ProductsRepository.GetById(id);
                product.Title = model.Title;
                product.Price = model.Price;
                product.Description = model.Description;
                product.CategoryId = model.CategoryId;  
                _unitOfWork.ProductsRepository.Update(product);

                _unitOfWork.Commit();
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                {
                    Description = "U modifikua nje produkt",
                    Name = nameof(product),
                    ObjectId = product.Id,
                    Type = "Debug"
                });
                return new StandardViewResponse<bool>(true);
			}
            catch (Exception ex) {
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                {
                    Description = ex.Message,
                    Name = nameof(product),
                    ObjectId = product.Id,
                    Type = "Error"
                });
            }
			return new StandardViewResponse<bool>(false, "Nuk u perditesua dot produkti.");
		}

        public StandardViewResponse<bool> DeleteProduct(int id)
        {
            var product = new Product();
            try 
            {
                product = _unitOfWork.ProductsRepository.GetById(id);
                product.IsDeleted = true;
                _unitOfWork.ProductsRepository.Update(product);
                _unitOfWork.Commit();
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                {
                    Description = "U fshi nje produkt",
                    Name = nameof(product),
                    ObjectId = product.Id,
                    Type = "Debug"
                });
                return new StandardViewResponse<bool>(true);


			}
            catch (Exception ex){
                OnLogOccured?.Invoke(new DAL.Entities.AuditLog
                {
                    Description = ex.Message,
                    Name = nameof(product),
                    ObjectId = product.Id,
                    Type = "Error"
                });
            }
            return new StandardViewResponse<bool>(false, "Nuk u fshi dot produkti.");
        }

		public async Task<List<ProductIndexModel>> Filter(int categoryId, string name)
		{
            try {
                var productList = (await _unitOfWork.ProductsRepository.Filter(categoryId, name)).Where(y => y.IsDeleted == false).Select(x => new ProductIndexModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    Title = x.Title,
                    ImagePath = x.ImagePath
                }).ToList();
				OnLogOccured?.Invoke(new DAL.Entities.AuditLog
				{
					Description = "U shfaq lista e produkteve",
					Name = nameof(productList),
					ObjectId = 0,//skemi id se esht list jo objekt
					Type = "Debug"
				});
				return productList;
            }
            catch (Exception){ }
            return new List<ProductIndexModel>();
		}
	}
}
