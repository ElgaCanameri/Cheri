using System.ComponentModel.DataAnnotations.Schema;

namespace FashionApp.DAL.Entities
{
    public class OrderItem : BaseEntity<int>
    {

        public int Quantity { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
