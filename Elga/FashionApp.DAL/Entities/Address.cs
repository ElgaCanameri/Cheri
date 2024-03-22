using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL.Entities
{
    public class Address : BaseEntity<int>
    {
        [ForeignKey("City")]
        public int CityId { get; set; }
        public City City { get; set; }

        [ForeignKey("Account")]
        public int UserId { get; set; }
        public AppUser Account { get; set; }
        [StringLength(100)]
        public string Destination { get; set; }

    }
}
