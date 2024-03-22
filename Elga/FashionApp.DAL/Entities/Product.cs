using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
		public string ImagePath { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
	}
}
