using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_TPL.Models
{
    public class User
    {
        public int Id { get; set; }

        public String UserName { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public int No_login_fail { get; set; }
        public int No_lock_Acount { get; set; }
        public String Madonvi { get; set; }
        public String Manhanvien { get; set; }
    }

}