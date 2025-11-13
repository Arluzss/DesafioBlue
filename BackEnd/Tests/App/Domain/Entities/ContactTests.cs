using App.Domain.Entities;
using FluentValidation;
//using Xunit;

namespace Tests.App.Domain.Entities
{
    public class ContactTests
    {
        [Fact]
        public void Should_Create_Contact_With_Valid_Data()
        {
            var name = "Arley Silva";
            var email = "arley@gmail.com";
            var phone = "(81) 90000-0000";

            var contact = new Contact
            {
                Name = name,
                Email = email,
                Phone = phone
            };

            Assert.Equal(name, contact.Name);
            Assert.Equal(email, contact.Email);
            Assert.Equal(phone, contact.Phone);
            Assert.NotEqual(Guid.Empty, contact.Id);
        }
    }
}
