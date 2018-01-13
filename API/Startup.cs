using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using API.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace API
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
            //services.AddCors();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:50779", "http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            services.AddMvc()
           .AddXmlSerializerFormatters();

            services.AddSingleton<IPayStationRepo, PayStationRepository>();

            services.AddAuthentication()
              .AddJwtBearer(cfg =>
              {
                  cfg.RequireHttpsMetadata = false;
                  cfg.SaveToken = true;

                  cfg.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidIssuer = Configuration["Tokens:Issuer"],
                      ValidAudience = Configuration["Tokens:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                      //ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                  };

              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication(); //needs to be up in the pipeline, before MVC

            // Shows UseCors with named policy.
            app.UseCors("AllowSpecificOrigin");

            app.UseMvc();
           
            //// Shows UseCors with CorsPolicyBuilder.
            //app.UseCors(builder =>
            //   builder.WithOrigins("http://localhost:50076").AllowAnyHeader());

        }
    }
}
