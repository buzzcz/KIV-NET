namespace PublicationsCore.Facade.Dto
{
    public class ArticleDto : PublicationDto
    {
        /// <summary>
        /// DOI number of the article
        /// </summary>
        public string Doi { get; set; }
        
        /// <summary>
        /// Range of pages containing the citation.
        /// </summary>
        public string Pages { get; set; }
        
        /// <summary>
        /// ISSN number of the magazine.
        /// </summary>
        public string Issn { get; set; }
        
        /// <summary>
        /// Title of the magazine.
        /// </summary>
        public string MagazineTitle { get; set; }
        
        /// <summary>
        /// Volume of the magazine.
        /// </summary>
        public int Volume { get; set; }
    }
}