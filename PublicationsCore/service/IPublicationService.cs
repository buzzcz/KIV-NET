using System.Collections.Generic;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public interface IPublicationService
    {
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
        /// Finds all books.
        /// </summary>
        /// <returns>All books.</returns>
        IList<BookDto> GetAllBooks();

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
    }
}