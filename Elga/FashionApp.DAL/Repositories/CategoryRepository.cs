using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace FashionApp.DAL.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category, int>{
        public new Task<List<Category>> GetAll();
    }
    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext){}

        public new async Task<List<Category>> GetAll()
        {
            return await _set.ToListAsync();
        }
    }
}
