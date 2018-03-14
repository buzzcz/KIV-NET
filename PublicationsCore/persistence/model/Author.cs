using System.ComponentModel.DataAnnotations;

namespace PublicationsCore.Persistence.Model
{
    /// <summary>
    /// Entity representing author of a publication.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Id of the author.
        /// </summary>
        [Key] [Required] public int Id { get; set; }

        /// <summary>
        /// Author's first name.
        /// </summary>
        [Required] public string FirstName { get; set; }

        /// <summary>
        /// Author's last name.
        /// </summary>
        [Required] public string LastName { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}";
        }

        protected bool Equals(Author other)
        {
            return Id == other.Id && string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Author) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}