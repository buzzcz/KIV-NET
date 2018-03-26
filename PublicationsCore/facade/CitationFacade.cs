using Microsoft.Extensions.Logging;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Service;

namespace PublicationsCore.facade
{
    public class CitationFacade : ICitationFacade
    {
        private readonly ILogger _logger;

        private readonly ICitationService _citationService;

        /// <summary>
        /// Constructor for creating CitationFacade.
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="citationService">Service handling creation of citaitons and HTML descriptions of
        /// publications.</param>
        public CitationFacade(ILogger logger, ICitationService citationService)
        {
            _logger = logger;
            _citationService = citationService;
        }

        public string GetCitation(PublicationDto publication)
        {
            _logger.LogInformation($"Getting citation for: {publication}.");
            string citation = null;
            if (publication is BookDto book)
            {
                citation = _citationService.GetBookCitation(book);
            }

            _logger.LogInformation($"Got citation: {citation}.");

            return citation;
        }

        public string GetHtmlDescription(PublicationDto publication)
        {
            _logger.LogInformation($"Getting HTML description of: {publication}.");
            string html = null;
            if (publication is BookDto book)
            {
                html = _citationService.GetBookHtmlDescription(book);
            }

            _logger.LogInformation($"Got HTML description: {html}.");

            return html;
        }
    }
}