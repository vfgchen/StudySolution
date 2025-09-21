namespace WebApiProject.Authority
{
    public static class AppRepository
    {
        private static List<Application> list = new List<Application>()
        {
            new Application
            {
                ApplicationId = 1,
                ApplicationName = "WebProject",
                ClientId = "70D20BDE-5DCF-4140-BEE2-66A5FDD08FA9",
                Secret  = "2D4B9F54-CE87-447A-84E0-51D39EBE3C6D"
            }
        };

        public static bool Authenticate(string clientId, string secret)
        {
            return list.Any(x => x.ClientId == clientId && x.Secret == secret);
        }

        public static Application? GetApplicationByClientId(string clientId)
        {
            return list.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
