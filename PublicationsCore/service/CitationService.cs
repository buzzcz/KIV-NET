using System;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Facade.Enums;

namespace PublicationsCore.Service
{
    public class CitationService : ICitationService
    {
        private string getBookCitation(PublicationDto publication)
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
        
        public string getCitation(PublicationDto publication)
        {
            switch (publication.Type)
            {
                case PublicationType.CONFERENCE_ARTICLE:
                    break;
                case PublicationType.MAGAZINE_ARTICLE:
                    break;
                case PublicationType.TECHNICAL_REPORT:
                    break;
                case PublicationType.QUALIFICATION_WORK:
                    break;
                case PublicationType.BOOK:
                    return getBookCitation(publication);
            }

            return "";
        }

        public string getHtmlDescription(PublicationDto publication)
        {
            throw new NotImplementedException();
        }
    }
}