using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FashionApp.DAL.Entities
{
    public class AppUser : IdentityUser<int>
    {
        
    }

    public class AppRole : IdentityRole<int>
    {

    }
}
