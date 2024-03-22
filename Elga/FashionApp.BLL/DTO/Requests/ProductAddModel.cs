using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.BLL.DTO.Requests
{
    public class ProductAddModel
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Precision(18, 2)]
		[Range(0.0, 100, ErrorMessage = "The field {0} must be greater than {1}.")]
		public decimal Price { get; set; }
        [DisplayName("Kategoria")]
        public int CategoryId { get; set; }
        [Range(0, 100)]
        public int Quantity { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }

    }
}
