namespace PublicationsCore.Facade.Dto
{
    /// <summary>
    /// Class representing a publisher.
    /// </summary>
    public class PublisherDto
    {
        /// <summary>
        /// Id of the publisher in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the publisher.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address of the publisher.
        /// </summary>
        public AddressDto Address { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Address)}: {Address}";
        }

        protected bool Equals(PublisherDto other)
        {
            return string.Equals(Name, other.Name) && Equals(Address, other.Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PublisherDto) obj);
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