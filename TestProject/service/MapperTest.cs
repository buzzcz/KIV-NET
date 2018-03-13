using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence.Model;
using PublicationsCore.Service;
using Xunit;

namespace TestProject.Service
{
    public class MapperTest
    {
        private readonly IMapper _mapper = new Mapper();

        [Fact]
        public void TestMapAuthor()
        {
            AuthorDto authorDto = new AuthorDto
            {
                FirstName = "Douglas",
                LastName = "Adams"
            };

            Author author = _mapper.MapAuthor(authorDto);

            Assert.Equal(authorDto.FirstName, author.FirstName);
            Assert.Equal(authorDto.LastName, author.LastName);

            AuthorDto newAuthorDto = _mapper.MapAuthor(author);

            Assert.Equal(author.FirstName, newAuthorDto.FirstName);
            Assert.Equal(author.LastName, newAuthorDto.LastName);

            Assert.Equal(authorDto, newAuthorDto);
        }
    }
}