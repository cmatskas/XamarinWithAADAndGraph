namespace AuthWithAAD
{
    public static class Constants
    {
        public static string RedirectUri = "msal6621f187-bb2c-4792-9d61-2fdf863afaab://auth";
        public static string ClientId = "6621f187-bb2c-4792-9d61-2fdf863afaab";
        public static string TenantId = "b55f0c51-61a7-45c3-84df-33569b247796";
        public static string iOSKeyChainSecurityGroup = "com.microsoft.aadauthentication";
        public static string[] Scopes = new string[] { "user.read", "people.read" };
    }
}
