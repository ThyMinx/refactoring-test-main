using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public sealed class ReferralsService
    {
        private IEnumerable<Location> _locations;
        private IReferralsRepository _referralsRepository;

        private const string YoungPeopleServiceName = "Young People";
        private const int YoungPeopleServiceMinAge = 16;
        private const int YoungPeopleServiceMaxAge = 21;
        private const int OtherServiceAge = 18;

        //I have to do it this way as I can't add dependency injection to program.cs, this allows for testing.
        public ReferralsService(IEnumerable<Location> locations, IReferralsRepository referralsRepository)
        {
            _locations = locations;
            _referralsRepository = referralsRepository;
        }

        public ReferralsService()
        {
            _locations = new List<Location> {
                    new Location("County Durham", Region.NorthEast),
                    new Location("Northumbria", Region.NorthEast),
                    new Location("North Yorkshire", Region.NorthEast),
                    new Location("Cumbria", Region.NorthWest),
                    new Location("Lancashire", Region.NorthWest),
                    new Location("Cheshire", Region.NorthWest),
            };

            _referralsRepository = new ReferralsRepository();
        }

        public bool AddReferral(string firstname, string lastname, DateTime dateOfBirth, string serviceName, string location)
        {
            if (!IsValidAge(dateOfBirth, serviceName))
            {
                return false;
            }

            var referral = new Referral();

            referral.Firstname = firstname;
            referral.Lastname = lastname;
            referral.DateOfBirth = dateOfBirth;

            referral.Service = _referralsRepository.GetService(serviceName);

            referral.Region = GetRegion(location);

            if (!IsValidReferral(referral))
            {
                return false;
            }

            _referralsRepository.CreateReferral(referral);

            return true;
        }

        private bool IsValidReferral(Referral referral)
        {
            var context = new ValidationContext(referral, serviceProvider: null, items: null);
            var errorResults = new List<ValidationResult>();

            return Validator.TryValidateObject(referral, context, errorResults);
        }

        private Region GetRegion(string location)
        {
            var region = _locations.FirstOrDefault(l => l.Name == location)?.Region;

            return region.GetValueOrDefault(Region.Other);
        }

        private bool IsValidAge(DateTime dateOfBirth, string serviceName)
        {
            var age = CalculateAge(dateOfBirth);

            if (serviceName.EndsWith(YoungPeopleServiceName) && (age < YoungPeopleServiceMinAge || age > YoungPeopleServiceMaxAge))
            {
                return false;
            }
            else if (age < OtherServiceAge)
            {
                return false;
            }

            return true;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var currentDate = DateTime.Now;
            var age = currentDate.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > currentDate.AddYears(-age))
            {
                return age - 1;
            }

            return age;
        }
    }
}
