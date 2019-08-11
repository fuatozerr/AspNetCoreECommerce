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
    public class AdminController : Controller
    {
        private IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View(new ProductListModel()
            {
                Products=_productService.GetAll()
            });
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();

        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            var entity = new Product
            {
                Name = model.Name,  
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price
            };

            _productService.Create(entity);
            return Redirect("Index");

        }


        public IActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }


            var entity = _productService.GetById((int)id);

            if(entity==null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id=entity.Id,
                Description=entity.Description,
                ImageUrl=entity.ImageUrl,
                Name=entity.Name,
                Price=entity.Price
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(ProductModel model)
        {
            var entity = _productService.GetById(model.Id);

            entity.Name = model.Name;
            entity.ImageUrl = model.ImageUrl;
            entity.Price = model.Price;
            entity.Description = model.Description;


            _productService.Update(entity);
            return RedirectToAction("Edit",entity);
        }


       

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            var entity = _productService.GetById(productId);

            _productService.Delete(entity);

            return RedirectToAction("Index");
        }



    }
}