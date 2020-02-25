using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using BookInfo.Repositories;
using BookInfo.Models;

namespace BookInfo
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
            => Configuration = configuration;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();  // In 3.1, this can be AddControllers() to reduce framework overhead

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(
                        Configuration["ConnectionStrings:MsSqlConnection"]));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(
                   options => options.UseSqlite(
                       Configuration["ConnectionStrings:SQLiteConnection"]));
            }

            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           app.UseMvc();
           /* We aren't using app.UseMvcWithDefaultRoute() because it defines a default route with the template: '{controller=Home}/{action=Index}/{id?}'
              but, we no longer have a Home controller, instead we have an Index.html page in wwwroot. */

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
