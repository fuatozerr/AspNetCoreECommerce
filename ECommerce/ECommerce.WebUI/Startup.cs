using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Business.Abstract;
using ECommerce.Business.Concrete;
using ECommerce.DataAccess.Abstract;
using ECommerce.DataAccess.Concrete.EfCore;
using ECommerce.WebUI.Identity;
using ECommerce.WebUI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.DataAccess.Concrete.EfCore;

namespace ECommerce.WebUI
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                         options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<ApplicationIdentityDbContext>().
                AddDefaultTokenProviders();

        

            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.Lockout.MaxFailedAccessAttempts = 5; //yanlıs girme hakkı
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                //options.User.AllowedUserNameCharacters = ""; izin verdiğimiz karakterler 

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false; //onay için
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,//scriptler ulasamaz
                    Name = ".ECommerce.Security.Cookie",
                    SameSite=SameSiteMode.Strict //csrf token

                };

            });


            services.AddScoped<IProductDal, EfCoreProductDal>();
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<IProductService, ProductManager>();



            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }
            app.UseStaticFiles();

            app.CustomStaticFiles(); //middleware
            app.UseAuthentication();

            app.UseMvc(routes =>
            {


                routes.MapRoute(
                   name: "adminProducts",
                   template: "admin/products",
                   defaults: new { controller = "Admin", action = "ProductList" });

                routes.MapRoute(
                  name: "adminProducts",
                  template: "admin/products/{id?}",
                  defaults: new { controller = "Admin", action = "EditProduct" });



                routes.MapRoute(
                    name: "products",
                    template: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" });


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}