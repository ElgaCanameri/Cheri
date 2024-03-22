using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Entities
{
    public class Cart : BaseEntity<int>
    {
        [ForeignKey("Account")]
        public int UserId { get; set; }
        public AppUser Account { get; set; }
		public List<CartProduct> CartProducts { get; set; }
	}
}
