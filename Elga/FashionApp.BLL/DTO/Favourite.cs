using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;

namespace FashionApp.BLL.DTO
{
	public class Favourite
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public int UserId { get; set; }
	}
}
