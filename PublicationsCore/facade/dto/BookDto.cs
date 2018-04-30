using System.ComponentModel.DataAnnotations;

namespace PublicationsCore.Facade.Dto
{
    public class BookDto : PublicationDto
    {
        /// <summary>
        /// ISBN number of the publication.
        /// </summary>
        [Required]
        public string Isbn { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Isbn)}: {Isbn}";
        }

        protected bool Equals(BookDto other)
        {
            return base.Equals(other) && string.Equals(Isbn, other.Isbn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BookDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Isbn != null ? Isbn.GetHashCode() : 0);
            }
        }
    }
}