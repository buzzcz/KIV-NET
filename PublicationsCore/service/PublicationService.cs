using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public PublicationDto AddPublication(PublicationDto publication)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = _mapper.MapPublication(publication);
                // TODO: Klaus - Check if subentity (e.g. address) already exists. Don't create new if not necessary.
                entity = db.Publications.Add(entity).Entity;
                db.SaveChanges();

                return _mapper.MapPublication(entity);   
            }
        }

        public PublicationDto GetPublication(int id)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = db.Publications.Include(p => p.Author).Include(p => p.Publisher)
                    .ThenInclude(p => p.Address).FirstOrDefault(p => p.Id == id);

                return entity != null ? _mapper.MapPublication(entity) : null;
            }
        }

        public PublicationDto EditPublication(PublicationDto publication)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = db.Publications.FirstOrDefault(p => p.Id == publication.Id);

                if (entity == null)
                {
                    return null;
                }

                entity = _mapper.MapPublication(publication);
                entity = db.Publications.Update(entity).Entity;
                db.SaveChanges();

                return _mapper.MapPublication(entity);
            }
        }

        public PublicationDto DeletePublication(int id)
        {
            using (PublicationsContext db = new PublicationsContext())
            {
                Publication entity = db.Publications.First(p => p.Id == id);

                if (entity == null)
                {
                    return null;
                }

                entity = db.Publications.Remove(entity).Entity;
                db.SaveChanges();

                return _mapper.MapPublication(entity);
            }
        }
    }
}