using System.ComponentModel.DataAnnotations.Schema;

namespace FashionApp.DAL.Entities
{
    public class Favourite : BaseEntity<int>
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Account")]
        public int UserId { get; set; }
        public AppUser Account { get; set; }
    }
}