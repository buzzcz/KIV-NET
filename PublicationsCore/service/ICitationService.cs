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

        /// <summary>
        /// Creates BibTex entry for specified book.
        /// </summary>
        /// <param name="article">Book for which the BibTex entry should be created.</param>
        /// <returns>BibTex entry for specified book.</returns>
        string GetBookBibTex(BookDto article);
        
        /// <summary>
        /// Creates citation of the specified article.
        /// </summary>
        /// <param name="article">Article to cite.</param>
        /// <returns>Citaion of the specified article.</returns>
        string GetArticleCitation(ArticleDto article);

        /// <summary>
        /// Creates HTML snippet describing the specified article.
        /// </summary>
        /// <param name="article">Article to get HTML description for.</param>
        /// <returns>HTML snippet describing the specified article.</returns>
        string GetArticleHtmlDescription(ArticleDto article);
        
        /// <summary>
        /// Creates BibTex entry for specified article.
        /// </summary>
        /// <param name="article">Article for which the BibTex entry should be created.</param>
        /// <returns>BibTex entry for specified article.</returns>
        string GetArticleBibTex(ArticleDto article);
    }
}