using System.ComponentModel.DataAnnotations;

namespace PublicationsCore.Persistence.Model
{
    public class AuthorPublication
    {
        [Key] [Required] public int Id { get; set; }

        [Required] public int AuthorId { get; set; }

        [Required] public int PublicationId { get; set; }

        public Author Author { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(AuthorId)}: {AuthorId}, {nameof(PublicationId)}: {PublicationId}, {nameof(Author)}: {Author}";
        }

        protected bool Equals(AuthorPublication other)
        {
            return Id == other.Id && AuthorId == other.AuthorId && PublicationId == other.PublicationId &&
                   Equals(Author, other.Author);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AuthorPublication) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ AuthorId;
                hashCode = (hashCode * 397) ^ PublicationId;
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}