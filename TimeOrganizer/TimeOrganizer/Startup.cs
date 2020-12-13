using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using TimeOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.SqlRepository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TimeOrganizer.Model.Tables;
using TimeOrganizer.Settings;
using System.Configuration;

namespace TimeOrganizer
{
    public class Startup
    {
        private IConfiguration conf;

        public Startup(IConfiguration conf)
        {
            this.conf = conf;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(conf.GetConnectionString("TimeOrganizerDatabase")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<ITaskRepository, SqlTaskRepository>();
            services.AddScoped<ITaskTypeRepository, SqlTaskTypeRepository>();
            services.AddScoped<IColorRepository, SqlColorRepository>();
            services.AddScoped<IApplicationUserTaskRepository, SqlApplicationUserTaskRepository>();
            services.AddScoped<IUserRelationshipRepository, SqlUserRelationshipRepository>();
            services.Configure<MailSettings>(conf.GetSection("MailSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
