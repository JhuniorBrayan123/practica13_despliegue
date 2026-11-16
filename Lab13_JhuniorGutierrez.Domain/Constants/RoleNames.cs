namespace Lab13_JhuniorGutierrez.Domain.Constants;

public static class RoleNames
{
    public const string Admin = "Admin";
    public const string Support = "Support";
    public const string User = "User";

    public const string Staff = Admin + "," + Support;

    public static bool IsBuiltIn(string roleName) => roleName is Admin or Support or User;
}
