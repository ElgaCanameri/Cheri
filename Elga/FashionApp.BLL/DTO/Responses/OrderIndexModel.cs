using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.BLL.DTO.Responses
{
    public class OrderIndexModel
    {
        public int UserId { get; set; }
        public AppUser Account { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        [Precision(10, 2)]
        public double TotalPrice { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
