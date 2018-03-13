namespace PublicationsCore.Facade.Dto
{
    /// <summary>
    /// Class representing address.
    /// </summary>
    public class AddressDto
    {
        /// <summary>
        /// Id of the address in database.
        /// </summary>
        public int Id { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int Number { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(State)}: {State}, {nameof(City)}: {City}, {nameof(Street)}: {Street}, {nameof(Number)}: {Number}";
        }

        protected bool Equals(AddressDto other)
        {
            return string.Equals(State, other.State) && string.Equals(City, other.City) &&
                   string.Equals(Street, other.Street) && Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AddressDto) obj);
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