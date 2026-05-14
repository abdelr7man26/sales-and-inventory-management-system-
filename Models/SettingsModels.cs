namespace Auto_Parts_Store.Models
{
    public class StoreProfile
    {
        public string Name      { get; set; }
        public string Phone     { get; set; }
        public string Address   { get; set; }
        public string TaxNumber { get; set; }
    }

    public class UIPreferences
    {
        public string Theme       { get; set; } = "Light";
        public string PrintFormat { get; set; } = "A4";
    }

    public class SystemPermissionEntry
    {
        public int    PermissionID  { get; set; }
        public string PermissionKey { get; set; }
        public string DisplayName   { get; set; }
        public string Module        { get; set; }
        public bool   IsGranted     { get; set; }
    }

    public class SystemRole
    {
        public int    RoleID      { get; set; }
        public string RoleName    { get; set; }
        public string Description { get; set; }
        public bool   IsBuiltIn   { get; set; }
    }

    public class UserAdminEntry
    {
        public int    PersonID  { get; set; }
        public string UserName  { get; set; }
        public string FullName  { get; set; }
        public string Phone     { get; set; }
        public string Address   { get; set; }
        public string Role      { get; set; }
    }
}
