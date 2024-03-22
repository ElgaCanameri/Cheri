using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionApp.DAL.Entities
{
    public class City : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
