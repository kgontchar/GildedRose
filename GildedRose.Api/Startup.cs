using GildedRose.Api.Extensions;
using GildedRose.BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GildedRose.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.ConfigureDatabaseConnections(Configuration);
            services.ConfigureDependencyInjection();
            services.ConfigureAutoMapperProfiles();
            services.ConfigureCors();
            services.ConfigureAuthentication(Configuration);
            services.AddAuthorization().AddMvcCore(options => options.RespectBrowserAcceptHeader = true).AddJsonFormatters();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
            app.UseCors("AllowAllOrigin");
            var addFakeDataService = app.ApplicationServices.GetService<IAddFakeDataService>();
            addFakeDataService.AddFakeData();
        }
    }
}
