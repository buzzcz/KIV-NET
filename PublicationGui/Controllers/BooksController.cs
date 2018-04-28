using Microsoft.AspNetCore.Mvc;
using PublicationsCore.facade;

namespace PublicationGui.Controllers
{
    public class BooksController : Controller
    {
        private readonly IPublicationFacade _publicationFacade;

        public BooksController(IPublicationFacade publicationFacade)
        {
            _publicationFacade = publicationFacade;
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}