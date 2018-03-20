using System.Collections.Generic;
using PublicationsCore.Facade.Dto;
using Xunit;

namespace TestProject.Facade.Dto
{
    public class PublicationDtoTest
    {
        [Fact]
        public void TestDtoProperties()
        {
            PublicationDto publication = new PublicationDto
            {
                Isbn = "8930723480239793",
                Title = "Name"
            };
            
            Assert.Equal("8930723480239793", publication.Isbn);
            Assert.Equal("Name", publication.Title);

            AuthorPublicationDto a = new AuthorPublicationDto
            {
                Author = new AuthorDto
                {
                    FirstName = "Douglas",
                    LastName = "Adams"
                }
            };
            publication.AuthorPublicationList = new List<AuthorPublicationDto>
            {
                a
            }; 
            
            Assert.Equal(a, publication.AuthorPublicationList[0]);
        }
    }
}