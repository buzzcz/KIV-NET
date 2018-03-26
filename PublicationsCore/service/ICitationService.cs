using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public interface ICitationService
    {
        /// <summary>
        /// Creates citation of the specified book.
        /// </summary>
        /// <param name="book">Book to cite.</param>
        /// <returns>Citaion of the specified book.</returns>
        string GetBookCitation(BookDto book);

        /// <summary>
        /// Creates HTML snippet describing the specified book.
        /// </summary>
        /// <param name="book">Book to get HTML description for.</param>
        /// <returns>HTML snippet describing the specified book.</returns>
        string GetBookHtmlDescription(BookDto book);
    }
}