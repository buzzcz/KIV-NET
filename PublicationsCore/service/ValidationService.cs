using System;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public class ValidationService : IValidationService
    {
        private static void ValidatePublisher(PublicationDto publication)
        {
            if (string.IsNullOrEmpty(publication.Publisher?.Name))
            {
                throw new ArgumentException("Nakladatelství musí být vyplněno.");
            }
        }

        private static void ValidateEdition(PublicationDto publication, string text = "Vydání")
        {
            if (string.IsNullOrEmpty(publication.Edition))
            {
                throw new ArgumentException($"{text} musí být vyplněno.");
            }
        }

        private static void ValidateTitle(PublicationDto publication)
        {
            if (string.IsNullOrEmpty(publication.Title))
            {
                throw new ArgumentException("Název musí být vyplněn.");
            }
        }

        private static void ValidateAddress(PublicationDto publication)
        {
            if (string.IsNullOrEmpty(publication.Publisher.Address))
            {
                throw new ArgumentException("Místo vydání musí být vyplněno.");
            }
        }

        private static void ValidateDate(PublicationDto publication)
        {
            if (publication.Date == null)
            {
                throw new ArgumentException("Rok vydání musí být vyplněn.");
            }
        }

        public void ValidateBook(BookDto book)
        {
            ValidateTitle(book);
            ValidateEdition(book);
            ValidatePublisher(book);
            ValidateAddress(book);
            ValidateDate(book);

            if (string.IsNullOrEmpty(book.Isbn))
            {
                throw new ArgumentException("ISBN musí být vyplněno.");
            }
        }

        public void ValidateArticle(ArticleDto article)
        {
            ValidateTitle(article);
            ValidateEdition(article, "Ročník");
            ValidateDate(article);

            if (string.IsNullOrEmpty(article.Pages))
            {
                throw new ArgumentException("Rozsah stran musí být vyplněn.");
            }

            if (string.IsNullOrEmpty(article.MagazineTitle))
            {
                throw new ArgumentException("Název časopisu musí být vyplněn.");
            }

            if (article.Volume < 0)
            {
                throw new ArgumentException("Číslo musí být vyplněno.");
            }
        }
    }
}