using LuminiHire.Models;
using LuminiHire.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuminiHire.Pages
{
	public class SearchModel : PageModel
	{
		public class Search
		{
			public string Query { get; set; } = string.Empty;
			public int PageSize { get; set; } = 5;
			public int Page { get; set; } = 1;
		}

		private readonly IElasticClient _elasticClient;
		private readonly ISchoolService _schoolService;
		public SearchModel(IElasticClient elasticClient, ISchoolService schoolService)
		{
			_elasticClient = elasticClient;
			_schoolService = schoolService;
			SchoolsData = new List<Schools>();
		}

		public void OnGet()
		{
		}

		[BindProperty]
		public IList<Schools> SchoolsData { get; set; }

		[BindProperty]
		public Search Filtro { get; set; }

		public async Task<IActionResult> OnPostAsync(string query, int page = 1, int pageSize = 5)
		{
			var result = await _schoolService.Find(Filtro.Query, Filtro.Page, Filtro.PageSize);

			if (result.IsValid)
			{
				foreach (var item in result.Documents)
				{
					SchoolsData.Add(item);
				}
			}

			return Page();
		}
	}
}
