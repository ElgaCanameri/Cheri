using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Entities
{
    public class Order : BaseEntity<int>
    {
        [ForeignKey("Account")]
        public int UserId { get; set; }
        public AppUser Account { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        [Precision(10, 2)]
        public double TotalPrice { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
