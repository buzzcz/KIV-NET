using PublicationsCore.Facade.Dto;

namespace PublicationsCore.facade
{
    /// <summary>
    /// Facade for handling of publications. 
    /// </summary>
    public interface IPublicationFacade
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
        /// Edits specified publication.
        /// </summary>
        /// <param name="publication">Edited publication with original id.</param>
        /// <returns>Edited publication.</returns>
        PublicationDto EditPublication(PublicationDto publication);

        /// <summary>
        /// Deletes publication based on its id.
        /// </summary>
        /// <param name="id">Id of publication to be deleted.</param>
        /// <returns>Deleted publication.</returns>
        PublicationDto DeletePublication(int id);
    }
}