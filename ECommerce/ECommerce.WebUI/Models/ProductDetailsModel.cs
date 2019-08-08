using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Models
{
    public class ProductDetailsModel
    {
        public Product Product { get; set; }

        public List<Category> Categories { get; set; }
    }
}
