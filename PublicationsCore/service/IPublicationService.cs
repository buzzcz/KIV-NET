using System.Collections.Generic;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    /// <summary>
    /// Interface for handling publications.
    /// </summary>
    public interface IPublicationService
    {
        /// <summary>
        /// Adds publication to database.
        /// </summary>
        /// <param name="publication">New publication to be added.</param>
        /// <returns>Added publication with filled id.</returns>
        PublicationDto AddPublication(PublicationDto publication);

        /// <summary>
        /// Finds publication based on its id.
        /// </summary>
        /// <param name="id">Id of the publication to find.</param>
        /// <returns>Found publication or <i>null</i> if none is found.</returns>
        PublicationDto GetPublication(int id);

        /// <summary>
        /// Finds all publications.
        /// </summary>
        /// <returns>All publications.</returns>
        IList<PublicationDto> GetAllPublications();
        
        /// <summary>
        /// Edits specified publication.
        /// </summary>
        /// <param name="publication">Edited publication with original id.</param>
        /// <returns>Edited publication.</returns>
        PublicationDto EditPublication(PublicationDto publication);

        /// <summary>
        /// Deletes specified publication.
        /// </summary>
        /// <param name="publication">Publication to be deleted.</param>
        /// <returns>Deleted publication.</returns>
        PublicationDto DeletePublication(PublicationDto publication);
    }
}