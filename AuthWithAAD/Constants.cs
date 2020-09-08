namespace AuthWithAAD
{
    public static class Constants
    {
        public static string RedirectUri = "msalf365af90-d6ee-4959-b8ac-667ccac6739a://auth";
        public static string ClientId = "f365af90-d6ee-4959-b8ac-667ccac6739a";
        public static string TenantId = "b55f0c51-61a7-45c3-84df-33569b247796";
        public static string iOSKeyChainSecurityGroup = "com.microsoft.aadauthentication";
        public static string[] Scopes = new string[] { "user.read", "people.read" };
    }
}
