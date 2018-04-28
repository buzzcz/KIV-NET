using System.Collections.Generic;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public interface IAuthorService
    {
        /// <summary>
        /// Finds all authors.
        /// </summary>
        /// <returns>All authors.</returns>
        IList<AuthorDto> GetAllAuthors();
    }
}