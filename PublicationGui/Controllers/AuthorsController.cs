using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            return View(_authorFacade.GetAllAuthors());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}