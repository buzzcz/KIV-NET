using PublicationsCore.Facade.Dto;

namespace PublicationsCore.facade
{
    public interface ICitationFacade
    {
        /// <summary>
        /// Creates citation of the specified publication.
        /// </summary>
        /// <param name="publication">Publication to cite.</param>
        /// <returns>Citaion of the specified publication.</returns>
        string GetCitation(PublicationDto publication);

        /// <summary>
        /// Creates HTML snippet describing the specified publication.
        /// </summary>
        /// <param name="publication">Publication that should be described by the snippet.</param>
        /// <returns>HTML snippet describing the specified publication.</returns>
        string GetHtmlDescription(PublicationDto publication);

        /// <summary>
        /// Creates BibTex entry for specified publication.
        /// </summary>
        /// <param name="publication">Publication for which the BibTex entry should be created.</param>
        /// <returns>BibTex entry for specified publication.</returns>
        string GetBibTex(PublicationDto publication);
    }
}