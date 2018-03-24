using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public interface ICitationService
    {
        /// <summary>
        /// Creates citation of the specified publication.
        /// </summary>
        /// <param name="publication">Publication to cite.</param>
        /// <returns>Citaion of the specified publication.</returns>
        string getCitation(PublicationDto publication);

        /// <summary>
        /// Creates HTML snippet describing the specified publication.
        /// </summary>
        /// <param name="publication">Publication that should be described by the snippet.</param>
        /// <returns>HTML snippet describing the specified publication.</returns>
        string getHtmlDescription(PublicationDto publication);
    }
}