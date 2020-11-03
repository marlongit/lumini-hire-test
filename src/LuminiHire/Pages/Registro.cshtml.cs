using LuminiHire.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LuminiHire.Pages
{
	public class RegistroModel : PageModel
	{
		private readonly ILogger<RegistroModel> _logger;

		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public RegistroModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<RegistroModel> logger)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			_logger = logger;
		}

		[BindProperty]
		public UsuarioRegistro UsuarioRegistro { get; set; }

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{

			if (!ModelState.IsValid) return RedirectToPage("./Error");

			var user = new IdentityUser
			{
				UserName = UsuarioRegistro.Email,
				Email = UsuarioRegistro.Email,
				EmailConfirmed = true
			};

			var result = await userManager.CreateAsync(user, UsuarioRegistro.Senha);

			if (result.Succeeded)
			{
				await signInManager.SignInAsync(user, isPersistent: false);

				_logger.LogInformation(3, "Usuário criado com novo login e senha");
				return RedirectToPage("./Index");
			}

			foreach (var error in result.Errors)
			{
				_logger.LogError(error.Code, error.Description);
				ModelState.AddModelError(error.Code, error.Description);
			}

			return RedirectToPage("./Error");
		}
	}
}
