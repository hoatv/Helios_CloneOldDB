using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Helios_CloneOldDB.Controllers;
using Helios_CloneOldDB.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Helios_CloneOldDB
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
			//var redisHost = Configuration.GetValue<string>("Redis:Host");
			//var redisPort = Configuration.GetValue<int>("Redis:Port");
			//var redisIpAddress = Dns.GetHostEntryAsync(redisHost).Result.AddressList.Last();
			//var redis = ConnectionMultiplexer.Connect($"{redisIpAddress}:{redisPort}");

			//services.AddDataProtection().PersistKeysToRedis(redis, "DataProtection-Keys");
			services.AddMvc();
			services.AddOptions();
			//services
        	//.AddDataProtection(opt => opt.ApplicationDiscriminator = "your-app-id")
        	//.ProtectKeysWithYourCustomKey()
        	//.PersistKeysToYourCustomLocation();
			//services.AddDataProtection();
			//services.BuildServiceProvider();
			//.SetApplicationName("helios-cloneolddb")
			//.PersistKeysToFileSystem(new DirectoryInfo(@"\\server\share\myapp-keys\"))
			//.ProtectKeysWithCertificate("thumbprint");
			//services.AddDataProtection().DisableAutomaticKeyGeneration();
			//services.AddTransient<HomeController>(_ => new AppDb(Configuration["ConnectionStrings:DefaultConnection"]));
			services.Add(new ServiceDescriptor(typeof(ContactContext), new ContactContext((Configuration.GetConnectionString("DefaultConnection")))));
			services.Add(new ServiceDescriptor(typeof(GetC2Context), new GetC2Context((Configuration.GetConnectionString("DefaultConnection")))));
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
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
