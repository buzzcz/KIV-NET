using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublicationGui.Controllers;
using PublicationsCore.facade;
using PublicationsCore.Service;
using PublicationsCore.Utils;

namespace PublicationGui
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
            services.AddMvc().AddControllersAsServices();
            services.AddSingleton(new MapperConfiguration(cfg => cfg.AddProfile(new CoreMappingProfile())).CreateMapper());
            services.AddSingleton<IPublicationFacade, PublicationFacade>();
            services.AddSingleton<IPublicationService, PublicationService>();
            services.AddSingleton<IAuthorFacade, AuthorFacade>();
            services.AddSingleton<IAuthorService, AuthorService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<ICitationFacade, CitationFacade>();
            services.AddSingleton<ICitationService, CitationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Publications/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Publications}/{action=Index}/{id?}");
            });
        }
    }
}