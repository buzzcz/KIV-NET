using PublicationsCore.Persistence;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Service
{
    public interface IPublicationService
    {
        /// <summary>
        /// Deletes old entities from old publication (author-publications, authors and publisher)
        /// if they differ from the new one or if the new one is null.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="oldPublication">Old publication from which to delete entities.</param>
        /// <param name="publication">New publication with changed entities. Defaults to null.</param>
        void DeleteOldPublicationSubentities(PublicationsContext db, Publication oldPublication,
            Publication publication = null);

        /// <summary>
        /// Deletes old publisher from old publication if they differ from the new one or if the new one is null.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="oldPublication">Old publication from which to delete entities.</param>
        /// <param name="publication">New publication with changed entities. Defaults to null.</param>
        void DeleteOldPublisher(PublicationsContext db, Publication oldPublication,
            Publication publication = null);

        /// <summary>
        /// Deletes old author-publications and authors from old publication if they differ from the new one or if the
        /// new one is null.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="oldPublication">Old publication from which to delete entities.</param>
        /// <param name="publication">New publication with changed entities. Defaults to null.</param>
        void DeleteOldAuthors(PublicationsContext db, Publication oldPublication,
            Publication publication = null);

        /// <summary>
        /// Checks if author from author-publication list already exists in the database and if so, this author is used
        /// instead of creating new one.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="publication">Publication where to check the authors.</param>
        void CheckAlreadyExistingAuthor(PublicationsContext db, Publication publication);

        /// <summary>
        /// Checks if publisher from publication already exists in the database and if so, this publisher is used
        /// instead of creating new one.
        /// </summary>
        /// <param name="db">Database context to use.</param>
        /// <param name="publication">Publication where to check the publisher.</param>
        void CheckAlreadyExistingPublisher(PublicationsContext db, Publication publication);
    }
}