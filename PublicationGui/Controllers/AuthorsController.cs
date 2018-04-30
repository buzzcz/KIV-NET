using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PublicationGui.Models;
using PublicationsCore.facade;

namespace PublicationGui.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorFacade _authorFacade;

        public AuthorsController(IAuthorFacade authorFacade)
        {
            _authorFacade = authorFacade;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Index()
        {
            return View(_authorFacade.GetAllAuthors());
        }
    }
}