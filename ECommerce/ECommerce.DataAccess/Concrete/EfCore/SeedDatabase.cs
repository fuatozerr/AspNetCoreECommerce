using ECommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerce.DataAccess.Concrete.EfCore
{
   public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if(context.Database.GetPendingMigrations().Count()==0)
            {
                if(context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categories);
                }


                if(context.Products.Count()==0)
                {
                    context.Products.AddRange(Products);
                }

                context.SaveChanges();
            }
        }

        private static Category[] Categories =
        {
            new Category(){ Name="Telefon"},
            new Category() { Name="Bilgisayar"}
        };

        private static Product[] Products =
        {
            new Product(){ Name="Samsun s5",Price=2000,ImageUrl="1.jpg"},
            new Product(){ Name="iphone s5",Price=3000,ImageUrl="1.jpg"},
            new Product(){ Name="xiomu s5",Price=5000,ImageUrl="1.jpg"},
            new Product(){ Name="Samsun s6",Price=8000,ImageUrl="1.jpg"},
            new Product(){ Name="Samsun s7",Price=10000,ImageUrl="1.jpg"},
            new Product(){ Name="Samsun s8",Price=5800,ImageUrl="1.jpg"},


        };
    }
}
