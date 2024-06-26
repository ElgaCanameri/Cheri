﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionApp.BLL.DTO.Responses
{
    public class CategoryIndexModel
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
    }
}
