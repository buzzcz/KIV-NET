using System;
using System.ComponentModel.DataAnnotations;
using PublicationsCore.Facade.Enums;

namespace PublicationsCore.Facade.Dto
{
    /// <summary>
    /// Class representing a publication.
    /// </summary>
    public class PublicationDto
    {
        /// <summary>
        /// Id of the publication in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ISBN number of the publication.
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// Title of the publication.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Author of the publication.
        /// </summary>
        public AuthorDto Author { get; set; }

        /// <summary>
        /// Date of publishing.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Type of publication.
        /// </summary>
        public PublicationType Type { get; set; }

        /// <summary>
        /// Publisher of the publication.
        /// </summary>
        public PublisherDto Publisher { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Isbn)}: {Isbn}, {nameof(Title)}: {Title}, {nameof(Author)}: {Author}, {nameof(Date)}: {Date}, {nameof(Type)}: {Type}, {nameof(Publisher)}: {Publisher}";
        }

        protected bool Equals(PublicationDto other)
        {
            return string.Equals(Isbn, other.Isbn) && string.Equals(Title, other.Title) &&
                   Equals(Author, other.Author) && Date.Equals(other.Date) && Type == other.Type &&
                   Equals(Publisher, other.Publisher);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PublicationDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Isbn != null ? Isbn.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Type;
                hashCode = (hashCode * 397) ^ (Publisher != null ? Publisher.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}