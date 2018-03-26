using AutoMapper;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence.Model;
using TestProject.Utils;
using Xunit;

namespace TestProject.Service
{
    public class MapperTest
    {
        private readonly IMapper _mapper;

        public MapperTest()
        {
            IMapper mapper = TestUtils.CreateMapper();
            _mapper = mapper;
        }

        [Fact]
        public void TestMapAuthor()
        {
            AuthorDto authorDto = new AuthorDto
            {
                FirstName = "Douglas",
                LastName = "Adams"
            };

            Author author = _mapper.Map<Author>(authorDto);

            Assert.Equal(authorDto.FirstName, author.FirstName);
            Assert.Equal(authorDto.LastName, author.LastName);

            AuthorDto newAuthorDto = _mapper.Map<AuthorDto>(author);

            Assert.Equal(author.FirstName, newAuthorDto.FirstName);
            Assert.Equal(author.LastName, newAuthorDto.LastName);

            Assert.Equal(authorDto, newAuthorDto);
        }

        [Fact]
        public void TestMapBook()
        {
            BookDto bookDto = TestUtils.CreateBook();

            Book book = _mapper.Map<Book>(bookDto);

            Assert.Equal(bookDto.Isbn, book.Isbn);
            Assert.Equal(bookDto.Title, book.Title);
            Assert.Equal(bookDto.Date, book.Date);

            BookDto newBookDto = _mapper.Map<BookDto>(book);

            Assert.Equal(bookDto.Isbn, newBookDto.Isbn);
            Assert.Equal(bookDto.Title, newBookDto.Title);
            Assert.Equal(bookDto.Date, newBookDto.Date);

            Assert.Equal(bookDto, newBookDto);
        }
    }
}