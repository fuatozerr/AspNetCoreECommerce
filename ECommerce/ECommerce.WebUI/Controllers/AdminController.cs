using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Business.Abstract;
using ECommerce.Entities;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Http;
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
                Products = _productService.GetAll()
            });
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();

        }
        [HttpPost]
        public async Task< IActionResult > CreateProduct(ProductModel model,IFormFile file)
        {
                if (file != null)
                {
                  
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                var entity = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImageUrl = file.FileName,
                    Price = model.Price
                };
                _productService.Create(entity);

                }
                return Redirect("ProductList");
        }


        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                Name = entity.Name,
                Price = entity.Price,
                SelectedCategories = entity.ProductCategories.Select(x => x.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult>EditProduct(ProductModel model, int[] categoryIds,IFormFile file)
        {
            if(ModelState.IsValid)
            {

            

            var entity = _productService.GetById(model.Id);

            entity.Name = model.Name;
            entity.ImageUrl = model.ImageUrl;
            entity.Price = model.Price;
            entity.Description = model.Description;
            if(file!=null)
                {
                    entity.ImageUrl = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using(var stream=new FileStream(path,FileMode.Create))
                    {
                      await file.CopyToAsync(stream);
                    }
                }

            _productService.Update(entity,categoryIds);
            return RedirectToAction("EditProduct", entity);
        }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
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
                Categories = _categoryService.GetAll()
            }); ;
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
                Name = model.Name
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
            });



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

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId,int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);

            return Redirect("/admin/editcategory/"+categoryId);
        }
    }
}