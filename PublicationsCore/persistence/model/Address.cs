using System.ComponentModel.DataAnnotations;

namespace PublicationsCore.Persistence.Model
{
    /// <summary>
    /// Entity representing address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Id of the address.
        /// </summary>
        [Key] [Required] public int Id { get; set; }

        [Required] public string State { get; set; }

        [Required] public string City { get; set; }

        [Required] public string Street { get; set; }

        [Required] public int Number { get; set; }

        protected bool Equals(Address other)
        {
            return string.Equals(State, other.State) && string.Equals(City, other.City) &&
                   string.Equals(Street, other.Street) && Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (State != null ? State.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Number;
                return hashCode;
            }
        }
    }
}