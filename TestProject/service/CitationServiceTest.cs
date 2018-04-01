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
            "ADAMS. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. " +
            "ISBN 7892347-913-2341-09.")]
        [InlineData(null, "Douglas", "Hitchhiker's Guide to the Galaxy", "1st Edition", "Pilsen", "University Press",
            "1990", "7892347-913-2341-09",
            "Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. " +
            "ISBN 7892347-913-2341-09.")]
        [InlineData(null, null, "Hitchhiker's Guide to the Galaxy", "1st Edition", "Pilsen", "University Press",
            "1990", "7892347-913-2341-09",
            "Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Hitchhiker's Guide to the Galaxy", "1st Edition", "Pilsen", "University Press",
            "1990", "7892347-913-2341-09",
            "ADAMS, Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN " +
            "7892347-913-2341-09.")]
        public void Test_GetBookCitation_ValidBookOneAuthor_CorrectBookCitation(string lastName, string name,
            string title, string edition, string address, string publisher, string date, string isbn, string expected)
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
            "ADAMS a MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN " +
            "7892347-913-2341-09.")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "Douglas a Steven. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. ISBN " +
            "7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "ADAMS, Douglas a MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. " +
            "ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "ADAMS, Douglas a Steven. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, 1990. " +
            "ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "ADAMS, Douglas a Steven MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press," +
            " 1990. ISBN 7892347-913-2341-09.")]
        public void Test_GetBookCitation_ValidBookTwoAuthors_CorrectBookCitation(string lastName, string name,
            string lastName2, string name2, string date, string expected)
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

            _output.WriteLine($"Getting citation for {bookDto} in BOOK CITATION TWO AUTHORS test.");
            string citation = _citationService.GetBookCitation(bookDto);
            _output.WriteLine($"Got citation {citation} in BOOK CITATION TWO AUTHORS test.");

            Assert.Equal(expected, citation);
        }

        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "ADAMS, MOFFAT, ADAMS a MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University Press, " +
            "1990. ISBN 7892347-913-2341-09.")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "Douglas, Steven, Steven a Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: University " +
            "Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "ADAMS, Douglas, MOFFAT, ADAMS a Douglas MOFFAT. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: " +
            "University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "ADAMS, Douglas, Steven, Steven ADAMS a Douglas. Hitchhiker's Guide to the Galaxy. 1st Edition. Pilsen: " +
            "University Press, 1990. ISBN 7892347-913-2341-09.")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "ADAMS, Douglas, Steven MOFFAT, Steven ADAMS a Douglas MOFFAT. Hitchhiker's Guide to the Galaxy. 1st " +
            "Edition. Pilsen: University Press, 1990. ISBN 7892347-913-2341-09.")]
        public void Test_GetBookCitation_ValidBookMoreAuthors_CorrectBookCitation(string lastName, string name,
            string lastName2, string name2, string date, string expected)
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

            _output.WriteLine($"Getting citation for {bookDto} in BOOK CITATION MORE AUTHORS test.");
            string citation = _citationService.GetBookCitation(bookDto);
            _output.WriteLine($"Got citation {citation} in BOOK CITATION MORE AUTHORS test.");

            Assert.Equal(expected, citation);
        }

        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "ADAMS, MOFFAT, ADAMS a MOFFAT. Some article. MagazineTitle. Pilsen: University Press, 1990, 5.(10), 206-" +
            "208. DOI 789302571-231. ISSN 87032987-1342.")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "Douglas, Steven, Steven a Douglas. Some article. MagazineTitle. Pilsen: University Press, 1990, 5.(10)," +
            " 206-208. DOI 789302571-231. ISSN 87032987-1342.")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "ADAMS, Douglas, MOFFAT, ADAMS a Douglas MOFFAT. Some article. MagazineTitle. Pilsen: University Press, " +
            "1990, 5.(10), 206-208. DOI 789302571-231. ISSN 87032987-1342.")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "ADAMS, Douglas, Steven, Steven ADAMS a Douglas. Some article. MagazineTitle. Pilsen: University Press, " +
            "1990, 5.(10), 206-208. DOI 789302571-231. ISSN 87032987-1342.")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "ADAMS, Douglas, Steven MOFFAT, Steven ADAMS a Douglas MOFFAT. Some article. MagazineTitle. Pilsen: " +
            "University Press, 1990, 5.(10), 206-208. DOI 789302571-231. ISSN 87032987-1342.")]
        public void Test_GetArticleCitation_ValidArticleMoreAuthors_CorrectArticleCitation(string lastName, string name,
            string lastName2, string name2, string date, string expected)
        {
            ArticleDto articleDto = TestUtils.CreateArticle();
            articleDto.Date = new DateTime(int.Parse(date), 1, 1);

            if (lastName != null || name != null)
            {
                articleDto.AuthorPublicationList = new List<AuthorPublicationDto>
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name,
                            LastName = lastName2
                        }
                    }
                );
            }

            _output.WriteLine($"Getting citation for {articleDto} in ARTICLE CITATION MORE AUTHORS test.");
            string citation = _citationService.GetArticleCitation(articleDto);
            _output.WriteLine($"Got citation {citation} in ARTICLE CITATION MORE AUTHORS test.");

            Assert.Equal(expected, citation);
        }

        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "<span class='book'><span class='authors'>ADAMS, MOFFAT, ADAMS a MOFFAT</span>. <span class='title'>" +
            "Hitchhiker's Guide to the Galaxy</span>. <span class='edition'>1st Edition</span>. <span " +
            "class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>. ISBN <span class='isbn'>7892347-913-2341-09</span>.</span>")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "<span class='book'><span class='authors'>Douglas, Steven, Steven a Douglas</span>. <span class='title'>" +
            "Hitchhiker's Guide to the Galaxy</span>. <span class='edition'>1st Edition</span>. <span class='address'>" +
            "Pilsen</span>: <span class='publisher'>University Press</span>, <span class='publication-date'>1990</span>. " +
            "ISBN <span class='isbn'>7892347-913-2341-09</span>.</span>")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "<span class='book'><span class='authors'>ADAMS, Douglas, MOFFAT, ADAMS a Douglas MOFFAT</span>. <span " +
            "class='title'>Hitchhiker's Guide to the Galaxy</span>. <span class='edition'>1st Edition</span>. <span " +
            "class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>. ISBN <span class='isbn'>7892347-913-2341-09</span>.</span>")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "<span class='book'><span class='authors'>ADAMS, Douglas, Steven, Steven ADAMS a Douglas</span>. <span " +
            "class='title'>Hitchhiker's Guide to the Galaxy</span>. <span class='edition'>1st Edition</span>. <span " +
            "class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>. ISBN <span class='isbn'>7892347-913-2341-09</span>.</span>")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "<span class='book'><span class='authors'>ADAMS, Douglas, Steven MOFFAT, Steven ADAMS a Douglas MOFFAT" +
            "</span>. <span class='title'>Hitchhiker's Guide to the Galaxy</span>. <span class='edition'>1st Edition" +
            "</span>. <span class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>. ISBN <span class='isbn'>7892347-913-2341-09</span>.</span>")]
        public void Test_GetBookHtmlDescription_ValidBookMoreAuthors_CorrectBookHtmlDescription(string lastName,
            string name, string lastName2, string name2, string date, string expected)
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

            _output.WriteLine($"Getting HTML description for {bookDto} in BOOK HTML DESCRIPTION test.");
            string html = _citationService.GetBookHtmlDescription(bookDto);
            _output.WriteLine($"Got HTML description {html} in BOOK HTML DESCRIPTION test.");

            Assert.Equal(expected, html);
        }

        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "<span class='article'><span class='authors'>ADAMS, MOFFAT, ADAMS a MOFFAT</span>. <span class='title'>" +
                "Some article</span>. <span class='magazine-title'>MagazineTitle</span>. <span class='address'>Pilsen" +
            "</span>: <span class='publisher'>University Press</span>, <span class='publication-date'>1990</span>, " +
            "<span class='edition'>5.</span>(<span class='volume'>10</span>), <span class='pages'>206-208</span>. DOI " +
            "<span class='doi'>789302571-231</span>. ISSN <span class='issn'>87032987-1342</span>.</span>")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "<span class='article'><span class='authors'>Douglas, Steven, Steven a Douglas</span>. <span " +
                "class='title'>Some article</span>. <span class='magazine-title'>MagazineTitle</span>. <span " +
            "class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>, <span class='edition'>5.</span>(<span class='volume'>10</span>), " +
            "<span class='pages'>206-208</span>. DOI <span class='doi'>789302571-231</span>. ISSN <span class='issn'>" +
            "87032987-1342</span>.</span>")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "<span class='article'><span class='authors'>ADAMS, Douglas, MOFFAT, ADAMS a Douglas MOFFAT</span>. <span" +
            " class='title'>Some article</span>. <span class='magazine-title'>MagazineTitle</span>. <span " +
            "class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>, <span class='edition'>5.</span>(<span class='volume'>10</span>), " +
            "<span class='pages'>206-208</span>. DOI <span class='doi'>789302571-231</span>. ISSN <span class='issn'>" +
            "87032987-1342</span>.</span>")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "<span class='article'><span class='authors'>ADAMS, Douglas, Steven, Steven ADAMS a Douglas</span>. <span" +
            " class='title'>Some article</span>. <span class='magazine-title'>MagazineTitle</span>. <span " +
            "class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>, <span class='edition'>5.</span>(<span class='volume'>10</span>), " +
            "<span class='pages'>206-208</span>. DOI <span class='doi'>789302571-231</span>. ISSN <span class='issn'>" +
            "87032987-1342</span>.</span>")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "<span class='article'><span class='authors'>ADAMS, Douglas, Steven MOFFAT, Steven ADAMS a Douglas MOFFAT" +
            "</span>. <span class='title'>Some article</span>. <span class='magazine-title'>MagazineTitle</span>. " +
            "<span class='address'>Pilsen</span>: <span class='publisher'>University Press</span>, <span " +
            "class='publication-date'>1990</span>, <span class='edition'>5.</span>(<span class='volume'>10</span>), " +
            "<span class='pages'>206-208</span>. DOI <span class='doi'>789302571-231</span>. ISSN <span class='issn'>" +
            "87032987-1342</span>.</span>")]
        public void Test_GetArticleHtmlDescription_ValidArticleMoreAuthors_CorrectArticleDescription(string lastName,
            string name, string lastName2, string name2, string date, string expected)
        {
            ArticleDto articleDto = TestUtils.CreateArticle();
            articleDto.Date = new DateTime(int.Parse(date), 1, 1);

            if (lastName != null || name != null)
            {
                articleDto.AuthorPublicationList = new List<AuthorPublicationDto>
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name,
                            LastName = lastName2
                        }
                    }
                );
            }

            _output.WriteLine($"Getting HTML description for {articleDto} in ARTICLE HTML DESCRIPTION test.");
            string html = _citationService.GetArticleHtmlDescription(articleDto);
            _output.WriteLine($"Got HTML description {html} in ARTICLE HTML DESCRIPTION test.");

            Assert.Equal(expected, html);
        }
        
        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "@book{HitGuitthGal, author = 'Adams and Moffat and Adams and Moffat', title = 'Hitchhiker's Guide to the " +
            "Galaxy', publisher = 'University Press', address = 'Pilsen', edition = '1st Edition', year = '1990', " +
            "ISBN = '7892347-913-2341-09',}")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "@book{HitGuitthGal, author = 'Douglas and Steven and Steven and Douglas', title = 'Hitchhiker's Guide to the " +
            "Galaxy', publisher = 'University Press', address = 'Pilsen', edition = '1st Edition', year = '1990', " +
            "ISBN = '7892347-913-2341-09',}")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "@book{HitGuitthGal, author = 'Douglas Adams and Moffat and Adams and Douglas Moffat', title = 'Hitchhiker's " +
            "Guide to the Galaxy', publisher = 'University Press', address = 'Pilsen', edition = '1st Edition', year " +
            "= '1990', ISBN = '7892347-913-2341-09',}")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "@book{HitGuitthGal, author = 'Douglas Adams and Steven and Steven Adams and Douglas', title = 'Hitchhiker's " +
            "Guide to the Galaxy', publisher = 'University Press', address = 'Pilsen', edition = '1st Edition', year " +
            "= '1990', ISBN = '7892347-913-2341-09',}")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "@book{HitGuitthGal, author = 'Douglas Adams and Steven Moffat and Steven Adams and Douglas Moffat', title = " +
            "'Hitchhiker's Guide to the Galaxy', publisher = 'University Press', address = 'Pilsen', edition = '1st " +
            "Edition', year = '1990', ISBN = '7892347-913-2341-09',}")]
        public void Test_GetBookBibTex_ValidBookMoreAuthors_CorrectBookBibTex(string lastName,
            string name, string lastName2, string name2, string date, string expected)
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

            _output.WriteLine($"Getting BibTex for {bookDto} in BOOK BIBTEX test.");
            string bitex = _citationService.GetBookBibTex(bookDto);
            _output.WriteLine($"Got BibTex {bitex} in BOOK BIBTEX test.");

            Assert.Equal(expected, bitex);
        }

        [Theory]
        [InlineData("Adams", null, "Moffat", null, "1990",
            "@article{Somart, author = 'Adams and Moffat and Adams and Moffat', title = 'Some article', journal = " +
            "'MagazineTitle', number = '5.', volume = '10', pages = '206-208', year = '1990', address = 'Pilsen', " +
            "publisher = 'University Press', ISSN = '87032987-1342', DOI = '789302571-231', }")]
        [InlineData(null, "Douglas", null, "Steven", "1990",
            "@article{Somart, author = 'Douglas and Steven and Steven and Douglas', title = 'Some article', journal =" +
            " 'MagazineTitle', number = '5.', volume = '10', pages = '206-208', year = '1990', address = 'Pilsen', " +
            "publisher = 'University Press', ISSN = '87032987-1342', DOI = '789302571-231', }")]
        [InlineData("Adams", "Douglas", "Moffat", null, "1990",
            "@article{Somart, author = 'Douglas Adams and Moffat and Adams and Douglas Moffat', title = 'Some article" +
            "', journal = 'MagazineTitle', number = '5.', volume = '10', pages = '206-208', year = '1990', address = " +
            "'Pilsen', publisher = 'University Press', ISSN = '87032987-1342', DOI = '789302571-231', }")]
        [InlineData("Adams", "Douglas", null, "Steven", "1990",
            "@article{Somart, author = 'Douglas Adams and Steven and Steven Adams and Douglas', title = 'Some article" +
            "', journal = 'MagazineTitle', number = '5.', volume = '10', pages = '206-208', year = '1990', address = " +
            "'Pilsen', publisher = 'University Press', ISSN = '87032987-1342', DOI = '789302571-231', }")]
        [InlineData("Adams", "Douglas", "Moffat", "Steven", "1990",
            "@article{Somart, author = 'Douglas Adams and Steven Moffat and Steven Adams and Douglas Moffat', title =" +
            " 'Some article', journal = 'MagazineTitle', number = '5.', volume = '10', pages = '206-208', year = " +
            "'1990', address = 'Pilsen', publisher = 'University Press', ISSN = '87032987-1342', DOI = '789302571-231" +
            "', }")]
        public void Test_GetArticleBibTex_ValidArticleMoreAuthors_CorrectArticleBibTex(string lastName,
            string name, string lastName2, string name2, string date, string expected)
        {
            ArticleDto articleDto = TestUtils.CreateArticle();
            articleDto.Date = new DateTime(int.Parse(date), 1, 1);

            if (lastName != null || name != null)
            {
                articleDto.AuthorPublicationList = new List<AuthorPublicationDto>
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
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
                articleDto.AuthorPublicationList.Add(new AuthorPublicationDto
                    {
                        Author = new AuthorDto
                        {
                            FirstName = name,
                            LastName = lastName2
                        }
                    }
                );
            }

            _output.WriteLine($"Getting BibTex for {articleDto} in ARTICLE BIBTEX test.");
            string bitex = _citationService.GetArticleBibTex(articleDto);
            _output.WriteLine($"Got BibTex {bitex} in ARTICLE BIBTEX test.");

            Assert.Equal(expected, bitex);
        }
    }
}