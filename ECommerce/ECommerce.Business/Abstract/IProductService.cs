using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Business.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        List<Product> GetAll();
        Product GetProductDetails(int id);
        List<Product> GetPopular();
        List<Product> GetProductsByCategory(string category);

        void Create(Product entity);
        void Update(Product entity);

        void Delete(Product entity);


    }
}
