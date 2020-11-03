using LuminiHire.Models;
using LuminiHire.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuminiHire.Pages
{
	public class DashboardModel : PageModel
	{
		[BindProperty]
		public IList<Schools> SchoolsData { get; set; }

		public IDictionary<string, decimal> Dashboards { get; set; }

		private readonly IElasticClient _elasticClient;
		private readonly ISchoolService _schoolService;

		public DashboardModel(IElasticClient elasticClient, ISchoolService schoolService)
		{
			_elasticClient = elasticClient;
			_schoolService = schoolService;
			SchoolsData = new List<Schools>();
			Dashboards = new Dictionary<string, decimal>();
		}

		public async Task<IActionResult> OnGetAsync()
		{
			if (User.Identity.IsAuthenticated)
			{
				var resultado = await _schoolService.GetAll();
				if (resultado.Documents.Count > 0)
				{
					var cidades = resultado.Documents.GroupBy(g => new { g.city });

					foreach (var cidade in cidades.OrderBy(x => x.Key.city).ToList())
					{
						Dashboards.Add(cidade.Key.city, resultado.Documents.Where(x => x.city.Equals(cidade.Key.city)).Count());
					}
				}
			}
			return Page();
		}
	}
}
