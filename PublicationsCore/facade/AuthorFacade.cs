using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Service;

namespace PublicationsCore.facade
{
    public class AuthorFacade : IAuthorFacade
    {
        private readonly ILogger _logger;

        private readonly IAuthorService _authorService;

        public AuthorFacade(ILogger<AuthorFacade> logger, IAuthorService authorService)
        {
            _logger = logger;
            _authorService = authorService;
        }

        public IList<AuthorDto> GetAllAuthors()
        {
            _logger.LogInformation("Getting all authors.");
            IList<AuthorDto> authors = _authorService.GetAllAuthors();
            _logger.LogInformation($"Got {authors.Count} authors.");

            return authors;
        }
    }
}