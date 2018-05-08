using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PublicationGui.Models;
using PublicationsCore.facade;
using PublicationsCore.Facade.Dto;

namespace PublicationGui.Controllers
{
    public class BooksController : Controller
    {
        private readonly IPublicationFacade _publicationFacade;

        private readonly IAuthorFacade _authorFacade;

        public BooksController(IPublicationFacade publicationFacade, IAuthorFacade authorFacade)
        {
            _publicationFacade = publicationFacade;
            _authorFacade = authorFacade;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Create()
        {
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData);
            
            return View();
        }

        public IActionResult Edit(BookDto book)
        {
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);
            
            return View("~/Views/Books/Edit.cshtml", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BookDto book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _publicationFacade.EditPublication(book);

                    return RedirectToAction("Index", "Publications");
                }
                catch (ArgumentException e)
                {
                    ViewData["Errors"] = e.Message;
                }
            }
            else
            {
                ViewData["Errors"] = "Formulář není validní";
            }
            
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookDto book)
        {
            // TODO: Remove, only for testing purposes!!
            if (book.AuthorPublicationList == null || book.AuthorPublicationList.Count == 0)
            {
                book.AuthorPublicationList = new List<AuthorPublicationDto>
                {
                    new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = "Douglas",
                            LastName = "Adams"
                        }
                    },
                    new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = "Scott",
                            LastName = "Whoever"
                        }
                    }
                };
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _publicationFacade.AddPublication(book);

                    return RedirectToAction("Index", "Publications");
                }
                catch (ArgumentException e)
                {
                    ViewData["Errors"] = e.Message;
                }
            }
            else
            {
                ViewData["Errors"] = "Formulář není validní";
            }
            
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);

            return View(book);
        }

        public IActionResult Detail(BookDto book)
        {
            return View("~/Views/Books/Detail.cshtml", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAuthorEditView(BookDto book)
        {
            PublicationsController.AddAuthor(book);
            
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);
            
            return View("~/Views/Books/Edit.cshtml", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveAuthorEditView(BookDto book, int index)
        {
            book.AuthorPublicationList.RemoveAt(index);
            
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);
            
            ModelState.Clear();
            
            return View("~/Views/Books/Edit.cshtml", book);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAuthorCreateView(BookDto book)
        {
            PublicationsController.AddAuthor(book);
            
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);
            
            return View("~/Views/Books/Create.cshtml", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveAuthorCreateView(BookDto book, int index)
        {
            book.AuthorPublicationList.RemoveAt(index);
            
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);
            
            ModelState.Clear();

            return View("~/Views/Books/Create.cshtml", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectAuthorEditView(BookDto book, int authorId)
        {
            AuthorDto author = _authorFacade.GetAuthor(authorId);
            PublicationsController.AddAuthor(book, author);
            
            PublicationsController.AddAuthorsToViewData(_authorFacade, ViewData, book);
            
            return View("~/Views/Books/Edit.cshtml", book);
        }
    }
}