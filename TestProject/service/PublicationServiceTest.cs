using System;
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
        }
        
        private static PublicationDto CreatePublication()
        {
            DateTime dateTime = DateTime.FromOADate(DateTime.Now.ToOADate());
            PublicationDto publicationDto = new PublicationDto
            {
                Author = new AuthorDto
                {
                    FirstName = "Douglas",
                    LastName = "Adams"
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
                Title = "Hitchhiker's Guide to the Galaxy",
                Type = PublicationType.BOOK
            };

            return publicationDto;
        }

        private static void Cleanup(Publication entity)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                db.Addresses.Remove(entity.Publisher.Address);
                db.Authors.Remove(entity.Author);
                db.Publishers.Remove(entity.Publisher);
                db.Publications.Remove(entity);
                db.SaveChanges();
            }
        }
        
        [Fact]
        public void TestAddPublication()
        {
            PublicationDto publicationDto = CreatePublication();
            _output.WriteLine($"Adding {publicationDto} in ADD test.");
            PublicationDto added = _publicationService.AddPublication(publicationDto);
            _output.WriteLine($"Added {added} in ADD test.");

            Assert.Equal(publicationDto, added);

            using (PublicationsContext db = new PublicationsContext())
            {
                _output.WriteLine($"Getting {added.Id} in ADD test from RAW db ctx.");
                Publication entity = db.Publications.Include(p => p.Author).Include(p => p.Publisher)
                    .ThenInclude(p => p.Address).First(p => p.Id == added.Id);
                db.SaveChanges();
                _output.WriteLine($"Got {entity} in ADD test.");
                added = _mapper.MapPublication(entity);
            }

            try
            {
                Assert.Equal(publicationDto, added);
            }
            finally
            {
                Cleanup(_mapper.MapPublication(added));
            }
        }

        [Fact]
        public void TestGetPublication()
        {
            PublicationDto publicationDto = CreatePublication();
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
    }
}