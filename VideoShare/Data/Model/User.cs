using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class User
    {
        [SQLColumn(0, "ID", true)]
        public int ID { get; set; }

        [SQLColumn(1, "Username")]
        public string Username { get; set; }

        [SQLColumn(2, "Email")]
        public string Email { get; set; }

        [SQLColumn(3, "PasswordHash")]
        public string PasswordHash { get; set; }

        [SQLColumn(4, "DisplayName")]
        public string DisplayName { get; set; }

        [SQLColumn(5, "RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [SQLColumn(6, "Info")]
        public string Info { get; set; }
    }
}