using Elasticsearch.Net;
using LuminiHire.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LuminiHire.Services
{
	public interface ISchoolService
	{
		Task Salvar(Schools schools);
		Task Deletar(Schools schools);
		Task<ISearchResponse<Schools>> GetAll();

		Task<ISearchResponse<Schools>> Find(string query, int page = 1, int pageSize = 5);
	}
	public class SchoolService : ISchoolService
	{
		IElasticClient _elasticClient;
		public SchoolService(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}

		public async Task Salvar(Schools schools)
		{
			string filePath = Path.GetDirectoryName("data");
			schools.UltimaAtualizacao = DateTime.UtcNow;

			bool schoolExists = File.Exists(filePath);

			if (schoolExists)
				await _elasticClient.UpdateAsync<Schools>(schools, u => u.Doc(schools));
			else
				await _elasticClient.IndexDocumentAsync(schools);
		}

		public async Task Deletar(Schools schools)
		{
			string filePath = Path.GetDirectoryName("data");

			if (File.Exists(filePath))
				File.Delete(filePath);

			await _elasticClient.DeleteAsync<Schools>(schools);
		}

		public async Task<ISearchResponse<Schools>> GetAll()
		{
			var response = await _elasticClient.SearchAsync<Schools>(
										 s => s.Query(q => q.QueryString(d => d.Query("")))
											  .From(0)
											  .Size(9999));
			return response;
		}

		public async Task<ISearchResponse<Schools>> Find(string query, int page = 1, int pageSize = 5)
		{
			var response = await _elasticClient.SearchAsync<Schools>(
		 s => s.Query(q => q.QueryString(d => d.Query(query)))
			  .From((page - 1) * pageSize)
			  .Size(pageSize));

			return response;
		}
	}
}