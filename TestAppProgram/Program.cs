using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppLibrary;

namespace TestAppProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var referralService = new ReferralsService();

            var result = referralService.AddReferral("Sarah", "Robinson", new DateTime(1996, 6, 1), "Test Service", "County Durham");

            if (result)
            {
                Console.WriteLine("Created referral");
            }
            else
            {
                Console.WriteLine("Referral wsa not created");
            }

        }
    }
}
