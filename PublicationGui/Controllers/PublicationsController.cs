using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PublicationGui.Models;
using PublicationsCore.facade;
using PublicationsCore.Facade.Dto;

namespace PublicationGui.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly IPublicationFacade _publicationFacade;

        private readonly BooksController _booksController;

        private readonly ICitationFacade _citationFacade;

        public PublicationsController(IPublicationFacade publicationFacade, BooksController booksController,
            ICitationFacade citationFacade)
        {
            _publicationFacade = publicationFacade;
            _booksController = booksController;
            _citationFacade = citationFacade;
        }

        public static void AddAuthor(BookDto book, AuthorDto author = null)
        {
            if (book.AuthorPublicationList == null)
            {
                book.AuthorPublicationList = new List<AuthorPublicationDto>();
            }

            book.AuthorPublicationList.Add(new AuthorPublicationDto
            {
                Author = author ?? new AuthorDto()
            });
        }

        public static void AddAuthorsToViewData(IAuthorFacade authorFacade, ViewDataDictionary viewData,
            PublicationDto publication = null)
        {
            List<AuthorDto> authors = (List<AuthorDto>) authorFacade.GetAllAuthors();
            if (publication?.AuthorPublicationList != null)
            {
                authors.RemoveAll(dto => publication.AuthorPublicationList.Any(ap =>
                    ap.Author.FirstName == dto.FirstName && ap.Author.LastName == dto.LastName));
            }

            viewData["Authors"] = authors;
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

        public IActionResult Detail(int? id)
        {
            if (id.HasValue)
            {
                PublicationDto publication = _publicationFacade.GetPublication(id.Value);

                if (publication is BookDto book)
                {
                    return _booksController.Detail(book);
                }
            }
            else
            {
                ViewData["Errors"] = "Id musí být vyplněné";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Citation(int? id)
        {
            if (id.HasValue)
            {
                PublicationDto publication = _publicationFacade.GetPublication(id.Value);

                string citation = _citationFacade.GetCitation(publication);
                ViewData["Title"] = "Citace publikace";

                return View("ActionOutput", citation);
            }

            ViewData["Errors"] = "Id musí být vyplněné";

            return RedirectToAction("Index");
        }

        public IActionResult BibTeX(int? id)
        {
            if (id.HasValue)
            {
                PublicationDto publication = _publicationFacade.GetPublication(id.Value);

                string bibTex = _citationFacade.GetBibTex(publication);
                ViewData["Title"] = "BibTeX citace publikace";

                return View("ActionOutput", bibTex);
            }

            ViewData["Errors"] = "Id musí být vyplněné";

            return RedirectToAction("Index");
        }

        public IActionResult HtmlDescription(int? id)
        {
            if (id.HasValue)
            {
                PublicationDto publication = _publicationFacade.GetPublication(id.Value);

                string html = _citationFacade.GetHtmlDescription(publication);
                ViewData["Title"] = "BibTeX citace publikace";

                return View("ActionOutput", html);
            }

            ViewData["Errors"] = "Id musí být vyplněné";

            return RedirectToAction("Index");
        }
    }
}