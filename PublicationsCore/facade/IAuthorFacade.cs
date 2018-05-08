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

        /// <summary>
        /// Finds author by id.
        /// </summary>
        /// <param name="id">Id of the author to find.</param>
        /// <returns>Author with specified id or null in none was found.</returns>
        AuthorDto GetAuthor(int id);
    }
}