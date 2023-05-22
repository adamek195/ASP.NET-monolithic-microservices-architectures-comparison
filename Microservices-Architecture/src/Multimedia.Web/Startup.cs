using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multimedia.Web.Exceptions.Filters;
using Multimedia.Web.Services;
using Multimedia.Web.Services.Interfaces;
using Multimedia.Web.Settings;

namespace Multimedia.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IUsersService, UsersService>();
            services.AddHttpClient<IImagesService, ImagesService>();
            services.AddHttpClient<IVideosService, VideosService>();
            SD.APIBase = Configuration["ServiceUrls:API"];

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<IVideosService, VideosService>();

            var repositorySettings = new RepositorySettings();
            Configuration.GetSection("RepositorySettings").Bind(repositorySettings);
            services.AddTransient(x => repositorySettings);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services.AddControllersWithViews();

            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Account/Login";
                    option.ExpireTimeSpan = System.TimeSpan.FromMinutes(20);
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
