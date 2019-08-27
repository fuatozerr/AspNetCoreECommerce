using ECommerce.DataAccess.Abstract;
using ECommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerce.DataAccess.Concrete.EfCore
{
    public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
        public Cart GetByUserId(string userId)
        {
            using(var context =new ShopContext())
            {
                return context.Carts.Include(x => x.CartItems)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefault(i => i.UserId==userId);            }
        }
    }
}
