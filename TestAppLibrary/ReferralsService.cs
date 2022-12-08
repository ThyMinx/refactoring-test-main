using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public class ReferralsService
    {
        public bool AddReferral(string firstname, string lastname, DateTime dateOfBirth, string serviceName, string location) {

            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname))
            {
                return false;
            }

            var currentDate = DateTime.Now;
            var age = currentDate.Year - dateOfBirth.Year;
            if (currentDate.Month < dateOfBirth.Month)
            {
                age = age - 1;
            }
            else if (currentDate.Month == dateOfBirth.Month && currentDate.Day < dateOfBirth.Day)
            {
                age = age - 1;
            }

            if (serviceName.EndsWith("Young People"))
            {
                if (age < 16 || age > 21)
                {
                    return false;
                }
            }
            else
            {
                if (age < 18)
                {
                    return false;
                }
            }

            var referral = new Referral();

            referral.Firstname = firstname;
            referral.Lastname = lastname;
            referral.DateOfBirth = dateOfBirth;

            var serviceDataAccess = new ServiceDataAccess(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            var service = serviceDataAccess.GetService(serviceName);

            if (service == null)
            {
                return false;
            }

            referral.Service = service;

            if (location == "County Durham" || location == "Northumbria" || location == "North Yorkshire")
            {
                referral.Region = Region.NorthEast;
            }
            else if (location == "Cumbria" || location == "Lancashire" || location == "Cheshire")
            {
                referral.Region = Region.NorthWest;
            }
            else
            {
                referral.Region = Region.Other;
            }

            ReferralDataAccess.CreateReferral(referral);

            return true;
        } 
    }
}
