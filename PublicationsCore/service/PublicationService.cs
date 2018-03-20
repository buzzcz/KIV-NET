using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    /// <summary>
    /// Implementation of <i>IPublicationService</i>.
    /// </summary>
    public class PublicationService : IPublicationService
    {
        /// <summary>
        /// Mapper between DTOs and entities.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for creating <i>PublicationService</i>.
        /// </summary>
        /// <param name="mapper">Mapper between DTOs and entities.</param>
        public PublicationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets publication with filled authors from entity.
        /// </summary>
        /// <param name="entity">Entity from which to get publication.</param>
        /// <returns>Publication with filled authors.</returns>
        private PublicationDto GetPublication(Publication entity)
        {
            if (entity != null)
            {
                return _mapper.MapPublication(entity);
            }

            return null;
        }

        /// <summary>
        /// Prepares publication query with its includes.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="asNoTracking"></param>
        /// <returns>Publication query with included AuthorPublications, Publishers and his address.</returns>
        private static IIncludableQueryable<Publication, Address> PreparePublicationsDb(PublicationsContext db,
            bool asNoTracking = false)
        {
            var retVal = asNoTracking ? db.Publications.AsNoTracking() : db.Publications;

            return retVal.Include(p => p.AuthorPublicationList).ThenInclude(ap => ap.Author).Include(p => p.Publisher)
                .ThenInclude(p => p.Address);
        }


        private static void DeleteOldPublicationSubentities(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            DeleteOldAuthors(db, oldPublication, publication);

            DeleteOldPublisher(db, oldPublication, publication);

            DeleteOldAddress(db, oldPublication, publication);
        }

        private static void DeleteOldAddress(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            if (publication == null || !oldPublication.Publisher.Address.Equals(publication.Publisher.Address))
            {
                IList<Publisher> used = db.Publishers.AsNoTracking().Include(p => p.Address)
                    .Where(p => p.Address.Id == oldPublication.Publisher.Id).ToList();
                if (used.Contains(oldPublication.Publisher) && used.Count == 1)
                {
                    db.Addresses.Remove(oldPublication.Publisher.Address);
                }
            }
        }

        private static void DeleteOldPublisher(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            if (publication == null || !oldPublication.Publisher.Equals(publication.Publisher))
            {
                IList<Publication> used = PreparePublicationsDb(db, true)
                    .Where(p => p.Publisher.Id == oldPublication.Publisher.Id).ToList();
                Console.WriteLine(
                    $"\nDEBUG========================================\n{used.Aggregate("", (current, publication1) => current + publication1.ToString())}");
                if (used.Contains(oldPublication) && used.Count == 1)
                {
                    Console.WriteLine(
                        $"\nDEBUG========================================\nRemoving{oldPublication.Publisher}");
                    db.Publishers.Remove(oldPublication.Publisher);
                }
            }
        }

        private static void DeleteOldAuthors(PublicationsContext db, Publication oldPublication,
            Publication publication = null)
        {
            if (oldPublication.AuthorPublicationList != null)
            {
                IList<AuthorPublication> authorPublicationsToRemove = publication != null
                    ? oldPublication.AuthorPublicationList
                        .Where(p => publication.AuthorPublicationList.All(p2 => p.Id != p2.Id)).ToList()
                    : (IList<AuthorPublication>) oldPublication.AuthorPublicationList.AsEnumerable();

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

        public PublicationDto AddPublication(PublicationDto publication)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = _mapper.MapPublication(publication);
                // TODO: Klaus - Check if subentity (e.g. address) already exists. Don't create new if not necessary.
                entity = db.Publications.Add(entity).Entity;
                db.SaveChanges();

                return GetPublication(entity);
            }
        }

        public PublicationDto GetPublication(int id)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = PreparePublicationsDb(db).FirstOrDefault(p => p.Id == id);

                return GetPublication(entity);
            }
        }

        public IList<PublicationDto> GetAllPublications()
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                IList<PublicationDto> list = new List<PublicationDto>();
                IList<Publication> entities = new List<Publication>(PreparePublicationsDb(db).AsEnumerable());

                foreach (var entity in entities)
                {
                    list.Add(GetPublication(entity));
                }

                return list;
            }
        }

        public PublicationDto EditPublication(PublicationDto publication)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                // TODO: Klaus - Check for existing subentities (don't create new).
                Publication entity = _mapper.MapPublication(publication);
                Publication oldPublication =
                    PreparePublicationsDb(db, true).FirstOrDefault(p => p.Id == publication.Id);
                DeleteOldPublicationSubentities(db, oldPublication, entity);

                entity = db.Publications.Update(entity).Entity;
                db.SaveChanges();

                return GetPublication(entity);
            }
        }

        public PublicationDto DeletePublication(PublicationDto publication)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = _mapper.MapPublication(publication);
                DeleteOldPublicationSubentities(db, entity);
                entity = db.Publications.Remove(entity).Entity;
                db.SaveChanges();

                return GetPublication(entity);
            }
        }
    }
}