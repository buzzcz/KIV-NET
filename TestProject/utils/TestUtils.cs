using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Facade.Enums;
using PublicationsCore.Persistence;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Utils
{
    public static class TestUtils
    {
        public static PublicationDto CreatePublication(string title = "Hitchhiker's Guide to the Galaxy")
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
                    Address = "Pilsen",
                    Name = "University Press"
                },
                Title = title,
                Type = PublicationType.BOOK,
                Edition = "1st Edition"
            };

            return publicationDto;
        }

        public static void Cleanup(ITestOutputHelper output)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                output.WriteLine("Cleanup db.");
                db.Database.ExecuteSqlCommand(
                    "delete from Authors; delete from Publishers; delete from Publications; delete from AuthorPublications;");
                
                Assert.Empty(db.AuthorPublications.AsEnumerable());
                Assert.Empty(db.Authors.AsEnumerable());
                Assert.Empty(db.Publishers.AsEnumerable());
                Assert.Empty(db.Publications.AsEnumerable());
            }
        }
    }
}