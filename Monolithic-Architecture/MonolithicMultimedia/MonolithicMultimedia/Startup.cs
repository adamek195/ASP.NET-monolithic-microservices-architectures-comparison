using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonolithicMultimedia.Data;
using MonolithicMultimedia.Repositories.Interfaces;
using MonolithicMultimedia.Repositories;
using MonolithicMultimedia.Services.Interfaces;
using MonolithicMultimedia.Services;
using MonolithicMultimedia.Mappings;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Exceptions.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using MonolithicMultimedia.Settings;

namespace MonolithicMultimedia
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
            services.AddDbContext<MonolithicMultimediaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MonolithicMultimediaCS")));

            services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<MonolithicMultimediaContext>();

            var dockerMediaRepositorySettings = new DockerMediaRepositorySettings();
            Configuration.GetSection("DockerMediaRepositorySettings").Bind(dockerMediaRepositorySettings);
            services.AddTransient(x => dockerMediaRepositorySettings);

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IImagesRepository, ImagesRepository>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<IVideosRepository, VideosRepository>();
            services.AddTransient<IVideosService, VideosService>();

            services.AddSingleton(AutoMapperConfig.Initialize());

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
                    option.ExpireTimeSpan = System.TimeSpan.FromMinutes(60);
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

            DataBaseMigrator.AddMigration(app);

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
