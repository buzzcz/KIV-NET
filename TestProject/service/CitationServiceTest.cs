using System;
using System.Collections.Generic;
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

        [Theory]
        [InlineData("Adams", null, "Hitchhiker's Guide to the Galaxy", "1st Edition", "Pilsen", "University Press",
            "1990", "7892347-913-2341-09",
            "ADAMS. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData(null, "Douglas", "Hitchhiker's Guide to the Galaxy", "1st Edition", "Pilsen", "University Press",
            "1990", "7892347-913-2341-09",
            "Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData(null, null, "Hitchhiker's Guide to the Galaxy", "1st Edition", "Pilsen", "University Press",
            "1990", "7892347-913-2341-09",
            "Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Hitchhiker's Guide to the Galaxy", "1st Edition", "Pilsen", "University Press",
            "1990", "7892347-913-2341-09",
            "ADAMS, Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        public void Test_GetBookCitation_CorrectBookOneAuthor_CorrectBookCitation(string lastName, string name,
            string title,
            string edition,
            string address, string publisher, string date, string isbn, string expected)
        {
            BookDto bookDto = new BookDto
            {
                Date = new DateTime(int.Parse(date), 1, 1),
                Edition = edition,
                Isbn = isbn,
                Publisher = new PublisherDto
                {
                    Address = address,
                    Name = publisher
                },
                Title = title
            };

            if (lastName != null || name != null)
            {
                bookDto.AuthorPublicationList = new List<AuthorPublicationDto>
                {
                    new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name,
                            LastName = lastName
                        }
                    }
                };
            }

            _output.WriteLine($"Getting citation for {bookDto} in BOOK CITATION test.");
            string citation = _citationService.GetBookCitation(bookDto);
            _output.WriteLine($"Got citation {citation} in BOOK CITATION test.");

            Assert.Equal(expected, citation);
        }

        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "ADAMS a MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "Douglas a Steven. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "ADAMS, Douglas a MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "ADAMS, Douglas a Steven. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "ADAMS, Douglas a Steven MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        public void Test_GetBookCitation_CorrectBookTwoAuthors_CorrectBookCitation(string lastName, string name,
            string lastName2,
            string name2, string date, string expected)
        {
            BookDto bookDto = TestUtils.CreateBook();
            bookDto.Date = new DateTime(int.Parse(date), 1, 1);

            if (lastName != null || name != null)
            {
                bookDto.AuthorPublicationList = new List<AuthorPublicationDto>
                {
                    new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name,
                            LastName = lastName
                        }
                    }
                };
            }

            if (lastName2 != null || name2 != null)
            {
                bookDto.AuthorPublicationList.Add(new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name2,
                            LastName = lastName2
                        }
                    }
                );
            }

            _output.WriteLine($"Getting citation for {bookDto} in BOOK CITATION test.");
            string citation = _citationService.GetBookCitation(bookDto);
            _output.WriteLine($"Got citation {citation} in BOOK CITATION test.");

            Assert.Equal(expected, citation);
        }

        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "ADAMS, MOFFAT, ADAMS a MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "Douglas, Steven, Steven a Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "ADAMS, Douglas, MOFFAT, ADAMS a Douglas MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "ADAMS, Douglas, Steven, Steven ADAMS a Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "ADAMS, Douglas, Steven MOFFAT, Steven ADAMS a Douglas MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        public void Test_GetBookCitation_CorrectBookMoreAuthors_CorrectBookCitation(string lastName, string name,
            string lastName2,
            string name2, string date, string expected)
        {
            BookDto bookDto = TestUtils.CreateBook();
            bookDto.Date = new DateTime(int.Parse(date), 1, 1);

            if (lastName != null || name != null)
            {
                bookDto.AuthorPublicationList = new List<AuthorPublicationDto>
                {
                    new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name,
                            LastName = lastName
                        }
                    }
                };
            }

            if (lastName2 != null || name2 != null)
            {
                bookDto.AuthorPublicationList.Add(new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name2,
                            LastName = lastName2
                        }
                    }
                );
            }

            if (lastName != null || name2 != null)
            {
                bookDto.AuthorPublicationList.Add(new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name2,
                            LastName = lastName
                        }
                    }
                );
            }

            if (lastName2 != null || name != null)
            {
                bookDto.AuthorPublicationList.Add(new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name,
                            LastName = lastName2
                        }
                    }
                );
            }

            _output.WriteLine($"Getting citation for {bookDto} in BOOK CITATION test.");
            string citation = _citationService.GetBookCitation(bookDto);
            _output.WriteLine($"Got citation {citation} in BOOK CITATION test.");

            Assert.Equal(expected, citation);
        }
    }
}