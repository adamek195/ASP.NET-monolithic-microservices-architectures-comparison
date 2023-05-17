using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Multimedia.Videos.Data;
using Multimedia.Videos.Exceptions.Filters;
using Multimedia.Videos.Mappings;
using Multimedia.Videos.Repositories;
using Multimedia.Videos.Repositories.Interfaces;
using Multimedia.Videos.Services;
using Multimedia.Videos.Services.Interfaces;
using Multimedia.Videos.Settings;
using System.Text;

namespace Multimedia.Videos
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
            services.AddDbContext<VideosMultimediaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("VideosMultimediaDockerCS")));

            var mediaRepositorySettings = new DockerMediaRepositorySettings();
            Configuration.GetSection("DockerMediaRepositorySettings").Bind(mediaRepositorySettings);
            services.AddTransient(x => mediaRepositorySettings);

            services.AddTransient<IVideosService, VideosService>();
            services.AddTransient<IVideosRepository, VideosRepository>();

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

            DataBaseMigrator.AddMigration(app);

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
