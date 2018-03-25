using System;
using PublicationsCore.Facade.Dto;
using PublicationsCore.Service;
using TestProject.Utils;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Service
{
    public class CitationServiceTest
    {
        private readonly ITestOutputHelper _output;

        private readonly ICitationService _citationService;

        public CitationServiceTest(ITestOutputHelper output)
        {
            _output = output;
            _citationService = new CitationService();
        }

        [Fact]
        public void TestBookCitation()
        {
            string expected =
                $"ADAMS, Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, {DateTime.Today:yyyy}. ISBN 7892347-913-2341-09.";
            PublicationDto publicationDto = TestUtils.CreatePublication();
            _output.WriteLine($"Getting citation for {publicationDto} in BOOK CITATION test.");
            string citation = _citationService.GetCitation(publicationDto);
            _output.WriteLine($"Got citation {citation} in BOOK CITATION test.");

            Assert.Equal(expected, citation);
        }
    }
}