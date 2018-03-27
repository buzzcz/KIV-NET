using PublicationsCore.Facade.Dto;

namespace PublicationsCore.Service
{
    public interface IValidationService
    {
        void ValidateBook(BookDto book);

        void ValidateArticle(ArticleDto article);
    }
}