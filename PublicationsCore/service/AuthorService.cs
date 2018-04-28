using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;

        public AuthorService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IList<AuthorDto> GetAllAuthors()
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                IList<Author> entities = db.Authors.ToList();
                
                return entities.Select(a => _mapper.Map<AuthorDto>(a)).ToList();
            }
        }
    }
}