using FluentAssertions;
using TestAppLibrary;
using Moq;
using System.Reflection;

namespace TestAppLibraryTests
{
    public class ReferralsServiceTests
    {
        private readonly ReferralsService _sut;
        private readonly Mock<IReferralsRepository> _referralsRepositoryMock = new Mock<IReferralsRepository>();
        private readonly IEnumerable<Location> _locations;

        public ReferralsServiceTests()
        {
            _referralsRepositoryMock.Setup(x => x.GetService(It.IsAny<string>())).Returns(new Service { Id = Guid.NewGuid(), Name = "Service Name" });
            _referralsRepositoryMock.Setup(x => x.CreateReferral(It.IsAny<Referral>()));

            _locations = new List<Location> {
                new Location("North Yorkshire", Region.NorthEast),
                new Location("Cumbria", Region.NorthWest),
            };
            _sut = new ReferralsService(_locations, _referralsRepositoryMock.Object);
        }

        [Theory]
        [MemberData(nameof(IsValidReferralData))]
        public void IsValidReferral_ShouldReturnExpected(string firstName, string lastName, DateTime dob, string serviceName, string location, bool expected)
        {
            //Arrange
            var region = InvokeGetRegion(location);
            var service = _referralsRepositoryMock.Object.GetService(serviceName);
            var referral = new Referral { Firstname = firstName, Lastname = lastName, DateOfBirth = dob, Service = service, Region = region };

            //Act
            var result = InvokeIsValidReferral(referral);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetRegionData))]
        public void GetRegion_ShouldReturnExpected_WhenReferralMissingRequiredField(string location, Region expected)
        {
            //Arrange

            //Act
            var result = InvokeGetRegion(location);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void AddRefferal_Returns_ExpectedResult(string firstName, string lastName, DateTime dob, string serviceName, string location, bool expectedResult)
        {
            //Arrange
            
            //Act
            var answer = _sut.AddReferral(firstName, lastName, dob, serviceName, location);

            //Assert
            answer.Should().Be(expectedResult);
        }

        public bool InvokeIsValidReferral(Referral referral)
        {
            MethodInfo method = _sut.GetType().GetMethod("IsValidReferral", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] prms = new object[1] { referral };
            return (bool)method.Invoke(_sut, prms);
        }

        public Region InvokeGetRegion(string location)
        {
            MethodInfo method = _sut.GetType().GetMethod("GetRegion", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] prms = new object[1] { location };
            return (Region)method.Invoke(_sut, prms);
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

        public static IEnumerable<object[]> GetRegionData =>
            new List<object[]>
            {
                new object[] { "County Durham", Region.Other},
                new object[] { "Cumbria", Region.NorthWest },
                new object[] { "North Yorkshire", Region.NorthEast },
                new object[] { string.Empty, Region.Other },
            };

        public static IEnumerable<object[]> IsValidReferralData =>
            new List<object[]>
            {
                new object[] { "Joe", "Bloggs", new DateTime(2004, 2, 29), "Test Service", "County Durham", true},
                new object[] { "Sarah", "Robinson", new DateTime(1996, 6, 1), "Test Service", "County Durham", true },
                new object[] { "John", "", new DateTime(1978, 2, 28), "Test Service", "County Durham", false },
                new object[] { "", "Robinson", new DateTime(1996, 6, 1), "Test Service", "Cumbria", false },
            };
    }
}