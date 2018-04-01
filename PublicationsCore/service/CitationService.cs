﻿using System;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public class CitationService : ICitationService
    {
        private const string SpanBook = "<span class='book'>";
        private const string SpanArticle = "<span class='article'>";
        private const string SpanEnd = "</span>";
        private const string SpanTitle = "<span class='title'>";
        private const string SpanPublisher = "<span class='publisher'>";
        private const string SpanPublicationDate = "<span class='publication-date'>";
        private const string SpanAddress = "<span class='address'>";
        private const string SpanEdition = "<span class='edition'>";
        private const string SpanAuthors = "<span class='authors'>";
        private const string SpanIsbn = "<span class='isbn'>";
        private const string SpanMagazineTile = "<span class='magazine-title'>";
        private const string SpanVolume = "<span class='volume'>";
        private const string SpanPages = "<span class='pages'>";
        private const string SpanDoi = "<span class='doi'>";
        private const string SpanIssn = "<span class='issn'>";

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

        /// <summary>
        /// Adds first author to the HTML description with span tag and class author.
        /// </summary>
        /// <param name="author">Author to add</param>
        /// <param name="html">HTML description to which author should be added.</param>
        /// <returns>HTML description with added author.</returns>
        private static string AddFirstAuthorHtml(AuthorDto author, string html)
        {
            html += SpanAuthors;
            html = AddFirstAuthor(author, html);

            return html;
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
            string html = $"{SpanBook}";

            if (book.AuthorPublicationList != null)
            {
                AuthorDto author = book.AuthorPublicationList[0].Author;
                html = AddFirstAuthorHtml(author, html);

                int count = book.AuthorPublicationList.Count;
                for (int i = 1; i < count; i++)
                {
                    html = AddNextAuthor(book.AuthorPublicationList[i].Author, html, i != count - 1);
                }

                html += $"{SpanEnd}. ";
            }

            html +=
                $"{SpanTitle}{book.Title}{SpanEnd}. {SpanEdition}{book.Edition}{SpanEnd}. {SpanAddress}" +
                $"{book.Publisher.Address}{SpanEnd}: {SpanPublisher}{book.Publisher.Name}{SpanEnd}, " +
                $"{SpanPublicationDate}{book.Date:yyyy}{SpanEnd}. ISBN {SpanIsbn}{book.Isbn}{SpanEnd}.{SpanEnd}";

            return html;
        }

        public string GetBookBibTex(BookDto article)
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
            string html = $"{SpanArticle}";

            if (article.AuthorPublicationList != null)
            {
                AuthorDto author = article.AuthorPublicationList[0].Author;
                html = AddFirstAuthorHtml(author, html);

                int count = article.AuthorPublicationList.Count;
                for (int i = 1; i < count; i++)
                {
                    html = AddNextAuthor(article.AuthorPublicationList[i].Author, html, i != count - 1);
                }

                html += $"{SpanEnd}. ";
            }

            html += $"{SpanTitle}{article.Title}{SpanEnd}. {SpanMagazineTile}{article.MagazineTitle}{SpanEnd}.";
            
            if (article.Publisher != null)
            {
                PublisherDto publisher = article.Publisher;
                if (publisher.Address != null && publisher.Name != null)
                {
                    html += $" {SpanAddress}{publisher.Address}{SpanEnd}: {SpanPublisher}{publisher.Name}{SpanEnd},";
                } else if (publisher.Address != null)
                {
                    html += $" {SpanAddress}{publisher.Address}{SpanEnd},";
                } else if (publisher.Name != null)
                {
                    html += $" {SpanPublisher}{publisher.Name}{SpanEnd},";
                }
            }

            html += $" {SpanPublicationDate}{article.Date:yyyy}{SpanEnd}, {SpanEdition}{article.Edition}{SpanEnd}(" +
            $"{SpanVolume}{article.Volume}{SpanEnd}), {SpanPages}{article.Pages}{SpanEnd}.";

            if (article.Doi != null)
            {
                html +=  $" DOI {SpanDoi}{article.Doi}{SpanEnd}.";
            }

            if (article.Issn != null)
            {
                html += $" ISSN {SpanIssn}{article.Issn}{SpanEnd}.";
            }

            html += $"{SpanEnd}";

            return html;
        }

        public string GetArticleBibTex(ArticleDto article)
        {
            throw new NotImplementedException();
        }
    }
}