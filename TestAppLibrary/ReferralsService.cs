using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public class ReferralsService
    {
        private List<Location> _locations;

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

            var serviceDataAccess = new ServiceDataAccess(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            var service = serviceDataAccess.GetService(serviceName);

            referral.Service = service;

            referral.Region = GetRegion(location);

            var context = new ValidationContext(referral, serviceProvider: null, items: null);
            var errorResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(referral, context, errorResults);
            if (!isValid)
            {
                return false;
            }

            ReferralDataAccess.CreateReferral(referral);

            return true;
        }

        private Region GetRegion(string location)
        {
            var region = _locations.Find(l => l.Name == location)?.Region;

            return region.GetValueOrDefault(Region.Other);
        }

        private bool IsValidAge(DateTime dateOfBirth, string serviceName)
        {
            var age = CalculateAge(dateOfBirth);

            if (serviceName.EndsWith("Young People") && (age < 16 || age > 21))
            {
                return false;
            }
            else if (age < 18)
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
