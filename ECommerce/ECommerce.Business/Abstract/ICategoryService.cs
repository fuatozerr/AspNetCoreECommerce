using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Business.Abstract
{
   public interface ICategoryService
    {
        List<Category> GetAll();

        Category GetByIdWithProducts(int id);
        void Create(Category entity);
        void Update(Category entity);

        void Delete(Category entity);

        Category GetById(int id);
    }
}
