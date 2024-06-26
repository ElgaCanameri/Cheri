﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionApp.DAL.Entities
{
	public class CartProduct : BaseEntity<int>
	{
		// Foreign keys
		[ForeignKey("Cart")]
		public int CartId { get; set; }
		public Cart Cart { get; set; }
		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public int Quantity { get; set; }
	}
}
