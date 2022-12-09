namespace TestAppLibrary
{
    public interface IReferralsRepository
    {
        Service GetService(string serviceName);
        void CreateReferral(Referral referral);
    }
}