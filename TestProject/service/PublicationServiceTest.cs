using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;
using PublicationsCore.Service;
using TestProject.Utils;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Service
{
    [CollectionDefinition("DbTests", DisableParallelization = true)]
    [Collection("DbTests")]
    public class PublicationServiceTest
    {
        private readonly ITestOutputHelper _output;

        private readonly IMapper _mapper;

        private readonly IPublicationService _publicationService;

        public PublicationServiceTest(ITestOutputHelper output)
        {
            _output = output;
            IMapper mapper = TestUtils.CreateMapper();
            _publicationService = new PublicationService(mapper);
            _mapper = mapper;
            Console.SetOut(new ConsoleOutToITestOutputHelper(output));
        }

        [Fact]
        public void Test_AddBook_ValidBook_SameBook()
        {
            BookDto bookDto = TestUtils.CreateBook("ADD");
            _output.WriteLine($"Adding {bookDto} in ADD test.");
            BookDto added = _publicationService.AddBook(bookDto);
            _output.WriteLine($"Added {added} in ADD test.");

            BookDto got;
            using (PublicationsContext db = new PublicationsContext())
            {
                _output.WriteLine($"Getting {added.Id} in ADD test from RAW db ctx.");
                Book entity = db.Books.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author)
                    .Include(p => p.Publisher).First(p => p.Id == added.Id);
                db.SaveChanges();
                _output.WriteLine($"Got entity {entity} in ADD test.");
                got = _mapper.Map<BookDto>(entity);
                _output.WriteLine($"Got {got} in ADD test.");
            }

            try
            {
                Assert.Equal(bookDto, added);
                Assert.Equal(bookDto, got);
                Assert.Equal(added, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void Test_GetBook_ValidBook_SameBook()
        {
            BookDto bookDto = TestUtils.CreateBook("GET");
            _output.WriteLine($"Adding {bookDto} in GET test.");
            bookDto = _publicationService.AddBook(bookDto);
            _output.WriteLine($"Added {bookDto} in GET test.");

            _output.WriteLine($"Getting {bookDto.Id} in GET test.");
            PublicationDto got = _publicationService.GetBook(bookDto.Id);
            _output.WriteLine($"Got {got} in GET test.");

            try
            {
                Assert.Equal(bookDto, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void Test_EditPublicationTitle_ValidBookAndTitle_BookWithNewTitle()
        {
            BookDto bookDto = TestUtils.CreateBook("EDIT TITLE");
            _output.WriteLine($"Adding {bookDto} in EDIT TITLE test.");
            bookDto = _publicationService.AddBook(bookDto);
            _output.WriteLine($"Added {bookDto} in EDIT TITLE test.");

            bookDto.Title = "Doctor Who";
            _output.WriteLine($"Updating {bookDto} in EDIT TITLE test.");
            BookDto edited = _publicationService.EditBook(bookDto);
            _output.WriteLine($"Updated {edited} in EDIT TITLE test.");

            _output.WriteLine($"Getting {bookDto.Id} in EDIT TITLE test.");
            BookDto got = _publicationService.GetBook(bookDto.Id);
            _output.WriteLine($"Got {got} in EDIT TITLE test.");

            try
            {
                Assert.Equal(bookDto, edited);
                Assert.Equal(bookDto, got);
                Assert.Equal(edited, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void Test_EditBookAuthor_ValidBookAndNewAuthor_BookWithNewAuthorAndUnusedAuthorDeleted()
        {
            BookDto bookDto = TestUtils.CreateBook("EDIT AUTHOR");
            bookDto.AuthorPublicationList.Add(new AuthorPublicationDto
            {
                Author = new AuthorDto
                {
                    FirstName = "Unknown",
                    LastName = "Writer"
                }
            });
            _output.WriteLine($"Adding {bookDto} in EDIT AUTHOR test.");
            bookDto = _publicationService.AddBook(bookDto);
            _output.WriteLine($"Added {bookDto} in EDIT AUTHOR test.");

            bookDto.AuthorPublicationList.Clear();
            bookDto.AuthorPublicationList.Add(new AuthorPublicationDto
            {
                Author = new AuthorDto
                {
                    FirstName = "Steven",
                    LastName = "Moffat"
                }
            });
            _output.WriteLine($"Updating {bookDto} in EDIT AUTHOR test.");
            BookDto edited = _publicationService.EditBook(bookDto);
            _output.WriteLine($"Updated {edited} in EDIT AUTHOR test.");

            _output.WriteLine($"Getting {bookDto.Id} in EDIT AUTHOR test.");
            BookDto got = _publicationService.GetBook(bookDto.Id);
            _output.WriteLine($"Got {got} in EDIT AUTHOR test.");

            try
            {
                Assert.Equal(bookDto, edited);
                Assert.Equal(bookDto, got);
                Assert.Equal(edited, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void Test_DeleteBook_ValidBook_DeletedBookAndAuthorAndPublisher()
        {
            BookDto bookDto = TestUtils.CreateBook("DELETE");
            _output.WriteLine($"Adding {bookDto} in DELETE test.");
            bookDto = _publicationService.AddBook(bookDto);
            _output.WriteLine($"Added {bookDto} in DELETE test.");

            _output.WriteLine($"Deleting {bookDto} in DELETE test.");
            BookDto deleted = _publicationService.DeleteBook(bookDto);
            _output.WriteLine($"Deleted {deleted} in DELETE test.");

            _output.WriteLine($"Getting {bookDto.Id} in DELETE test.");
            BookDto got = _publicationService.GetBook(bookDto.Id);
            _output.WriteLine($"Got {got} in DELETE test.");

            try
            {
                Assert.Equal(bookDto.Id, deleted.Id);
                Assert.Equal(bookDto.Isbn, deleted.Isbn);
                Assert.Equal(bookDto.Title, deleted.Title);
                Assert.Equal(bookDto.Date, deleted.Date);
                Assert.Empty(deleted.AuthorPublicationList);
                Assert.Null(deleted.Publisher);
                Assert.Null(got);

                using (PublicationsContext db = new PublicationsContext())
                {
                    Assert.Empty(db.AuthorPublications.AsEnumerable());
                    Assert.Empty(db.Authors.AsEnumerable());
                    Assert.Empty(db.Books.AsEnumerable());
                    Assert.Empty(db.Publishers.AsEnumerable());
                }
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void Test_GetAllPublications_ValidBooks_AllBooks()
        {
            List<PublicationDto> list = new List<PublicationDto>();
            for (int i = 0; i < 10; i++)
            {
                BookDto bookDto = TestUtils.CreateBook("GET ALL" + i);
                bookDto.Title = i.ToString();

                _output.WriteLine($"Adding {bookDto} in GET ALL test.");
                _output.WriteLine($"Added {_publicationService.AddBook(bookDto)} in GET ALL test.");

                list.Add(bookDto);
            }

            _output.WriteLine("Getting all in GET ALL test.");
            IList<PublicationDto> got = _publicationService.GetAllPublications();
            _output.WriteLine($"Got {got.Count} in GET ALL test.");

            try
            {
                Assert.Equal(list.Count, got.Count);
                Assert.Equal(list, got);
                for (int i = 0; i < list.Count; i++)
                {
                    Assert.Equal(list[i], got[i]);
                }
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void Test_AddBook_ValidBooksWithSameAuthor_BooksWithSameAuthor()
        {
            BookDto bookDto1 = TestUtils.CreateBook();
            BookDto bookDto2 = TestUtils.CreateBook("Doctor Who");
            _output.WriteLine($"Adding {bookDto1} in ADD TWO WITH SAME AUTHOR test.");
            bookDto1 = _publicationService.AddBook(bookDto1);
            _output.WriteLine($"Added {bookDto1} in ADD TWO WITH SAME AUTHOR test.");
            _output.WriteLine($"Adding {bookDto2} in ADD TWO WITH SAME AUTHOR test.");
            bookDto2 = _publicationService.AddBook(bookDto2);
            _output.WriteLine($"Added {bookDto2} in ADD TWO WITH SAME AUTHOR test.");

            BookDto got1 = _publicationService.GetBook(bookDto1.Id);
            BookDto got2 = _publicationService.GetBook(bookDto2.Id);

            try
            {
                Assert.Equal(bookDto1.AuthorPublicationList[0].AuthorId, bookDto2.AuthorPublicationList[0].AuthorId);
                Assert.Equal(bookDto1.AuthorPublicationList[0].Author, bookDto2.AuthorPublicationList[0].Author);
                Assert.Equal(bookDto1.AuthorPublicationList[0].AuthorId, got1.AuthorPublicationList[0].AuthorId);
                Assert.Equal(bookDto1.AuthorPublicationList[0].Author, got1.AuthorPublicationList[0].Author);
                Assert.Equal(bookDto2.AuthorPublicationList[0].AuthorId, got2.AuthorPublicationList[0].AuthorId);
                Assert.Equal(bookDto2.AuthorPublicationList[0].Author, got2.AuthorPublicationList[0].Author);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void Test_DeleteBook_ValidBooksWithTheSameAuthor_DeletedAllBooksAndAuthorsAndPublishers()
        {
            BookDto bookDto1 = TestUtils.CreateBook();
            BookDto bookDto2 = TestUtils.CreateBook("Doctor Who");
            _output.WriteLine($"Adding {bookDto1} in DELETE TWO WITH SAME AUTHOR test.");
            bookDto1 = _publicationService.AddBook(bookDto1);
            _output.WriteLine($"Added {bookDto1} in DELETE TWO WITH SAME AUTHOR test.");
            _output.WriteLine($"Adding {bookDto2} in DELETE TWO WITH SAME AUTHOR test.");
            bookDto2 = _publicationService.AddBook(bookDto2);
            _output.WriteLine($"Added {bookDto2} in DELETE TWO WITH SAME AUTHOR test.");

            _output.WriteLine($"Deleting {bookDto1} in DELETE TWO WITH SAME AUTHOR test.");
            BookDto deleted1 = _publicationService.DeleteBook(bookDto1);
            _output.WriteLine($"Deleted {deleted1} in DELETE TWO WITH SAME AUTHOR test.");

            _output.WriteLine($"Getting {bookDto1.Id} in DELETE TWO WITH SAME AUTHOR test.");
            BookDto got1 = _publicationService.GetBook(bookDto1.Id);
            _output.WriteLine($"Got {got1} in DELETE test.");

            _output.WriteLine($"Getting {bookDto2.Id} in DELETE TWO WITH SAME AUTHOR test.");
            BookDto got2 = _publicationService.GetBook(bookDto2.Id);
            _output.WriteLine($"Got {got2} in DELETE test.");

            try
            {
                Assert.Null(got1);
                Assert.Equal(bookDto2, got2);

                using (PublicationsContext db = new PublicationsContext())
                {
                    IList<AuthorPublication> authorPublications = db.AuthorPublications.ToList();
                    IList<Author> authors = db.Authors.ToList();
                    IList<Publisher> publishers = db.Publishers.ToList();
                    IList<Book> publications = db.Books.ToList();
                    Assert.Equal(1, authorPublications.Count);
                    Assert.Equal(1, authors.Count);
                    Assert.Equal(1, publishers.Count);
                    Assert.Equal(1, publications.Count);
                    foreach (AuthorPublicationDto authorPublicationDto in bookDto2.AuthorPublicationList)
                    {
                        Assert.Contains(_mapper.Map<AuthorPublication>(authorPublicationDto), authorPublications);
                        Assert.Contains(_mapper.Map<Author>(authorPublicationDto.Author), authors);
                    }

                    Assert.Contains(_mapper.Map<Publisher>(bookDto2.Publisher), publishers);
                    Assert.Contains(_mapper.Map<Book>(bookDto2), publications);
                }
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }
        
        [Fact]
        public void Test_AddArticle_ValidArticle_SameArticle()
        {
            ArticleDto articleDto = TestUtils.CreateArticle("ADD");
            _output.WriteLine($"Adding {articleDto} in ADD test.");
            ArticleDto added = _publicationService.AddArticle(articleDto);
            _output.WriteLine($"Added {added} in ADD test.");

            ArticleDto got;
            using (PublicationsContext db = new PublicationsContext())
            {
                _output.WriteLine($"Getting {added.Id} in ADD test from RAW db ctx.");
                Article entity = db.Articles.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author)
                    .Include(p => p.Publisher).First(p => p.Id == added.Id);
                db.SaveChanges();
                _output.WriteLine($"Got entity {entity} in ADD test.");
                got = _mapper.Map<ArticleDto>(entity);
                _output.WriteLine($"Got {got} in ADD test.");
            }

            try
            {
                Assert.Equal(articleDto, added);
                Assert.Equal(articleDto, got);
                Assert.Equal(added, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }
        
        [Fact]
        public void Test_GetAllPublications_ValidBooksAndArticles_AllBooksAndArticles()
        {
            List<PublicationDto> list = new List<PublicationDto>();
            for (int i = 0; i < 10; i++)
            {
                BookDto bookDto = TestUtils.CreateBook("GET ALL" + i);
                bookDto.Title = i.ToString();

                _output.WriteLine($"Adding {bookDto} in GET ALL test.");
                _output.WriteLine($"Added {_publicationService.AddBook(bookDto)} in GET ALL test.");

                list.Add(bookDto);
            }
            for (int i = 0; i < 10; i++)
            {
                ArticleDto articleDto = TestUtils.CreateArticle("GET ALL" + i);
                articleDto.Title = i.ToString();

                _output.WriteLine($"Adding {articleDto} in GET ALL test.");
                _output.WriteLine($"Added {_publicationService.AddArticle(articleDto)} in GET ALL test.");

                list.Add(articleDto);
            }

            _output.WriteLine("Getting all in GET ALL test.");
            IList<PublicationDto> got = _publicationService.GetAllPublications();
            _output.WriteLine($"Got {got.Count} in GET ALL test.");

            try
            {
                Assert.Equal(list.Count, got.Count);
                Assert.Equal(list, got);
                for (int i = 0; i < list.Count; i++)
                {
                    Assert.Equal(list[i], got[i]);
                }
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }
    }
}