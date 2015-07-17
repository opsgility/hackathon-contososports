namespace Contoso.Models
{
    public static class StaticConfig
    {
        public static class DbContext
        {
            public const string WebConnectionStringName = "name=DefaultConnectionString";
            public const string MobileServiceConnectionStringName = "Name=MS_TableConnectionString";
        }
    }
}
