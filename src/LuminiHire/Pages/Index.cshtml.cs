using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LuminiHire.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public IndexModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<IndexModel> logger)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;

			this._logger = logger;
		}
		
		public void OnGet()
		{
			if (ModelState.IsValid)
			{
				
			}
		}
	}
}
