using System;

namespace Auto_Parts_Store.Models
{
    public class Users
    {
        public int UserID { get; set; } 
        public string CurrentUserName { get; set; } 
        public string FullName { get; set; }       
        public string CurrentUserRole { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
