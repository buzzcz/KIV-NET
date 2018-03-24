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

        /// <summary>
        /// Constructor for creating <i>PublicationFacade</i>.
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="publicationService">Service handling database operations with publications.</param>
        public PublicationFacade(ILogger<PublicationFacade> logger, IPublicationService publicationService)
        {
            _logger = logger;
            _publicationService = publicationService;
        }

        public PublicationDto AddPublication(PublicationDto publication)
        {
            _logger.LogInformation($"Adding publication: {publication}.");
            publication = _publicationService.AddPublication(publication);
            _logger.LogInformation($"Added publication id: {publication.Id}.");

            return publication;
        }

        public PublicationDto GetPublication(int id)
        {
            _logger.LogInformation($"Getting publication id: {id}.");
            PublicationDto publication = _publicationService.GetPublication(id);
            _logger.LogInformation($"Got publication: {publication}.");

            return publication;
        }

        public IList<PublicationDto> GetAllPublications()
        {
            _logger.LogInformation("Getting all publications.");
            IList<PublicationDto> list = _publicationService.GetAllPublications();
            _logger.LogInformation($"Got {list.Count} publications.");

            return list;
        }

        public PublicationDto EditPublication(PublicationDto publication)
        {
            _logger.LogInformation($"Editing publication: {publication}.");
            publication = _publicationService.EditPublication(publication);
            _logger.LogInformation($"Edited publication: {publication}.");

            return publication;
        }

        public PublicationDto DeletePublication(PublicationDto publication)
        {
            _logger.LogInformation($"Deleting publication: {publication}.");
            publication = _publicationService.DeletePublication(publication);
            _logger.LogInformation($"Deleted publication: {publication}.");

            return publication;
        }
    }
}