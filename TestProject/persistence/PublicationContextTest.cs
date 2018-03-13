using System;
using System.Linq;
using PublicationsCore.Facade.Enums;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;
using Xunit;

namespace TestProject.Persistence
{
    public class PublicationContextTest
    {
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
                Assert.Contains(a, selected);

                ctx.Authors.Remove(a);
                ctx.SaveChanges();
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
                    Author = author,
                    Isbn = "8375209824",
                    Title = "Hitchhiker's Guide to the Galaxy",
                    Date = DateTime.Now,
                    Type = PublicationType.BOOK,
                    Publisher = publisher
                };

                ctx.Publications.Add(publication);
                ctx.SaveChanges();

                var selected = ctx.Publications.Where(s => s.Isbn == "8375209824").ToList();
                Assert.Contains(publication, selected);

                ctx.Publications.Remove(publication);
                ctx.Authors.Remove(author);
                ctx.Addresses.Remove(address);
                ctx.Publishers.Remove(publisher);
                ctx.SaveChanges();
            }
        }
    }
}