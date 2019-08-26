using ECommerce.DataAccess.Abstract;
using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Concrete.EfCore
{
    public class EfCoreCartDal:EfCoreGenericRepository<Cart,ShopContext>,ICartDal
    {
    }
}
