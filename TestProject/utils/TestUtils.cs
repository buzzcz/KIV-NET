using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Utils
{
    public static class TestUtils
    {
        public static BookDto CreateBook(string title = "Hitchhiker's Guide to the Galaxy")
        {
            DateTime dateTime = DateTime.FromOADate(DateTime.Now.ToOADate());
            BookDto bookDto = new BookDto
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
                    Address = "Pilsen",
                    Name = "University Press"
                },
                Title = title,
                Edition = "1st Edition"
            };

            return bookDto;
        }

        public static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(conf =>
            {
                conf.CreateMap<Author, AuthorDto>();
                conf.CreateMap<AuthorPublication, AuthorPublicationDto>();
                conf.CreateMap<Book, BookDto>();
                conf.CreateMap<Publication, PublicationDto>();
                conf.CreateMap<Publisher, PublisherDto>();
            }));
        }

        public static void Cleanup(ITestOutputHelper output)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                output.WriteLine("Cleanup db.");
                db.Database.ExecuteSqlCommand(
                    "delete from AuthorPublications; delete from Authors; delete from Books; delete from Publishers;");
                
                Assert.Empty(db.AuthorPublications.AsEnumerable());
                Assert.Empty(db.Authors.AsEnumerable());
                Assert.Empty(db.Publishers.AsEnumerable());
                Assert.Empty(db.Books.AsEnumerable());
            }
        }
    }
}