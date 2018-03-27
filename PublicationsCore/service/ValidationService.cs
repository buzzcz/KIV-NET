using System;
using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public class ValidationService : IValidationService
    {
        public void ValidateBook(BookDto book)
        {
            if (string.IsNullOrEmpty(book.Isbn))
            {
                throw new ArgumentNullException(nameof(book.Isbn), "ISBN musí být vyplněno.");
            }

            if (string.IsNullOrEmpty(book.Title))
            {
                throw new ArgumentNullException(nameof(book.Title), "Název musí být vyplněn.");
            }

            if (string.IsNullOrEmpty(book.Edition))
            {
                throw new ArgumentNullException(nameof(book.Edition), "Vydání musí být vyplněno.");
            }

            if (string.IsNullOrEmpty(book.Publisher?.Name))
            {
                string field;
                if (book.Publisher == null)
                {
                    field = nameof(book.Publisher);
                }
                else
                {
                    field = nameof(book.Publisher.Name);
                }
                throw new ArgumentNullException(field, "Nakladatelství musí být vyplněno.");
            }

            if (string.IsNullOrEmpty(book.Publisher.Address))
            {
                throw new ArgumentNullException(nameof(book.Publisher.Address), "Místo vydání musí být vyplněno.");
            }

            if (book.Date == null)
            {
                throw new ArgumentNullException(nameof(book.Date), "Rok vydání musí být vyplněn.");
            }
        }
    }
}