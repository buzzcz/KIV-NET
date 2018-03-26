using System;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public class CitationService : ICitationService
    {
        private static string GetBookCitation(BookDto book)
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

        public string GetCitation(PublicationDto publication)
        {
            if (publication is BookDto book)
            {
                return GetBookCitation(book);
            }

            throw new NotImplementedException();
        }

        public string GetHtmlDescription(PublicationDto publication)
        {
            throw new NotImplementedException();
        }
    }
}