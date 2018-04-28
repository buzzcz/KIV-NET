using AutoMapper;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Utils
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorPublication, AuthorPublicationDto>();
            CreateMap<Book, BookDto>();
            CreateMap<Publication, PublicationDto>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<Article, ArticleDto>();
        }
    }
}