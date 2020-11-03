using LuminiHire.Models;
using LuminiHire.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace LuminiHire.Configurations
{
	public static class ElasticsearchExtensions
	{
		public static void AddElasticsearch(
		 this IServiceCollection services, IConfiguration configuration)
		{
			var url = configuration["elasticsearch:url"];
			var defaultIndex = configuration["elasticsearch:index"];

			var settings = new ConnectionSettings(new Uri(url))
				 .DefaultIndex(defaultIndex)
				 .DefaultMappingFor<Schools>(m => m.PropertyName(p => p.unitid, "unitid"));

			var client = new ElasticClient(settings);
			services.AddSingleton<IElasticClient>(client);

			var schoolService = new SchoolService(client);
			services.AddSingleton<ISchoolService>(schoolService); 
		}
	}
}
