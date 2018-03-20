using System.Collections.Generic;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    /// <summary>
    /// Implementation of the <i>IMapper</i> interface.
    /// </summary>
    public class Mapper : IMapper
    {
        public PublicationDto MapPublication(Publication publication)
        {
            if (publication == null)
            {
                return null;
            }
            
            IList<AuthorPublicationDto> list = new List<AuthorPublicationDto>();
            if (publication.AuthorPublicationList != null)
            {
                foreach (AuthorPublication authorPublication in publication.AuthorPublicationList)
                {
                    list.Add(MapAuthorPublication(authorPublication));
                }
            }

            return new PublicationDto
            {
                Date = publication.Date,
                Id = publication.Id,
                Isbn = publication.Isbn,
                Publisher = MapPublisher(publication.Publisher),
                Title = publication.Title,
                Type = publication.Type,
                AuthorPublicationList = list
            };

        }

        public Publication MapPublication(PublicationDto publication)
        {
            if (publication == null)
            {
                return null;
            }
            
            IList<AuthorPublication> list = new List<AuthorPublication>();
            if (publication.AuthorPublicationList != null)
            {
                foreach (AuthorPublicationDto authorPublication in publication.AuthorPublicationList)
                {
                    list.Add(MapAuthorPublication(authorPublication));
                }
            }

            return new Publication
            {
                Date = publication.Date,
                Id = publication.Id,
                Isbn = publication.Isbn,
                Publisher = MapPublisher(publication.Publisher),
                Title = publication.Title,
                Type = publication.Type,
                AuthorPublicationList = list
            };
        }

        public AddressDto MapAddress(Address address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressDto
            {
                City = address.City,
                Id = address.Id,
                Number = address.Number,
                State = address.State,
                Street = address.Street
            };
        }

        public Address MapAddress(AddressDto address)
        {
            if (address == null)
            {
                return null;
            }

            return new Address
            {
                City = address.City,
                Id = address.Id,
                Number = address.Number,
                State = address.State,
                Street = address.Street
            };
        }

        public AuthorDto MapAuthor(Author author)
        {
            if (author == null)
            {
                return null;
            }

            return new AuthorDto
            {
                FirstName = author.FirstName,
                Id = author.Id,
                LastName = author.LastName
            };
        }

        public Author MapAuthor(AuthorDto author)
        {
            if (author == null)
            {
                return null;
            }

            return new Author
            {
                FirstName = author.FirstName,
                Id = author.Id,
                LastName = author.LastName
            };
        }

        public PublisherDto MapPublisher(Publisher publisher)
        {
            if (publisher == null)
            {
                return null;
            }

            return new PublisherDto
            {
                Address = MapAddress(publisher.Address),
                Id = publisher.Id,
                Name = publisher.Name
            };
        }

        public Publisher MapPublisher(PublisherDto publisher)
        {
            if (publisher == null)
            {
                return null;
            }

            return new Publisher
            {
                Address = MapAddress(publisher.Address),
                Id = publisher.Id,
                Name = publisher.Name
            };
        }

        public AuthorPublicationDto MapAuthorPublication(AuthorPublication authorPublication)
        {
            if (authorPublication == null)
            {
                return null;
            }

            return new AuthorPublicationDto
            {
                Author = MapAuthor(authorPublication.Author),
                AuthorId = authorPublication.AuthorId,
                Id = authorPublication.Id,
                PublicationId = authorPublication.PublicationId
            };
        }

        public AuthorPublication MapAuthorPublication(AuthorPublicationDto authorPublication)
        {
            if (authorPublication == null)
            {
                return null;
            }

            return new AuthorPublication
            {
                Author = MapAuthor(authorPublication.Author),
                AuthorId = authorPublication.AuthorId,
                Id = authorPublication.Id,
                PublicationId = authorPublication.PublicationId
            };
        }
    }
}