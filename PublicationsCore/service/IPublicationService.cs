using System.Collections.Generic;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public interface IPublicationService
    {
        /// <summary>
        /// Finds all books.
        /// </summary>
        /// <returns>All books.</returns>
        IList<PublicationDto> GetAllPublications();

        /// <summary>
        /// Adds book to database.
        /// </summary>
        /// <param name="book">New book to be added.</param>
        /// <returns>Added book with filled id.</returns>
        BookDto AddBook(BookDto book);

        /// <summary>
        /// Finds book based on its id.
        /// </summary>
        /// <param name="id">Id of the book to find.</param>
        /// <returns>Found book or <i>null</i> if none is found.</returns>
        BookDto GetBook(int id);

        /// <summary>
        /// Edits specified book.
        /// </summary>
        /// <param name="book">Edited book with original id.</param>
        /// <returns>Edited book.</returns>
        BookDto EditBook(BookDto book);

        /// <summary>
        /// Deletes specified book.
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Deleted book.</returns>
        BookDto DeleteBook(BookDto book);
        
        /// <summary>
        /// Adds article to database.
        /// </summary>
        /// <param name="article">New article to be added.</param>
        /// <returns>Added article with filled id.</returns>
        ArticleDto AddArticle(ArticleDto article);

        /// <summary>
        /// Finds article based on its id.
        /// </summary>
        /// <param name="id">Id of the article to find.</param>
        /// <returns>Found article or <i>null</i> if none is found.</returns>
        ArticleDto GetArticle(int id);
        
        /// <summary>
        /// Edits specified article.
        /// </summary>
        /// <param name="article">Edited article with original id.</param>
        /// <returns>Edited article.</returns>
        ArticleDto EditArticle(ArticleDto article);

        /// <summary>
        /// Deletes specified article.
        /// </summary>
        /// <param name="article"></param>
        /// <returns>Deleted article.</returns>
        ArticleDto DeleteArticle(ArticleDto article);
    }
}