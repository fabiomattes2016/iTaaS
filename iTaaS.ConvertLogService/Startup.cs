using AutoMapper;
using iTaaS.ConvertLogService.Data;
using iTaaS.ConvertLogService.Repositories;
using iTaaS.ConvertLogService.Repositories.Interfaces;
using iTaaS.ConvertLogService.Services;
using iTaaS.ConvertLogService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace iTaaS.ConvertLogService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddScoped<IDestinationRepository, DestinationRepository>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IDestinationService, DestinationService>();
            services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Conversor de Logs", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger(); app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conversor de Logs"); });
                app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
            }

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
