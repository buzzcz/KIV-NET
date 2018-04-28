using System.Collections.Generic;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.facade
{
    /// <summary>
    /// Facade for handling authors. 
    /// </summary>
    public interface IAuthorFacade
    {
        /// <summary>
        /// Finds all authors.
        /// </summary>
        /// <returns>All authors.</returns>
        IList<AuthorDto> GetAllAuthors();
    }
}