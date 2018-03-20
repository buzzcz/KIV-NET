using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Facade.Enums;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;
using PublicationsCore.Service;
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

        private static PublicationDto CreatePublication(string title = "Hitchhiker's Guide to the Galaxy")
        {
            DateTime dateTime = DateTime.FromOADate(DateTime.Now.ToOADate());
            PublicationDto publicationDto = new PublicationDto
            {
                AuthorPublicationList = new List<AuthorPublicationDto>
                {
                   new AuthorPublicationDto
                   {
                       Author = new AuthorDto
                       {
                           FirstName = "Douglas",
                           LastName = "Adams"
                       }
                   }
                },
                Date = dateTime,
                Isbn = "7892347-913-2341-09",
                Publisher = new PublisherDto
                {
                    Address = new AddressDto
                    {
                        City = "Pilsen",
                        Number = 8,
                        State = "CZ",
                        Street = "Univerzitní"
                    },
                    Name = "University Press"
                },
                Title = title,
                Type = PublicationType.BOOK
            };

            return publicationDto;
        }

        private void Cleanup(Publication entity)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                db.Addresses.Remove(entity.Publisher.Address);
                db.Publishers.Remove(entity.Publisher);
                if (entity.AuthorPublicationList != null)
                {
                    _output.WriteLine(entity.AuthorPublicationList.Aggregate("", (current, authorPublication) => current + $"{authorPublication}; "));
                    db.AuthorPublications.RemoveRange(entity.AuthorPublicationList);

                    foreach (AuthorPublication authorPublication in entity.AuthorPublicationList)
                    {
                        db.Authors.Remove(authorPublication.Author);
                    }
                }

                db.Publications.Remove(entity);
                db.SaveChanges();
            }
        }

        [Fact]
        public void TestAddPublication()
        {
            PublicationDto publicationDto = CreatePublication("ADD");
            _output.WriteLine($"Adding {publicationDto} in ADD test.");
            PublicationDto added = _publicationService.AddPublication(publicationDto);
            _output.WriteLine($"Added {added} in ADD test.");

            PublicationDto got;
            using (PublicationsContext db = new PublicationsContext())
            {
                _output.WriteLine($"Getting {added.Id} in ADD test from RAW db ctx.");
                Publication entity = db.Publications.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author)
                    .Include(p => p.Publisher).ThenInclude(p => p.Address).First(p => p.Id == added.Id);
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
                Cleanup(_mapper.MapPublication(added));
            }
        }

        [Fact]
        public void TestGetPublication()
        {
            PublicationDto publicationDto = CreatePublication("GET");
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
                Cleanup(_mapper.MapPublication(got));
            }
        }

        [Fact]
        public void TestEditPublicationTitle()
        {
            PublicationDto publicationDto = CreatePublication("EDIT TITLE");
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
                Cleanup(_mapper.MapPublication(got));
            }
        }

        [Fact]
        public void TestEditPublicationAuthor()
        {
            PublicationDto publicationDto = CreatePublication("EDIT AUTHOR");
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
                Cleanup(_mapper.MapPublication(got));
            }
        }

        [Fact]
        public void TestDeletePublication()
        {
            PublicationDto publicationDto = CreatePublication("DELETE");
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
                Assert.True(!deleted.AuthorPublicationList.Any());
                Assert.Equal(null, deleted.Publisher);
                Assert.Equal(null, got);
            }
            finally
            {
                if (got != null)
                {
                    Cleanup(_mapper.MapPublication(got));
                }
            }
        }

        [Fact]
        public void TestGetAllPublications()
        {
            List<PublicationDto> list = new List<PublicationDto>();
            for (int i = 0; i < 10; i++)
            {
                PublicationDto publicationDto = CreatePublication("GET ALL" + i);
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
                foreach (var publication in got)
                {
                    Cleanup(_mapper.MapPublication(publication));
                }
            }
        }
    }
}