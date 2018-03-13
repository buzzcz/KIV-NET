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
            return new PublicationDto
            {
                Author = MapAuthor(publication.Author),
                Date = publication.Date,
                Id = publication.Id,
                Isbn = publication.Isbn,
                Publisher = MapPublisher(publication.Publisher),
                Title = publication.Title,
                Type = publication.Type
            };
        }

        public Publication MapPublication(PublicationDto publication)
        {
            return new Publication
            {
                Author = MapAuthor(publication.Author),
                Date = publication.Date,
                Id = publication.Id,
                Isbn = publication.Isbn,
                Publisher = MapPublisher(publication.Publisher),
                Title = publication.Title,
                Type = publication.Type
            };
        }

        public AddressDto MapAddress(Address address)
        {
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
            return new AuthorDto
            {
                FirstName = author.FirstName,
                Id = author.Id,
                LastName = author.LastName
            };
        }

        public Author MapAuthor(AuthorDto author)
        {
            return new Author
            {
                FirstName = author.FirstName,
                Id = author.Id,
                LastName = author.LastName
            };
        }

        public PublisherDto MapPublisher(Publisher publisher)
        {
            return new PublisherDto
            {
                Address = MapAddress(publisher.Address),
                Id = publisher.Id,
                Name = publisher.Name
            };
        }

        public Publisher MapPublisher(PublisherDto publisher)
        {
            return new Publisher
            {
                Address = MapAddress(publisher.Address),
                Id = publisher.Id,
                Name = publisher.Name
            };
        }
    }
}