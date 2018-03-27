using Microsoft.EntityFrameworkCore;
using PublicationsCore.Persistence.Model;

namespace PublicationsCore.Persistence
{
    /// <summary>
    /// Database context of the publications.
    /// </summary>
    public class PublicationsContext : DbContext
    {
        /// <summary>
        /// Set of authors in database.
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Set of publishers in database.
        /// </summary>
        public DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// Set of author-publications in database.
        /// </summary>
        public DbSet<AuthorPublication> AuthorPublications { get; set; }

        /// <summary>
        /// Set of publications in database.
        /// </summary>
        public DbSet<Publication> Publications { get; set; }
        
        public DbSet<Book> Books { get; set; }
        
        public DbSet<Article> Articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=dotnet;user=root;password=");
        }
    }
}