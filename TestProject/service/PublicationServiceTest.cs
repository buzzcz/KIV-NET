using System;
using System.Collections.Generic;
using System.Linq;
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
            IMapper mapper = new Mapper();
            _publicationService = new PublicationService(mapper);
            _mapper = mapper;
            Console.SetOut(new ConsoleOutToITestOutputHelper(output));
        }

        [Fact]
        public void TestAddPublication()
        {
            PublicationDto publicationDto = TestUtils.CreatePublication("ADD");
            _output.WriteLine($"Adding {publicationDto} in ADD test.");
            PublicationDto added = _publicationService.AddPublication(publicationDto);
            _output.WriteLine($"Added {added} in ADD test.");

            PublicationDto got;
            using (PublicationsContext db = new PublicationsContext())
            {
                _output.WriteLine($"Getting {added.Id} in ADD test from RAW db ctx.");
                Publication entity = db.Publications.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author)
                    .Include(p => p.Publisher).First(p => p.Id == added.Id);
                db.SaveChanges();
                _output.WriteLine($"Got entity {entity} in ADD test.");
                got = _mapper.MapPublication(entity);
                _output.WriteLine($"Got {got} in ADD test.");
            }

            try
            {
                Assert.Equal(publicationDto, added);
                Assert.Equal(publicationDto, got);
                Assert.Equal(added, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void TestGetPublication()
        {
            PublicationDto publicationDto = TestUtils.CreatePublication("GET");
            _output.WriteLine($"Adding {publicationDto} in GET test.");
            publicationDto = _publicationService.AddPublication(publicationDto);
            _output.WriteLine($"Added {publicationDto} in GET test.");

            _output.WriteLine($"Getting {publicationDto.Id} in GET test.");
            PublicationDto got = _publicationService.GetPublication(publicationDto.Id);
            _output.WriteLine($"Got {got} in GET test.");

            try
            {
                Assert.Equal(publicationDto, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void TestEditPublicationTitle()
        {
            PublicationDto publicationDto = TestUtils.CreatePublication("EDIT TITLE");
            _output.WriteLine($"Adding {publicationDto} in EDIT TITLE test.");
            publicationDto = _publicationService.AddPublication(publicationDto);
            _output.WriteLine($"Added {publicationDto} in EDIT TITLE test.");

            publicationDto.Title = "Doctor Who";
            _output.WriteLine($"Updating {publicationDto} in EDIT TITLE test.");
            PublicationDto edited = _publicationService.EditPublication(publicationDto);
            _output.WriteLine($"Updated {edited} in EDIT TITLE test.");

            _output.WriteLine($"Getting {publicationDto.Id} in EDIT TITLE test.");
            PublicationDto got = _publicationService.GetPublication(publicationDto.Id);
            _output.WriteLine($"Got {got} in EDIT TITLE test.");

            try
            {
                Assert.Equal(publicationDto, edited);
                Assert.Equal(publicationDto, got);
                Assert.Equal(edited, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void TestEditPublicationAuthor()
        {
            PublicationDto publicationDto = TestUtils.CreatePublication("EDIT AUTHOR");
            publicationDto.AuthorPublicationList.Add(new AuthorPublicationDto
            {
                Author = new AuthorDto
                {
                    FirstName = "Unknown",
                    LastName = "Writer"
                }
            });
            _output.WriteLine($"Adding {publicationDto} in EDIT AUTHOR test.");
            publicationDto = _publicationService.AddPublication(publicationDto);
            _output.WriteLine($"Added {publicationDto} in EDIT AUTHOR test.");

            publicationDto.AuthorPublicationList.Clear();
            publicationDto.AuthorPublicationList.Add(new AuthorPublicationDto
            {
                Author = new AuthorDto
                {
                    FirstName = "Steven",
                    LastName = "Moffat"
                }
            });
            _output.WriteLine($"Updating {publicationDto} in EDIT AUTHOR test.");
            PublicationDto edited = _publicationService.EditPublication(publicationDto);
            _output.WriteLine($"Updated {edited} in EDIT AUTHOR test.");

            _output.WriteLine($"Getting {publicationDto.Id} in EDIT AUTHOR test.");
            PublicationDto got = _publicationService.GetPublication(publicationDto.Id);
            _output.WriteLine($"Got {got} in EDIT AUTHOR test.");

            try
            {
                Assert.Equal(publicationDto, edited);
                Assert.Equal(publicationDto, got);
                Assert.Equal(edited, got);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }

        [Fact]
        public void TestDeletePublication()
        {
            PublicationDto publicationDto = TestUtils.CreatePublication("DELETE");
            _output.WriteLine($"Adding {publicationDto} in DELETE test.");
            publicationDto = _publicationService.AddPublication(publicationDto);
            _output.WriteLine($"Added {publicationDto} in DELETE test.");

            _output.WriteLine($"Deleting {publicationDto} in DELETE test.");
            PublicationDto deleted = _publicationService.DeletePublication(publicationDto);
            _output.WriteLine($"Deleted {deleted} in DELETE test.");

            _output.WriteLine($"Getting {publicationDto.Id} in DELETE test.");
            PublicationDto got = _publicationService.GetPublication(publicationDto.Id);
            _output.WriteLine($"Got {got} in DELETE test.");

            try
            {
                Assert.Equal(publicationDto.Id, deleted.Id);
                Assert.Equal(publicationDto.Isbn, deleted.Isbn);
                Assert.Equal(publicationDto.Title, deleted.Title);
                Assert.Equal(publicationDto.Date, deleted.Date);
                Assert.Equal(publicationDto.Type, deleted.Type);
                Assert.Empty(deleted.AuthorPublicationList);
                Assert.Null(deleted.Publisher);
                Assert.Null(got);

                using (PublicationsContext db = new PublicationsContext())
                {
                    Assert.Empty(db.AuthorPublications.AsEnumerable());
                    Assert.Empty(db.Authors.AsEnumerable());
                    Assert.Empty(db.Publishers.AsEnumerable());
                    Assert.Empty(db.Publications.AsEnumerable());
                }
            }
            finally
            {
                if (got != null)
                {
                    TestUtils.Cleanup(_output);
                }
            }
        }

        [Fact]
        public void TestAddMorePublicationsAndGetAllPublications()
        {
            List<PublicationDto> list = new List<PublicationDto>();
            for (int i = 0; i < 10; i++)
            {
                PublicationDto publicationDto = TestUtils.CreatePublication("GET ALL" + i);
                publicationDto.Title = i.ToString();

                _output.WriteLine($"Adding {publicationDto} in GET ALL test.");
                _output.WriteLine($"Added {_publicationService.AddPublication(publicationDto)} in GET ALL test.");

                list.Add(publicationDto);
            }

            _output.WriteLine("Getting all in GET ALL test.");
            List<PublicationDto> got = (List<PublicationDto>) _publicationService.GetAllPublications();
            _output.WriteLine($"Got {got.Count} in GET ALL test.");
            foreach (PublicationDto publicationDto in got)
            {
                _output.WriteLine($"Got {publicationDto} in GET ALL test.");
            }

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
        public void TestAddTwoPublicationsWithTheSameAuthor()
        {
            PublicationDto publicationDto1 = TestUtils.CreatePublication();
            PublicationDto publicationDto2 = TestUtils.CreatePublication("Doctor Who");
            _output.WriteLine($"Adding {publicationDto1} in ADD TWO WITH SAME AUTHOR test.");
            publicationDto1 = _publicationService.AddPublication(publicationDto1);
            _output.WriteLine($"Added {publicationDto1} in ADD TWO WITH SAME AUTHOR test.");
            _output.WriteLine($"Adding {publicationDto2} in ADD TWO WITH SAME AUTHOR test.");
            publicationDto2 = _publicationService.AddPublication(publicationDto2);
            _output.WriteLine($"Added {publicationDto2} in ADD TWO WITH SAME AUTHOR test.");

            try
            {
                Assert.Equal(publicationDto1.AuthorPublicationList[0].AuthorId,
                    publicationDto2.AuthorPublicationList[0].AuthorId);
                Assert.Equal(publicationDto1.AuthorPublicationList[0].Author,
                    publicationDto2.AuthorPublicationList[0].Author);
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }
        
        [Fact]
        public void TestDeleteTwoPublicationsWithTheSameAuthor()
        {
            PublicationDto publicationDto1 = TestUtils.CreatePublication();
            PublicationDto publicationDto2 = TestUtils.CreatePublication("Doctor Who");
            _output.WriteLine($"Adding {publicationDto1} in DELETE TWO WITH SAME AUTHOR test.");
            publicationDto1 = _publicationService.AddPublication(publicationDto1);
            _output.WriteLine($"Added {publicationDto1} in DELETE TWO WITH SAME AUTHOR test.");
            _output.WriteLine($"Adding {publicationDto2} in DELETE TWO WITH SAME AUTHOR test.");
            publicationDto2 = _publicationService.AddPublication(publicationDto2);
            _output.WriteLine($"Added {publicationDto2} in DELETE TWO WITH SAME AUTHOR test.");
            
            _output.WriteLine($"Deleting {publicationDto1} in DELETE TWO WITH SAME AUTHOR test.");
            PublicationDto deleted1 = _publicationService.DeletePublication(publicationDto1);
            _output.WriteLine($"Deleted {deleted1} in DELETE TWO WITH SAME AUTHOR test.");

            _output.WriteLine($"Getting {publicationDto1.Id} in DELETE TWO WITH SAME AUTHOR test.");
            PublicationDto got1 = _publicationService.GetPublication(publicationDto1.Id);
            _output.WriteLine($"Got {got1} in DELETE test.");
            
            _output.WriteLine($"Getting {publicationDto2.Id} in DELETE TWO WITH SAME AUTHOR test.");
            PublicationDto got2 = _publicationService.GetPublication(publicationDto2.Id);
            _output.WriteLine($"Got {got2} in DELETE test.");

            try
            {
                Assert.Null(got1);
                Assert.Equal(publicationDto2, got2);
                
                using (PublicationsContext db = new PublicationsContext())
                {
                    IList<AuthorPublication> authorPublications = db.AuthorPublications.AsEnumerable().ToList();
                    IList<Author> authors = db.Authors.AsEnumerable().ToList();
                    IList<Publisher> publishers = db.Publishers.AsEnumerable().ToList();
                    IList<Publication> publications = db.Publications.AsEnumerable().ToList();
                    Assert.Equal(1, authorPublications.Count);
                    Assert.Equal(1, authors.Count);
                    Assert.Equal(1, publishers.Count);
                    Assert.Equal(1, publications.Count);
                    foreach (AuthorPublicationDto authorPublicationDto in publicationDto2.AuthorPublicationList)
                    {
                        Assert.Contains(_mapper.MapAuthorPublication(authorPublicationDto), authorPublications);
                        Assert.Contains(_mapper.MapAuthor(authorPublicationDto.Author), authors);
                    }

                    Assert.Contains(_mapper.MapPublisher(publicationDto2.Publisher), publishers);
                    Assert.Contains(_mapper.MapPublication(publicationDto2), publications);
                }
            }
            finally
            {
                TestUtils.Cleanup(_output);
            }
        }
    }
}