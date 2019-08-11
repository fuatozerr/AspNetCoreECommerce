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
        
        //sayfalama ve categori filtreleme ürün/telefon?page=1
        public IActionResult List(string category,int page=1)
        {
            const int pagesize = 3;
            return View(new ProductListModel()
            {
                PageInfo =new PageInfo()
                {
                    TotalItems=_productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage=pagesize,
                    CurrentCategory=category



                },

                Products = _productService.GetProductsByCategory(category,page, pagesize)

            });
        }


    }
}