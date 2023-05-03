using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Multimedia.Images.Data;
using Multimedia.Images.Exceptions.Filters;
using Multimedia.Images.Mappings;
using Multimedia.Images.Repositories;
using Multimedia.Images.Repositories.Interfaces;
using Multimedia.Images.Services;
using Multimedia.Images.Services.Interfaces;
using Multimedia.Images.Settings;
using System.Text;

namespace Multimedia.Images
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
            services.AddDbContext<ImagesMultimediaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ImagesMultimediaCS")));

            var mediaRepositorySettings = new MediaRepositorySettings();
            Configuration.GetSection("MediaRepositorySettings").Bind(mediaRepositorySettings);
            services.AddTransient(x => mediaRepositorySettings);

            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<IImagesRepository, ImagesRepository>();

            services.AddSingleton(AutoMapperConfig.Initialize());

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services.AddControllers();

            var key = Encoding.ASCII.GetBytes(Configuration["JwtToken:Key"]);

            services.AddAuthentication(options =>
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidIssuer = Configuration["JwtToken:TokenIssuer"],
                        ValidateAudience = false,
                        RequireExpirationTime = true
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
