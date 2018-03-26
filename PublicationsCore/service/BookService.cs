using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    /// <summary>
    /// Implementation of <i>IBookService</i>.
    /// </summary>
    public class BookService : IBookService
    {
        /// <summary>
        /// Mapper between DTOs and entities.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Service handling general publications.
        /// </summary>
        private readonly IPublicationService _publicationService;

        /// <summary>
        /// Constructor for creating <i>BookService</i>.
        /// </summary>
        /// <param name="mapper">Mapper between DTOs and entities.</param>
        /// <param name="publicationService">Publication service handling general publications.</param>
        public BookService(IMapper mapper, IPublicationService publicationService)
        {
            _mapper = mapper;
            _publicationService = publicationService;
        }

        /// <summary>
        /// Gets book mapped from entity or null.
        /// </summary>
        /// <param name="entity">Entity from which to get book.</param>
        /// <returns>Book or null of entiy was null.</returns>
        private BookDto GetBook(Book entity)
        {
            if (entity != null)
            {
                return _mapper.Map<BookDto>(entity);
            }

            return null;
        }

        /// <summary>
        /// Prepares book query with its includes.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="asNoTracking"></param>
        /// <returns>Book query with included AuthorPublications and Publishers.</returns>
        private static IIncludableQueryable<Book, Publisher> PrepareBookDb(PublicationsContext db,
            bool asNoTracking = false)
        {
            var retVal = asNoTracking ? db.Books.AsNoTracking() : db.Books;

            return retVal.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author).Include(p => p.Publisher);
        }

        public BookDto AddBook(BookDto book)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = _mapper.Map<Book>(book);
                
                _publicationService.CheckAlreadyExistingAuthor(db, entity);
                _publicationService.CheckAlreadyExistingPublisher(db, entity);

                entity = db.Books.Add(entity).Entity;
                db.SaveChanges();

                return GetBook(entity);
            }
        }

        public BookDto GetBook(int id)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = PrepareBookDb(db).FirstOrDefault(p => p.Id == id);

                return GetBook(entity);
            }
        }

        public IList<BookDto> GetAllBooks()
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                IList<Book> entities = new List<Book>(PrepareBookDb(db).AsEnumerable());

                return entities.Select(GetBook).ToList();
            }
        }

        public BookDto EditBook(BookDto book)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = _mapper.Map<Book>(book);
                Book oldBook = PrepareBookDb(db, true).FirstOrDefault(p => p.Id == book.Id);
                
                _publicationService.DeleteOldPublicationSubentities(db, oldBook, entity);
                
                _publicationService.CheckAlreadyExistingAuthor(db, entity);

                entity = db.Books.Update(entity).Entity;
                db.SaveChanges();

                return GetBook(entity);
            }
        }

        public BookDto DeleteBook(BookDto book)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Book entity = _mapper.Map<Book>(book);
                
                _publicationService.DeleteOldPublicationSubentities(db, entity);
                
                entity = db.Books.Remove(entity).Entity;
                db.SaveChanges();

                return GetBook(entity);
            }
        }
    }
}