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
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
    {
        public Category GetByIdWithProducts(int id)
        {
           using(var context=new ShopContext())
            {
                return context.Categories.Where(x => x.Id == id)
                    .Include(x => x.ProductCategories)
                    .ThenInclude(i => i.Product).FirstOrDefault(); 
            }

        }
    }
}
