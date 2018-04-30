using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PublicationGui.Models;
using PublicationsCore.facade;
using PublicationsCore.Facade.Dto;

namespace PublicationGui.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly IPublicationFacade _publicationFacade;

        private readonly BooksController _booksController;

        public PublicationsController(IPublicationFacade publicationFacade, BooksController booksController)
        {
            _publicationFacade = publicationFacade;
            _booksController = booksController;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                _publicationFacade.DeletePublication(id.Value);
            }
            else
            {
                ViewData["Errors"] = "Id musí být vyplněné";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                PublicationDto publication = _publicationFacade.GetPublication(id.Value);

                if (publication is BookDto book)
                {
                    return _booksController.Edit(book);
                }
            }
            else
            {
                ViewData["Errors"] = "Id musí být vyplněné";
            }

            return RedirectToAction("Index");
        }
    }
}