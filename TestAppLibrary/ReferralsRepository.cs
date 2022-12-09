using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public class ReferralsRepository : IReferralsRepository
    {
        private readonly ServiceDataAccess _serviceDataAccess;

        public ReferralsRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            _serviceDataAccess = new ServiceDataAccess(connectionString);
        }

        public Service GetService(string serviceName) =>
            _serviceDataAccess.GetService(serviceName);

        public void CreateReferral(Referral referral) =>
            ReferralDataAccess.CreateReferral(referral);
    }
}
