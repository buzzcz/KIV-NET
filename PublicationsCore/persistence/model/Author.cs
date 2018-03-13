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

        protected bool Equals(Author other)
        {
            return string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName);
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
                return ((FirstName != null ? FirstName.GetHashCode() : 0) * 397) ^
                       (LastName != null ? LastName.GetHashCode() : 0);
            }
        }
    }
}