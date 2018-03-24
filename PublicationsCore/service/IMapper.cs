using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    /// <summary>
    /// Interface for mapping service between DTOs and Entities and vice-versa.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Maps publication entity to DTO.
        /// </summary>
        /// <param name="publication">Entity to be mapped to DTO.</param>
        /// <returns>DTO of the mapped entity.</returns>
        PublicationDto MapPublication(Publication publication);

        /// <summary>
        /// Maps publication DTO to entity.
        /// </summary>
        /// <param name="publication">DTO to be mapped to entity.</param>
        /// <returns>Entity of the mapped DTO.</returns>
        Publication MapPublication(PublicationDto publication);

        /// <summary>
        /// Maps author entity to DTO.
        /// </summary>
        /// <param name="author">Entity to be mapped to DTO.</param>
        /// <returns>DTO of the mapped entity.</returns>
        AuthorDto MapAuthor(Author author);
        
        /// <summary>
        /// Maps author DTO to entity.
        /// </summary>
        /// <param name="author">DTO to be mapped to entity.</param>
        /// <returns>Entity of the mapped DTO.</returns>
        Author MapAuthor(AuthorDto author);

        /// <summary>
        /// Maps publisher entity to DTO.
        /// </summary>
        /// <param name="publisher">Entity to be mapped to DTO.</param>
        /// <returns>DTO of the mapped entity.</returns>
        PublisherDto MapPublisher(Publisher publisher);
        
        /// <summary>
        /// Maps publisher DTO to entity.
        /// </summary>
        /// <param name="publisher">DTO to be mapped to entity.</param>
        /// <returns>Entity of the mapped DTO.</returns>
        Publisher MapPublisher(PublisherDto publisher);
        
        /// <summary>
        /// Maps author-publication entity to DTO.
        /// </summary>
        /// <param name="authorPublication">Entity to be mapped to DTO.</param>
        /// <returns>DTO of the mapped entity.</returns>
        AuthorPublicationDto MapAuthorPublication(AuthorPublication authorPublication);
        
        /// <summary>
        /// Maps author-publication DTO to entity.
        /// </summary>
        /// <param name="authorPublication">DTO to be mapped to entity.</param>
        /// <returns>Entity of the mapped DTO.</returns>
        AuthorPublication MapAuthorPublication(AuthorPublicationDto authorPublication);
    }
}