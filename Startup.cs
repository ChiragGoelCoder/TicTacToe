using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicTacToe.Services;
using TicTacToe.Extensions;
using Microsoft.AspNetCore.Routing;
using TicTacToe.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace TicTacToe
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
            services.AddControllersWithViews();
            //services.AddDirectoryBrowser();

            services.AddSingleton<IUserService, UserService>();

            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            var routeBuilder = new RouteBuilder(app);
            
            routeBuilder.MapGet("CreateUser", context =>
            {
                var firstName = context.Request.Query["firstName"];
                var lastName = context.Request.Query["lastName"];
                var email = context.Request.Query["email"];
                var password = context.Request.Query["password"];
                var userService = context.RequestServices.
                GetService<IUserService>();
                userService.RegisterUser(new UserModel
                {
                    FirstName =
                firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password
                });
                
                return context.Response.WriteAsync($"User {firstName} {lastName} has been successfully created.");
            });
            var newUserRoutes = routeBuilder.Build();
            //var options = new RewriteOptions().AddRewrite("NewUser", "/UserRegistration/Index", false);
            //app.UseRewriter(options);
            app.UseRouter(newUserRoutes);
            app.UseCommunicationMiddleware();
            app.UseStatusCodePages("text/plain", "HTTP Error - Status Code:{ 0}"); 


            //app.UseCommicationMiddleware();
            //app.UseDirectoryBrowser();
            app.UseRouting();

            app.UseAuthorization();
        

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
                //endpoints.MapControllers();
               
            });
        }
    }
}
