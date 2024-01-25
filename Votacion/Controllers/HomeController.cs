using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
                    .Select(c => c.Value).FirstOrDefault();

                fotoPerfil = claimsUser.Claims.Where(c => c.Type == "FotoPerfil")
                    .Select(c => c.Value).FirstOrDefault();
            }

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["fotoPerfil"] = fotoPerfil;
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
