using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using preparation.Models.Account;
using preparation.Models.Contexts;
using preparation.Services.Cart;
using preparation.Services.ExternalDB;
using preparation.Services.Messenger;
using preparation.Services.Recommender;
using preparation.Services.Streinger;
using preparation.Services.TopAlgorithm;

namespace preparation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SuppliersContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<MessengerContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<UserContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            services.AddIdentity<User, IdentityRole>((options) =>
                {
                    options.User.RequireUniqueEmail = true;    // уникальный email

                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                })
                .AddEntityFrameworkStores<UserContext>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("admin");
                    });
                options.AddPolicy("supplier",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("supplier");
                    });

            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
            });


            services.AddSingleton<IExternalDb, ExternalDb>();
            services.AddSingleton<IStreinger, Streinger>();
            services.AddSingleton<ITopAlgorithm, TopAlgorithm>();
            services.AddSingleton<IRecommender, Recommender>();
            services.AddScoped<IMessenger, Messenger>();
            services.AddScoped<ICart, Cart>((s) => new Cart());

            services.AddHttpClient();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();


            app.UseMvc(router =>
            {
                router.MapRoute(name:"default",template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
