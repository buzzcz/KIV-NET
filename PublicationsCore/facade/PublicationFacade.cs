using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Service;

namespace PublicationsCore.facade
{
    /// <summary>
    /// Implementation of <i>IPublicationFacade</i>.
    /// </summary>
    public class PublicationFacade : IPublicationFacade
    {
        private readonly ILogger _logger;
        
        private readonly IBookService _bookService;

        private readonly IValidationService _validationService;

        /// <summary>
        /// Constructor for creating <i>PublicationFacade</i>.
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="bookService">Service handling books.</param>
        /// <param name="validationService">Validation service used to validate publications.</param>
        public PublicationFacade(ILogger logger, IBookService bookService, IValidationService validationService)
        {
            _logger = logger;
            _bookService = bookService;
            _validationService = validationService;
        }

        public PublicationDto AddPublication(PublicationDto publication)
        {
            _logger.LogInformation($"Adding publication: {publication}.");
            if (publication is BookDto book)
            {
                _validationService.validateBook(book);
                publication = _bookService.AddBook(book);
            }

            _logger.LogInformation($"Added publication: {publication}.");

            return publication;
        }

        public PublicationDto GetPublication(int id)
        {
            _logger.LogInformation($"Getting publication id: {id}.");
            PublicationDto publication = _bookService.GetBook(id);
            _logger.LogInformation($"Got publication: {publication}.");

            return publication;
        }

        public IList<PublicationDto> GetAllPublications()
        {
            _logger.LogInformation("Getting all publications.");
            IList<PublicationDto> list = new List<PublicationDto>(_bookService.GetAllBooks());
            _logger.LogInformation($"Got {list.Count} publications.");

            return list;
        }

        public PublicationDto EditPublication(PublicationDto publication)
        {
            _logger.LogInformation($"Editing publication: {publication}.");
            if (publication is BookDto book)
            {
                _validationService.validateBook(book);
                publication = _bookService.EditBook(book);
            }

            _logger.LogInformation($"Edited publication: {publication}.");

            return publication;
        }

        public PublicationDto DeletePublication(PublicationDto publication)
        {
            _logger.LogInformation($"Deleting publication: {publication}.");
            if (publication is BookDto book)
            {
                _validationService.validateBook(book); // TODO: Klaus - Should there be the validation in here?
                publication = _bookService.DeleteBook(book);
            }

            _logger.LogInformation($"Deleted publication: {publication}.");

            return publication;
        }
    }
}