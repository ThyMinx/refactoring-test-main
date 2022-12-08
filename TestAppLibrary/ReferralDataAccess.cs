using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public static class ReferralDataAccess
    {

        public static void CreateReferral(Referral referral) {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Referral_Create";
                    cmd.Parameters.AddWithValue("@firstname", referral.Firstname);
                    cmd.Parameters.AddWithValue("@lastname", referral.Lastname);
                    cmd.Parameters.AddWithValue("@dateOfBirth", referral.DateOfBirth);
                    cmd.Parameters.AddWithValue("@serviceId", referral.Service.Id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
