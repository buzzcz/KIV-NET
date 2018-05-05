using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    public class PublicationService : IPublicationService
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor to create Publication Service.
        /// </summary>
        /// <param name="mapper">Mapper used to map DTOs to entites and vice versa.</param>
        public PublicationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Deletes old entities from old publication (author-publications, authors and publisher)
        /// if they differ from the new one or if the new one is null.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="oldPublication">Old publication from which to delete entities.</param>
        /// <param name="publication">New publication with changed entities. Defaults to null.</param>
        private void DeleteOldPublicationSubentities(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            DeleteOldAuthors(db, oldPublication, publication);

            DeleteOldPublisher(db, oldPublication, publication);
        }

        /// <summary>
        /// Deletes old publisher from old publication if they differ from the new one or if the new one is null.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="oldPublication">Old publication from which to delete entities.</param>
        /// <param name="publication">New publication with changed entities. Defaults to null.</param>
        private static void DeleteOldPublisher(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            if (publication == null || !oldPublication.Publisher.Equals(publication.Publisher))
            {
                IList<Publication> used = db.Publications.AsNoTracking().Include(p => p.AuthorPublicationList)
                    .ThenInclude(ap => ap.Author).Include(p => p.Publisher)
                    .Where(p => p.Publisher.Id == oldPublication.Publisher.Id).ToList();

                if (used.Contains(oldPublication) && used.Count == 1)
                {
                    db.Publishers.Remove(oldPublication.Publisher);
                }
            }
        }

        /// <summary>
        /// Deletes old author-publications and authors from old publication if they differ from the new one or if the
        /// new one is null.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="oldPublication">Old publication from which to delete entities.</param>
        /// <param name="publication">New publication with changed entities. Defaults to null.</param>
        private void DeleteOldAuthors(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
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

        /// <summary>
        /// Checks if author from author-publication list already exists in the database and if so, this author is used
        /// instead of creating new one.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="publication">Publication where to check the authors.</param>
        private void CheckAlreadyExistingAuthor(PublicationsContext db, Publication publication)
        {
            if (publication.AuthorPublicationList != null)
            {
                IList<AuthorPublication> toRemove = new List<AuthorPublication>();
                IList<AuthorPublication> toAdd = new List<AuthorPublication>();
                foreach (AuthorPublication authorPublication in publication.AuthorPublicationList)
                {
                    Author author = authorPublication.Author;

                    Author existing = db.Authors.AsNoTracking().FirstOrDefault(a =>
                        a.FirstName == author.FirstName && a.LastName == author.LastName);
                    if (existing != null)
                    {
                        HandleExistingAuthor(db, authorPublication, existing, toRemove, toAdd);
                    }
                    else
                    {
                        authorPublication.Id = 0;
                    }
                }

                foreach (AuthorPublication authorPublication in toRemove)
                {
                    publication.AuthorPublicationList.Remove(authorPublication);
                }

                foreach (AuthorPublication authorPublication in toAdd)
                {
                    publication.AuthorPublicationList.Add(authorPublication);
                }
            }
        }

        /// <summary>
        /// Handles using existing author in publication.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="authorPublication">AuthorPublication to handle.</param>
        /// <param name="existing">Existing author.</param>
        /// <param name="toRemove">List of AuthorPublications that should be removed afterwards (their authors were
        /// edited and new ones are in toAdd list).</param>
        /// <param name="toAdd">List of AuthorPublications that should be added afterwards (authors from the original
        /// AuthorPublications were edited and old one are in toRemove list).</param>
        private static void HandleExistingAuthor(PublicationsContext db, AuthorPublication authorPublication, Author existing,
            IList<AuthorPublication> toRemove, IList<AuthorPublication> toAdd)
        {
            AuthorPublication oldAp = db.AuthorPublications.AsNoTracking().Include(ap => ap.Author)
                .FirstOrDefault(ap => ap.Id == authorPublication.Id);

            EntityEntry entry = db.ChangeTracker.Entries().FirstOrDefault(e => e.Entity.Equals(existing));
            if (entry != null)
            {
                HandleExistingAuthorLocal(authorPublication, toRemove, toAdd, entry, oldAp);
            }
            else
            {
                HandleExistingAuthorDb(db, authorPublication, existing, toRemove, toAdd, oldAp);
            }
        }

        private static void HandleExistingAuthorDb(PublicationsContext db, AuthorPublication authorPublication,
            Author existing, IList<AuthorPublication> toRemove, IList<AuthorPublication> toAdd,
            AuthorPublication oldAp)
        {
            if (oldAp != null && oldAp.Author.Equals(existing))
            {
                authorPublication.Author.Id = existing.Id;
                authorPublication.AuthorId = existing.Id;
                db.Entry(authorPublication.Author).State = EntityState.Unchanged;
            }
            else
            {
                toRemove.Add(authorPublication);
                AuthorPublication newAp = new AuthorPublication
                {
                    AuthorId = existing.Id,
                    Author = existing
                };
                db.Entry(newAp.Author).State = EntityState.Unchanged;
                toAdd.Add(newAp);
            }
        }

        private static void HandleExistingAuthorLocal(AuthorPublication authorPublication,
            IList<AuthorPublication> toRemove, IList<AuthorPublication> toAdd, EntityEntry entry,
            AuthorPublication oldAp)
        {
            entry.State = EntityState.Unchanged;
            if (oldAp != null && oldAp.Author.Equals(entry.Entity))
            {
                authorPublication.AuthorId = ((Author) entry.Entity).Id;
                authorPublication.Author = (Author) entry.Entity;
            }
            else
            {
                toRemove.Add(authorPublication);
                AuthorPublication newAp = new AuthorPublication
                {
                    AuthorId = ((Author) entry.Entity).Id,
                    Author = (Author) entry.Entity
                };
                toAdd.Add(newAp);
            }
        }

        /// <summary>
        /// Checks if publisher from publication already exists in the database and if so, this publisher is used
        /// instead of creating new one.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="publication">Publication where to check the publisher.</param>
        private static void CheckAlreadyExistingPublisher(PublicationsContext db, Publication publication)
        {
            Publisher publisher = publication.Publisher;

            Publisher existing = db.Publishers.AsNoTracking()
                .FirstOrDefault(p => p.Name == publisher.Name && p.Address == publisher.Address);
            if (existing != null)
            {
                EntityEntry entry = db.ChangeTracker.Entries().FirstOrDefault(e => e.Entity.Equals(existing));
                if (entry != null)
                {
                    entry.State = EntityState.Unchanged;
                    publication.Publisher = (Publisher) entry.Entity;
                }
                else
                {
                    publisher.Id = existing.Id;
                    db.Entry(publisher).State = EntityState.Unchanged;
                }
            }
        }

        public IList<PublicationDto> GetAllPublications()
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                IList<Book> bookEntities = db.Books.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author)
                    .Include(p => p.Publisher).ToList();
                IList<Article> articleEntities = db.Articles.Include(p => p.AuthorPublicationList)
                    .ThenInclude(ap => ap.Author).Include(p => p.Publisher).ToList();

                IList<BookDto> books = bookEntities.Select(b => _mapper.Map<BookDto>(b)).ToList();
                IList<ArticleDto> articles = articleEntities.Select(a => _mapper.Map<ArticleDto>(a)).ToList();

                List<PublicationDto> retVal = new List<PublicationDto>(books);
                retVal.AddRange(articles);

                return retVal;
            }
        }

        public BookDto AddBook(BookDto book)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = _mapper.Map<Book>(book);

                CheckAlreadyExistingAuthor(db, entity);
                CheckAlreadyExistingPublisher(db, entity);

                entity = db.Books.Add(entity).Entity;
                db.SaveChanges();

                return _mapper.Map<BookDto>(entity);
            }
        }

        public BookDto GetBook(int id)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = db.Books.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author)
                    .Include(p => p.Publisher).FirstOrDefault(p => p.Id == id);

                return _mapper.Map<BookDto>(entity);
            }
        }

        public BookDto EditBook(BookDto book)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = _mapper.Map<Book>(book);
                Book oldBook = db.Books.AsNoTracking().Include(p => p.AuthorPublicationList)
                    .ThenInclude(ap => ap.Author).Include(p => p.Publisher).FirstOrDefault(p => p.Id == book.Id);

                CheckAlreadyExistingAuthor(db, entity);
                CheckAlreadyExistingPublisher(db, entity);

                entity = db.Books.Update(entity).Entity;
                db.SaveChanges();
                
                DeleteOldPublicationSubentities(db, oldBook, entity);
                db.SaveChanges();

                return _mapper.Map<BookDto>(entity);
            }
        }

        public BookDto DeletePublication(int id)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = db.Publications.AsNoTracking().Include(p => p.AuthorPublicationList)
                    .ThenInclude(ap => ap.Author).Include(p => p.Publisher).FirstOrDefault(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }

                DeleteOldPublicationSubentities(db, entity);

                entity = db.Publications.Remove(entity).Entity;
                db.SaveChanges();

                return _mapper.Map<BookDto>(entity);
            }
        }

        public ArticleDto AddArticle(ArticleDto article)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Article entity = _mapper.Map<Article>(article);

                CheckAlreadyExistingAuthor(db, entity);
                CheckAlreadyExistingPublisher(db, entity);

                entity = db.Articles.Add(entity).Entity;
                db.SaveChanges();

                return _mapper.Map<ArticleDto>(entity);
            }
        }

        public ArticleDto GetArticle(int id)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Article entity = db.Articles.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author)
                    .Include(p => p.Publisher).FirstOrDefault(p => p.Id == id);

                return _mapper.Map<ArticleDto>(entity);
            }
        }

        public ArticleDto EditArticle(ArticleDto article)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Article entity = _mapper.Map<Article>(article);
                Article oldArticle = db.Articles.AsNoTracking().Include(p => p.AuthorPublicationList)
                    .ThenInclude(ap => ap.Author).Include(p => p.Publisher).FirstOrDefault(p => p.Id == article.Id);

                CheckAlreadyExistingAuthor(db, entity);
                CheckAlreadyExistingPublisher(db, entity);

                entity = db.Articles.Update(entity).Entity;
                db.SaveChanges();

                DeleteOldPublicationSubentities(db, oldArticle, entity);
                db.SaveChanges();

                return _mapper.Map<ArticleDto>(entity);
            }
        }
    }
}