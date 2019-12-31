using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using ThAmCo.Customer.Services.Auth;
using ThAmCo.Customer.Services.Brands;
using ThAmCo.Customer.Services.Categories;
using ThAmCo.Customer.Services.Orders;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Services.Profiles;
using ThAmCo.Customer.Services.Reviews;

namespace ThAmCo.Customer.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication("Cookies")
                    .AddCookie("Cookies");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (Configuration["UseFakeProducts"] == "true")
            {
                services.AddTransient<IProductsService, FakeProductsService>();
            }
            else
            {
                services.AddHttpClient<IProductsService, ProductsService>(c =>
                        {
                            c.BaseAddress = new System.Uri(Configuration["ProductsAPI"]);
                            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        })
                        .AddTransientHttpErrorPolicy(p =>
                            p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                        .AddTransientHttpErrorPolicy(p =>
                            p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            }

            if (Configuration["UseFakeBrandsCategories"] == "true")
            {
                services.AddTransient<IBrandsService, FakeBrandsService>();
                services.AddTransient<ICategoriesService, FakeCategoriesService>();
            }
            else
            {
                services.AddHttpClient<IBrandsService, BrandsService>(c =>
                        {
                            c.BaseAddress = new System.Uri(Configuration["ProductsAPI"]);
                            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        })
                        .AddTransientHttpErrorPolicy(p =>
                            p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                        .AddTransientHttpErrorPolicy(p =>
                            p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
                services.AddHttpClient<ICategoriesService, CategoriesService>(c =>
                        {
                            c.BaseAddress = new System.Uri(Configuration["ProductsAPI"]);
                            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        })
                        .AddTransientHttpErrorPolicy(p =>
                            p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                        .AddTransientHttpErrorPolicy(p =>
                            p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            }

            if (Configuration["UseFakeReviews"] == "true")
            {
                services.AddTransient<IReviewsService, FakeReviewsService>();
            }
            else
            {
                services.AddHttpClient<IReviewsService, ReviewsService>(c =>
                        {
                            c.BaseAddress = new System.Uri(Configuration["ReviewsAPI"]);
                            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        })
                        .AddTransientHttpErrorPolicy(p =>
                            p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                        .AddTransientHttpErrorPolicy(p =>
                            p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            }

            if (Configuration["UseFakeOrders"] == "true")
            {
                services.AddTransient<IOrdersService, FakeOrdersService>();
            }
            else
            {
                services.AddHttpClient<IOrdersService, OrdersService>(c =>
                        {
                            c.BaseAddress = new System.Uri(Configuration["OrdersAPI"]);
                            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        })
                        .AddTransientHttpErrorPolicy(p =>
                            p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                        .AddTransientHttpErrorPolicy(p =>
                            p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            }

            if (Configuration["UseFakeProfiles"] == "true")
            {
                services.AddTransient<IProfilesService, FakeProfilesService>();
            }
            else
            {
                services.AddHttpClient<IProfilesService, ProfilesService>(c =>
                        {
                            c.BaseAddress = new System.Uri(Configuration["ProfilesAPI"]);
                            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        })
                        .AddTransientHttpErrorPolicy(p =>
                            p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                        .AddTransientHttpErrorPolicy(p =>
                            p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            }

            services.AddHttpClient<IAuthService, AuthService>(c =>
                    {
                        c.BaseAddress = new System.Uri(Configuration["AuthServer"]);
                    })
                    .AddTransientHttpErrorPolicy(p =>
                        p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                    .AddTransientHttpErrorPolicy(p =>
                        p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
