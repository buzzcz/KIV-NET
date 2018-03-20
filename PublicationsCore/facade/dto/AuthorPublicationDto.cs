namespace PublicationsCore.Facade.Dto
{
    public class AuthorPublicationDto
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public int PublicationId { get; set; }

        public AuthorDto Author { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Author)}: {Author}";
        }

        protected bool Equals(AuthorPublicationDto other)
        {
            return Equals(Author, other.Author);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AuthorPublicationDto) obj);
        }

        public override int GetHashCode()
        {
            return (Author != null ? Author.GetHashCode() : 0);
        }
    }
}