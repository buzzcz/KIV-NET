using System.ComponentModel.DataAnnotations;

namespace PublicationsCore.Persistence.Model
{
    /// <summary>
    /// Entity representing a publisher.
    /// </summary>
    public class Publisher
    {
        /// <summary>
        /// Id of the publisher.
        /// </summary>
        [Key] [Required] public int Id { get; set; }

        /// <summary>
        /// Name of the publisher.
        /// </summary>
        [Required] public string Name { get; set; }

        /// <summary>
        /// Address of the publisher.
        /// </summary>
        [Required] public Address Address { get; set; }

        protected bool Equals(Publisher other)
        {
            return string.Equals(Name, other.Name) && Equals(Address, other.Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Publisher) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Address != null ? Address.GetHashCode() : 0);
            }
        }
    }
}