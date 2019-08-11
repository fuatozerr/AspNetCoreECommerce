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
        private ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
                _categoryService = categoryService;
        }

        public IActionResult ProductList()
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
            return Redirect("ProductList");

        }


        public IActionResult EditProduct(int? id)
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
        public IActionResult EditProduct(ProductModel model)
        {
            var entity = _productService.GetById(model.Id);

            entity.Name = model.Name;
            entity.ImageUrl = model.ImageUrl;
            entity.Price = model.Price;
            entity.Description = model.Description;


            _productService.Update(entity);
            return RedirectToAction("EditProduct", entity);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);

            _productService.Delete(entity);

            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryList()
            {
                Categories=_categoryService.GetAll()
            });;
        }
        
        public IActionResult CreateCategory()
        {
            return View();

        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name=model.Name
            };
            _categoryService.Create(entity);
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var entity = _categoryService.GetByIdWithProducts(id);

            return View(new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            }) ; 



        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {

            var entity = _categoryService.GetById(model.Id);

            entity.Name = model.Name;
            _categoryService.Update(entity);


            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

             _categoryService.Delete(entity);

            return RedirectToAction("CategoryList");

        }
    }
}