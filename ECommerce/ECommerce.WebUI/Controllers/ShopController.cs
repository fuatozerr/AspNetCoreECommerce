using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Business.Abstract;
using ECommerce.Entities;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Details(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            Product product= _productService.GetProductDetails((int)id);

            if(product==null)
            {
                return NotFound();
            }

            return View(new ProductDetailsModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(x => x.Category).ToList()

            }); ;
        }
        
        public IActionResult List(string category)
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetProductsByCategory(category)

            });
        }


    }
}