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

        private readonly IPublicationService _publicationService;

        private readonly IValidationService _validationService;

        /// <summary>
        /// Constructor for creating <i>PublicationFacade</i>.
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="publicationService">Service handling publications.</param>
        /// <param name="validationService">Validation service used to validate publications.</param>
        public PublicationFacade(ILogger<PublicationFacade> logger, IPublicationService publicationService,
            IValidationService validationService)
        {
            _logger = logger;
            _publicationService = publicationService;
            _validationService = validationService;
        }

        public PublicationDto AddPublication(PublicationDto publication)
        {
            _logger.LogInformation($"Adding publication: {publication}.");
            if (publication is BookDto book)
            {
                _validationService.ValidateBook(book);
                publication = _publicationService.AddBook(book);
            }
            else if (publication is ArticleDto article)
            {
                _validationService.ValidateArticle(article);
                _publicationService.AddArticle(article);
            }

            _logger.LogInformation($"Added publication: {publication}.");

            return publication;
        }

        public PublicationDto GetPublication(int id)
        {
            _logger.LogInformation($"Getting publication id: {id}.");
            PublicationDto publication = _publicationService.GetBook(id);
            if (publication == null)
            {
                publication = _publicationService.GetArticle(id);
            }

            _logger.LogInformation($"Got publication: {publication}.");

            return publication;
        }

        public IList<PublicationDto> GetAllPublications()
        {
            _logger.LogInformation("Getting all publications.");
            IList<PublicationDto> list = new List<PublicationDto>(_publicationService.GetAllPublications());
            _logger.LogInformation($"Got {list.Count} publications.");

            return list;
        }

        public PublicationDto EditPublication(PublicationDto publication)
        {
            _logger.LogInformation($"Editing publication: {publication}.");
            if (publication is BookDto book)
            {
                _validationService.ValidateBook(book);
                publication = _publicationService.EditBook(book);
            }
            else if (publication is ArticleDto article)
            {
                _validationService.ValidateArticle(article);
                _publicationService.EditArticle(article);
            }

            _logger.LogInformation($"Edited publication: {publication}.");

            return publication;
        }

        public PublicationDto DeletePublication(PublicationDto publication)
        {
            _logger.LogInformation($"Deleting publication: {publication}.");
            if (publication is BookDto book)
            {
                _validationService.ValidateBook(book); // TODO: Klaus - Should there be the validation in here?
                publication = _publicationService.DeleteBook(book);
            }

            _logger.LogInformation($"Deleted publication: {publication}.");

            return publication;
        }
    }
}