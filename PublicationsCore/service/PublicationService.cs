using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    public class PublicationService : IPublicationService
    {
        public void DeleteOldPublicationSubentities(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            DeleteOldAuthors(db, oldPublication, publication);

            DeleteOldPublisher(db, oldPublication, publication);
        }

        public void DeleteOldPublisher(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            if (publication == null || !oldPublication.Publisher.Equals(publication.Publisher))
            {
                IQueryable<Publication> query = null;
                if (oldPublication is Book)
                {
                    query = db.Books.AsNoTracking();
                }

                IList<Publication> used = query.Where(p => p.Publisher.Id == oldPublication.Publisher.Id)
                    .Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author).Include(p => p.Publisher)
                    .ToList();
                foreach (var publication1 in used)
                {
                    Console.WriteLine($"List: {publication1}");
                }

                if (used.Contains(oldPublication) && used.Count == 1)
                {
                    db.Publishers.Remove(oldPublication.Publisher);
                }
            }
        }

        public void DeleteOldAuthors(PublicationsContext db, Publication oldPublication, Publication publication = null)
        {
            if (oldPublication.AuthorPublicationList != null)
            {
                IList<AuthorPublication> authorPublicationsToRemove = publication != null
                    ? oldPublication.AuthorPublicationList
                        .Where(p => publication.AuthorPublicationList.All(p2 => p.Id != p2.Id)).ToList()
                    : oldPublication.AuthorPublicationList.ToList();

                db.AuthorPublications.RemoveRange(authorPublicationsToRemove);
                foreach (AuthorPublication authorPublication in authorPublicationsToRemove)
                {
                    IList<AuthorPublication> used = db.AuthorPublications.AsNoTracking().Include(ap => ap.Author)
                        .Where(ap => ap.AuthorId == authorPublication.AuthorId).ToList();
                    if (used.Contains(authorPublication) && used.Count == 1)
                    {
                        db.Authors.Remove(authorPublication.Author);
                    }
                }
            }
        }

        public void CheckAlreadyExistingAuthor(PublicationsContext db, Publication publication)
        {
            if (publication.AuthorPublicationList != null)
            {
                foreach (AuthorPublication authorPublication in publication.AuthorPublicationList)
                {
                    Author author = authorPublication.Author;

                    Author existing = db.Authors.AsNoTracking().FirstOrDefault(a =>
                        a.FirstName == author.FirstName && a.LastName == author.LastName);
                    if (existing != null)
                    {
                        author.Id = existing.Id;
                        authorPublication.AuthorId = existing.Id;
                        db.Entry(author).State = EntityState.Unchanged;
                    }
                }
            }
        }

        public void CheckAlreadyExistingPublisher(PublicationsContext db, Publication publication)
        {
            Publisher publisher = publication.Publisher;

            Publisher existing = db.Publishers.AsNoTracking().FirstOrDefault(p => p.Name == publisher.Name);
            if (existing != null)
            {
                publisher.Id = existing.Id;
                db.Entry(publisher).State = EntityState.Unchanged;
            }
        }
    }
}