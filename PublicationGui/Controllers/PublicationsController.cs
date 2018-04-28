using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PublicationGui.Models;
using PublicationsCore.facade;

namespace PublicationGui.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly IPublicationFacade _publicationFacade;

        public PublicationsController(IPublicationFacade publicationFacade)
        {
            _publicationFacade = publicationFacade;
        }
        
        public IActionResult Index()
        {
            return View(_publicationFacade.GetAllPublications());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}