using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Repositories
{
    public interface IProductsRepository : IBaseRepository<Product, int>
    {
        public Task<List<Product>> Filter(int categoryId, string title);
    }

    public class ProductsRepository : BaseRepository<Product, int>, IProductsRepository
    {
        public ProductsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<Product>> Filter(int categoryId, string title)
        {
            var products = new List<Product>();
            if(categoryId != 0)
            {
                return await _set.Where(x => x.Title.Contains(title ?? "") && x.CategoryId == categoryId).ToListAsync();
            }
            return await _set.Where(x => x.Title.Contains(title ?? "")).ToListAsync();
        }
    }

}








