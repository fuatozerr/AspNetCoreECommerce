    using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ECommerce.DataAccess.Abstract
{
    public interface IProductDal:IRepository<Product>
    {
        IEnumerable<Product> GetPopularProduct();

        Product GetProductDetails(int id);

        List<Product> GetProductsByCategory(string category,int page,int pagesize);
        int GetProductsByCategory(string category);
        Product GetByIdWithCategories(int id);
    }
}
