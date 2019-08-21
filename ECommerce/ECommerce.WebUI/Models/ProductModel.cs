using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60,MinimumLength =3,ErrorMessage ="Ürün ismi 3 ile 60 arası olmalı")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "Ürün ismi 10 ile 60 arası olmalı")]
        public string Description { get; set; }
        [Required(ErrorMessage ="Fiyat girin")]
        public decimal Price { get; set; }

        public List<Category> SelectedCategories{ get; set; }
    }
}
