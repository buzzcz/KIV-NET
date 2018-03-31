using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private static void DeleteOldPublicationSubentities(PublicationsContext db, Publication oldPublication,
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
                    .Where(p => p.Publisher.Id == oldPublication.Publisher.Id).Include(p => p.AuthorPublicationList)
                    .ThenInclude(ap => ap.Author).Include(p => p.Publisher).ToList();
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

        /// <summary>
        /// Deletes old author-publications and authors from old publication if they differ from the new one or if the
        /// new one is null.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="oldPublication">Old publication from which to delete entities.</param>
        /// <param name="publication">New publication with changed entities. Defaults to null.</param>
        private static void DeleteOldAuthors(PublicationsContext db, Publication oldPublication,
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
        private static void CheckAlreadyExistingAuthor(PublicationsContext db, Publication publication)
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

        /// <summary>
        /// Checks if publisher from publication already exists in the database and if so, this publisher is used
        /// instead of creating new one.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="publication">Publication where to check the publisher.</param>
        private static void CheckAlreadyExistingPublisher(PublicationsContext db, Publication publication)
        {
            Publisher publisher = publication.Publisher;

            Publisher existing = db.Publishers.AsNoTracking().FirstOrDefault(p => p.Name == publisher.Name);
            if (existing != null)
            {
                publisher.Id = existing.Id;
                db.Entry(publisher).State = EntityState.Unchanged;
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

                DeleteOldPublicationSubentities(db, oldBook, entity);

                CheckAlreadyExistingAuthor(db, entity);
                CheckAlreadyExistingPublisher(db, entity);

                entity = db.Books.Update(entity).Entity;
                db.SaveChanges();

                return _mapper.Map<BookDto>(entity);
            }
        }

        public BookDto DeleteBook(BookDto book)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = _mapper.Map<Book>(book);

                DeleteOldPublicationSubentities(db, entity);

                entity = db.Books.Remove(entity).Entity;
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

                DeleteOldPublicationSubentities(db, oldArticle, entity);

                CheckAlreadyExistingAuthor(db, entity);
                CheckAlreadyExistingPublisher(db, entity);

                entity = db.Articles.Update(entity).Entity;
                db.SaveChanges();

                return _mapper.Map<ArticleDto>(entity);
            }
        }

        public ArticleDto DeleteArticle(ArticleDto article)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Article entity = _mapper.Map<Article>(article);

                DeleteOldPublicationSubentities(db, entity);

                entity = db.Articles.Remove(entity).Entity;
                db.SaveChanges();

                return _mapper.Map<ArticleDto>(entity);
            }
        }
    }
}