using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        /// Title of the publication.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Author of the publication.
        /// </summary>
        public IList<AuthorPublicationDto> AuthorPublicationList { get; set; }

        /// <summary>
        /// Date of publishing.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Publisher of the publication.
        /// </summary>
        public PublisherDto Publisher { get; set; }

        /// <summary>
        /// Edition of the book.
        /// </summary>
        [Required]
        public string Edition { get; set; }
        
        // TODO: Klaus - It should also be possible to upload many files to given publication.

        private string AuthorPublicationListToString()
        {
            return AuthorPublicationList != null
                ? AuthorPublicationList.Aggregate("",
                    (current, authorPublication) => current + $"{authorPublication}; ")
                : "";
        }

        private bool AuthorPublicationListEquals(PublicationDto other)
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
                $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(AuthorPublicationList)}: [{AuthorPublicationListToString()}], {nameof(Date)}: {Date}, {nameof(Publisher)}: {Publisher}";
        }

        protected bool Equals(PublicationDto other)
        {
            return string.Equals(Title, other.Title) && AuthorPublicationListEquals(other) && Date.Equals(other.Date) &&
                   Equals(Publisher, other.Publisher) && string.Equals(Edition, other.Edition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PublicationDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AuthorPublicationList != null ? AuthorPublicationList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ (Publisher != null ? Publisher.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Edition != null ? Edition.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}