using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.BLL.DTO.Responses
{
	public class ProductDetailsModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public int Quantity { get; set; }
		public IFormFile? Image { get; set; }
		public string? ImagePath { get; set; }
	}
}
