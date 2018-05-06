using System.ComponentModel.DataAnnotations;

namespace PublicationsCore.Facade.Dto
{
    /// <summary>
    /// Class representing author of a publication.
    /// </summary>
    public class AuthorDto
    {
        /// <summary>
        /// Id of the author in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Author's first name.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Author's last name.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}";
        }

        protected bool Equals(AuthorDto other)
        {
            return string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AuthorDto) obj);
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