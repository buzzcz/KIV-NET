using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PublicationsCore.Facade.Enums;

namespace PublicationsCore.Persistence.Model
{
    /// <summary>
    /// Entity representing a publication.
    /// </summary>
    public class Publication
    {
        /// <summary>
        /// Id of the publication.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// ISBN number of the publication.
        /// </summary>
        [Required]
        public string Isbn { get; set; }

        /// <summary>
        /// Title of the publication.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Date of publishing.
        /// </summary>
        [Required, Column(TypeName = "datetime(3)")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Type of publication.
        /// </summary>
        [Required]
        public PublicationType Type { get; set; }

        /// <summary>
        /// Publisher of the publication.
        /// </summary>
        [Required]
        public Publisher Publisher { get; set; }

        /// <summary>
        /// List of author-publications that contains information about the authors of the publication.
        /// </summary>
        [Required]
        public IList<AuthorPublication> AuthorPublicationList { get; set; }

        private string AuthorPublicationListToString()
        {
            return AuthorPublicationList != null
                ? AuthorPublicationList.Aggregate("", (current, author) => current + $"{author}; ")
                : "";
        }

        private bool AuthorPublicationListEquals(Publication other)
        {
            if (AuthorPublicationList != null && other.AuthorPublicationList != null)
            {
                return AuthorPublicationList.Count == other.AuthorPublicationList.Count &&
                       AuthorPublicationList.All(other.AuthorPublicationList.Contains);
            }

            return AuthorPublicationList == null && other.AuthorPublicationList == null;
        }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Isbn)}: {Isbn}, {nameof(Title)}: {Title}, {nameof(Date)}: {Date}, {nameof(Type)}: {Type}, {nameof(Publisher)}: {Publisher}, {nameof(AuthorPublicationList)}: [{AuthorPublicationListToString()}]";
        }

        protected bool Equals(Publication other)
        {
            return Id == other.Id && string.Equals(Isbn, other.Isbn) && string.Equals(Title, other.Title) &&
                   Date.Equals(other.Date) && Type == other.Type && Equals(Publisher, other.Publisher) &&
                   AuthorPublicationListEquals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Publication) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Isbn != null ? Isbn.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Type;
                hashCode = (hashCode * 397) ^ (Publisher != null ? Publisher.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AuthorPublicationList != null ? AuthorPublicationList.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}