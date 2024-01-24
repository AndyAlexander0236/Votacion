using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Votacion.Models;

namespace Votacion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

		public IActionResult Index()
		{
			ClaimsPrincipal claimsUser = HttpContext.User;
			string nombreUsuario = "";
			string fotoPerfil = "";

			if (claimsUser.Identity.IsAuthenticated)
			{
				nombreUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
					.Select(c => c.Value).SingleOrDefault();

				fotoPerfil = claimsUser.Claims.Where(c => c.Type == "fotoPerfil")
					.Select(c => c.Value).SingleOrDefault();
			}

			ViewData["nombreUsuario"] = nombreUsuario;
			ViewData["fotoPerfil"] = fotoPerfil; // Cambié "fotoPerfil" a "FotoPerfil"
			return View();
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}