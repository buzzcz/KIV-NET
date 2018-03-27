using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;
using TestProject.Utils;
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
        public void TestSaveAndLoadBook()
        {
            using (var db = new PublicationsContext())
            {
                Author author = new Author
                {
                    FirstName = "Douglas",
                    LastName = "Adams"
                };

                Publisher publisher = new Publisher
                {
                    Name = "ZČU",
                    Address = "Pilsen"
                };

                Book book = new Book
                {
                    Isbn = "8375209824",
                    Title = "Hitchhiker's Guide to the Galaxy",
                    Date = DateTime.Now,
                    Publisher = publisher,
                    Edition = "1st"
                };

                book = db.Books.Add(book).Entity;
                author = db.Authors.Add(author).Entity;
                db.SaveChanges();

                AuthorPublication authorPublication = new AuthorPublication
                {
                    AuthorId = author.Id,
                    PublicationId = book.Id
                };
                authorPublication = db.AuthorPublications.Add(authorPublication).Entity;
                db.SaveChanges();
                _output.WriteLine($"AuthorPublication: {authorPublication}.");

                IList<Book> selected = db.Books.Where(s => s.Isbn == "8375209824").Include(p => p.AuthorPublicationList)
                    .Include(p => p.Publisher).ToList();
                _output.WriteLine($"Publication: {book}.");
                foreach (Book b in selected)
                {
                    _output.WriteLine($"Selected publication: {b}.");
                }

                try
                {
                    Assert.Contains(book, selected);
                    Assert.Equal(authorPublication, book.AuthorPublicationList[0]);
                    Assert.Equal(authorPublication, selected[0].AuthorPublicationList[0]);
                }
                finally
                {
                    db.AuthorPublications.Remove(authorPublication);
                    db.Authors.Remove(author);
                    db.Books.Remove(book);
                    db.Publishers.Remove(publisher);
                    db.SaveChanges();
                }
            }
        }
    }
}