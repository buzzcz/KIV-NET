using System.Linq;
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
        /// Database context.
        /// </summary>
        private readonly PublicationsContext _db;

        /// <summary>
        /// Mapper between DTOs and entities.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for creating <i>PublicationService</i>.
        /// </summary>
        /// <param name="db">Database context used for database operations.</param>
        /// <param name="mapper">Mapper between DTOs and entities.</param>
        public PublicationService(PublicationsContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public PublicationDto AddPublication(PublicationDto publication)
        {
            Publication entity = _mapper.MapPublication(publication);
            // TODO: Klaus - Check if subentity (e.g. address) already exists. Don't create new if not necessary.
            entity = _db.Publications.Add(entity).Entity;
            _db.SaveChanges();

            return _mapper.MapPublication(entity);
        }

        public PublicationDto GetPublication(int id)
        {
            Publication entity = _db.Publications.DefaultIfEmpty(null).FirstOrDefault(p => p.Id == id);

            return entity != null ? _mapper.MapPublication(entity) : null;
        }

        public PublicationDto EditPublication(PublicationDto publication)
        {
            Publication entity = _db.Publications.DefaultIfEmpty(null).FirstOrDefault(p => p.Id == publication.Id);

            if (entity == null)
            {
                return null;
            }

            entity = _mapper.MapPublication(publication);
            entity = _db.Publications.Update(entity).Entity;
            _db.SaveChanges();

            return _mapper.MapPublication(entity);
        }

        public PublicationDto DeletePublication(int id)
        {
            Publication entity = _db.Publications.DefaultIfEmpty(null).First(p => p.Id == id);

            if (entity == null)
            {
                return null;
            }
            
            entity = _db.Publications.Remove(entity).Entity;
            _db.SaveChanges();

            return _mapper.MapPublication(entity);
        }
    }
}