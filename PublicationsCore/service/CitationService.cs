using System;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public class CitationService : ICitationService
    {
        private static string GetBookCitation(BookDto publication)
        {
            string citation = "";

            if (publication.AuthorPublicationList != null)
            {
                AuthorDto author = publication.AuthorPublicationList[0].Author;
                citation += $"{author.LastName.ToUpper()}, {author.FirstName}";

                int count = publication.AuthorPublicationList.Count;
                for (int i = 1; i < count; i++)
                {
                    citation += i != count - 1 ? ", " : " a ";

                    author = publication.AuthorPublicationList[i].Author;
                    citation += $"{author.FirstName} {author.LastName.ToUpper()}";
                }

                citation += ". ";
            }

            citation +=
                $"{publication.Title}. {publication.Edition}. {publication.Publisher.Address}: {publication.Publisher.Name}, {publication.Date:yyyy}. ISBN {publication.Isbn}.";

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