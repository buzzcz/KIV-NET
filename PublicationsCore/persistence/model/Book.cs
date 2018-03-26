using System.ComponentModel.DataAnnotations;

namespace PublicationsCore.Persistence.Model
{
    public class Book : Publication
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

        protected bool Equals(Book other)
        {
            return base.Equals(other) && string.Equals(Isbn, other.Isbn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Book) obj);
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