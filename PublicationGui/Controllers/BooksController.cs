﻿using System;
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

        public BooksController(IPublicationFacade publicationFacade)
        {
            _publicationFacade = publicationFacade;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(BookDto book)
        {
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

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookDto book)
        {
            // TODO: Remove, only for testing purposes!!
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

            return View(book);
        }

        public IActionResult Detail(BookDto book)
        {
            return View("~/Views/Books/Detail.cshtml", book);
        }
    }
}