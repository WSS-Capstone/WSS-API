namespace WSS.API.Infrastructure.Config;

public static class RoleName
{
    public static readonly string ADMIN = "Admin";
    public static readonly string OWNER = "Owner";
    public static readonly string STAFF = "Staff";
    public static readonly string PARTNER = "Partner";
    public static readonly string CUSTOMER = "Customer";
}

public enum RoleEnum
{
    Admin,
    Owner,
    Staff,
    Partner,
    Customer
}