using System;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Facade.Enums;

namespace PublicationsCore.Service
{
    public class CitationService : ICitationService
    {
        private static string GetBookCitation(PublicationDto publication)
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
            switch (publication.Type)
            {
                case PublicationType.ConferenceArticle:
                    break;
                case PublicationType.MagazineArticle:
                    break;
                case PublicationType.TechnicalReport:
                    break;
                case PublicationType.QualificationWork:
                    break;
                case PublicationType.Book:
                    return GetBookCitation(publication);
            }

            return "";
        }

        public string GetHtmlDescription(PublicationDto publication)
        {
            throw new NotImplementedException();
        }
    }
}