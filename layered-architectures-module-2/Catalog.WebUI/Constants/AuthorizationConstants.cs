namespace Catalog.WebUI.Constants;

public static class AuthorizationConstants
{
    public static class Roles
    {
        public const string Manager = "manager";
        public const string Buyer = "buyer";
    }
    
    public static class Permissions
    {
        public const string Create = "create";
        public const string Read = "read";
        public const string Update = "update";
        public const string Delete = "delete";
    }
    
    public static class Policies
    {
        public const string Create = "create-policy";
        public const string Read = "read-policy";
        public const string Update = "update-policy";
        public const string Delete = "delete-policy";
    }
}