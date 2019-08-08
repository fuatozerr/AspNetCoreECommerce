using ECommerce.DataAccess.Abstract;
using ECommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ECommerce.DataAccess.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>,   IProductDal
    {
        public IEnumerable<Product> GetPopularProduct()
        {
            throw new NotImplementedException();
        }

        public Product GetProductDetails(int id)
        {
            using(var context=new ShopContext())
            {
                return context.Products.Where(x => x.Id == id)
                    .Include(x => x.ProductCategories)
                    .ThenInclude(x => x.Category)
                    .FirstOrDefault();
            }
        }
    }
}
