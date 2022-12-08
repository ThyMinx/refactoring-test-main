using FluentAssertions;
using TestAppLibrary;

namespace TestAppLibraryTests
{
    public class ReferralsServiceTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void AddRefferal_Returns_ExpectedResult(string firstName, string lastName, DateTime dob, string serviceName, string location, bool expectedResult)
        {
            //Arrange
            var referralsService = new ReferralsService();

            //Act
            var answer = referralsService.AddReferral(firstName, lastName, dob, serviceName, location);

            //Assert
            answer.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { "John", "", new DateTime(1978, 2, 28), "Test Service", "County Durham", false },
                new object[] { "Judy", "Smith", new DateTime(2000, 10, 15), "Test Service Young People", "London", false },
                new object[] { "", "Robinson", new DateTime(1996, 6, 1), "Test Service", "Cumbria", false },
                new object[] { "Peter", "Davids", new DateTime(2014, 6, 1), "Test Service", "North Yorkshire", false },
                new object[] { "Joe", "Bloggs", new DateTime(2004, 2, 29), "Test Service", "County Durham", true },
                new object[] { "Sarah", "Robinson", new DateTime(1996, 6, 1), "Test Service", "County Durham", true },
            };
    }
}