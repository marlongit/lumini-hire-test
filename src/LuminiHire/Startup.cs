using LuminiHire.Configurations;
using LuminiHire.data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace LuminiHire
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
			#region Inclusão do identity

			services.AddDbContext<AplicacaoDbContext>(options =>
				 options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
			
			services.AddIdentity<IdentityUser, IdentityRole>(options =>
			{
				options.User.RequireUniqueEmail = false;
			}).AddEntityFrameworkStores<AplicacaoDbContext>()
			  .AddDefaultTokenProviders();

			#endregion

			#region Politica de Cookies
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				  .AddCookie(options => options.LoginPath = "/Login");
			#endregion

			#region Elasticsearch

			services.AddElasticsearch(Configuration);

			#endregion

			#region Swagger
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(name: "v1", new OpenApiInfo
				{
					Title = "Lumini Hire Test",
					Description = "Essa api faz parte da prova para Fullstack Developer - Dev SR",
					Contact = new OpenApiContact() { Name = "Marlon Everson", Email = "marlon-marlon@hotmail.com" },
					License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/license/MIT") }
				});
			});
			#endregion

			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "v1");
			});

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

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
						 name: "default",
						 pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
