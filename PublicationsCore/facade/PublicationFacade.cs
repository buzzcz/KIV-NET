using System;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Service;

namespace PublicationsCore.facade
{
    /// <summary>
    /// Implementation of <i>IPublicationFacade</i>.
    /// </summary>
    public class PublicationFacade : IPublicationFacade
    {
        private readonly IPublicationService _publicationService;

        /// <summary>
        /// Constructor for creating <i>PublicationFacade</i>.
        /// </summary>
        /// <param name="publicationService">Service handling database operations with publications.</param>
        public PublicationFacade(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        public PublicationDto AddPublication(PublicationDto publication)
        {
            // TODO: Klaus - Use logger instead. Check the whole project.
            Console.WriteLine($"Adding publication: {publication}.");
            publication = _publicationService.AddPublication(publication);
            Console.WriteLine($"Added publication id: {publication.Id}.");

            return publication;
        }

        public PublicationDto GetPublication(int id)
        {
            Console.WriteLine($"Getting publication id: {id}.");
            PublicationDto publication = _publicationService.GetPublication(id);
            Console.WriteLine($"Got publication: {publication}.");

            return publication;
        }

        public PublicationDto EditPublication(PublicationDto publication)
        {
            Console.WriteLine($"Editing publication: {publication}.");
            publication = _publicationService.EditPublication(publication);
            Console.WriteLine($"Edited publication: {publication}.");

            return publication;
        }

        public PublicationDto DeletePublication(int id)
        {
            Console.WriteLine($"Deleting publication id: {id}.");
            PublicationDto publication = _publicationService.DeletePublication(id);
            Console.WriteLine($"Deleted publication: {publication}.");

            return publication;
        }
    }
}