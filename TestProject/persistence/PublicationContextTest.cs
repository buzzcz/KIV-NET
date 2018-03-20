using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PublicationsCore.Facade.Enums;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Persistence
{
    public class PublicationContextTest
    {
        
        private readonly ITestOutputHelper _output;

        public PublicationContextTest(ITestOutputHelper output)
        {
            _output = output;
            Console.SetOut(new ConsoleOutToITestOutputHelper(output));
        }
        
        [Fact]
        public void TestSaveAndLoadAuthor()
        {
            using (var ctx = new PublicationsContext())
            {
                Author a = new Author
                {
                    FirstName = "Douglas",
                    LastName = "Adams"
                };

                ctx.Authors.Add(a);
                ctx.SaveChanges();

                var selected = ctx.Authors.Where(s => s.LastName == "Adams").ToList();

                try
                {
                    Assert.Contains(a, selected);
                }
                finally
                {
                    ctx.Authors.Remove(a);
                    ctx.SaveChanges();
                }
            }
        }

        [Fact]
        public void TestSaveAndLoadPublication()
        {
            using (var ctx = new PublicationsContext())
            {
                Author author = new Author
                {
                    FirstName = "Douglas",
                    LastName = "Adams"
                };

                Address address = new Address
                {
                    State = "CZ",
                    City = "Pilsen",
                    Street = "Univerzitní",
                    Number = 8
                };

                Publisher publisher = new Publisher
                {
                    Name = "ZČU",
                    Address = address
                };

                Publication publication = new Publication
                {
                    Isbn = "8375209824",
                    Title = "Hitchhiker's Guide to the Galaxy",
                    Date = DateTime.Now,
                    Type = PublicationType.BOOK,
                    Publisher = publisher
                };

                publication = ctx.Publications.Add(publication).Entity;
                author = ctx.Authors.Add(author).Entity;
                ctx.SaveChanges();

                AuthorPublication authorPublication = new AuthorPublication
                {
                    AuthorId = author.Id,
                    PublicationId = publication.Id
                };
                authorPublication = ctx.AuthorPublications.Add(authorPublication).Entity;
                ctx.SaveChanges();

                var selected = ctx.Publications.Where(s => s.Isbn == "8375209824").Include(p => p.AuthorPublicationList)
                    .Include(p => p.Publisher).ThenInclude(p => p.Address).ToList();
                _output.WriteLine($"Publication: {publication}.");
                foreach (Publication p in selected)
                {
                    _output.WriteLine($"Selected publication: {p}.");
                }

                try
                {
                    Assert.Contains(publication, selected);
                    Assert.Equal(authorPublication, publication.AuthorPublicationList[0]);
                    Assert.Equal(authorPublication, selected[0].AuthorPublicationList[0]);
                }
                finally
                {
                    ctx.Publications.Remove(publication);
                    ctx.Authors.Remove(author);
                    ctx.Addresses.Remove(address);
                    ctx.Publishers.Remove(publisher);
                    ctx.SaveChanges();
                }
            }
        }
    }
}