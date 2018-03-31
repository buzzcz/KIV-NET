using System;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public class CitationService : ICitationService
    {
        /// <summary>
        /// Adds next author (not the first one) to the citation.
        /// </summary>
        /// <param name="author">Author to add.</param>
        /// <param name="citation">Citation to which author should be added.</param>
        /// <param name="comma">Indicates whether author should be separated by comma or 'and'.</param>
        /// <returns>Citation with added author.</returns>
        private static string AddNextAuthor(AuthorDto author, string citation, bool comma)
        {
            citation += comma ? "," : " a";

            if (author.FirstName != null)
            {
                citation += $" {author.FirstName}";
            }

            if (author.LastName != null)
            {
                citation += $" {author.LastName.ToUpper()}";
            }

            return citation;
        }

        /// <summary>
        /// Adds first author to the citation.
        /// </summary>
        /// <param name="author">Author to add.</param>
        /// <param name="citation">Citation to which author should be added.</param>
        /// <returns>Citation with added author.</returns>
        private static string AddFirstAuthor(AuthorDto author, string citation)
        {
            if (author.LastName != null)
            {
                citation += $"{author.LastName.ToUpper()}";
            }

            if (author.FirstName != null)
            {
                if (author.LastName != null)
                {
                    citation += ", ";
                }

                citation += $"{author.FirstName}";
            }

            return citation;
        }

        public string GetBookCitation(BookDto book)
        {
            string citation = "";

            if (book.AuthorPublicationList != null)
            {
                AuthorDto author = book.AuthorPublicationList[0].Author;
                citation = AddFirstAuthor(author, citation);

                int count = book.AuthorPublicationList.Count;
                for (int i = 1; i < count; i++)
                {
                    citation = AddNextAuthor(book.AuthorPublicationList[i].Author, citation, i != count - 1);
                }

                citation += ". ";
            }

            citation +=
                $"{book.Title}. {book.Edition}. {book.Publisher.Address}: {book.Publisher.Name}, {book.Date:yyyy}. ISBN {book.Isbn}.";

            return citation;
        }

        public string GetBookHtmlDescription(BookDto book)
        {
            throw new NotImplementedException();
        }

        public string GetArticleCitation(ArticleDto article)
        {
            string citation = "";

            if (article.AuthorPublicationList != null)
            {
                AuthorDto author = article.AuthorPublicationList[0].Author;
                citation = AddFirstAuthor(author, citation);

                int count = article.AuthorPublicationList.Count;
                for (int i = 1; i < count; i++)
                {
                    citation = AddNextAuthor(article.AuthorPublicationList[i].Author, citation, i != count - 1);
                }

                citation += ". ";
            }

            citation += $"{article.Title}. {article.MagazineTitle}.";
            
            if (article.Publisher != null)
            {
                PublisherDto publisher = article.Publisher;
                if (publisher.Address != null && publisher.Name != null)
                {
                    citation += $" {publisher.Address}: {publisher.Name},";
                } else if (publisher.Address != null)
                {
                    citation += $"{publisher.Address},";
                } else if (publisher.Name != null)
                {
                    citation += $"{publisher.Name},";
                }
            }

            citation += $" {article.Date:yyyy}, {article.Edition}({article.Volume}), {article.Pages}.";

            if (article.Doi != null)
            {
                citation +=  $" DOI {article.Doi}.";
            }

            if (article.Issn != null)
            {
                citation += $" ISSN {article.Issn}.";
            }

            return citation;
        }

        public string GetArticleHtmlDescription(ArticleDto article)
        {
            throw new NotImplementedException();
        }
    }
}