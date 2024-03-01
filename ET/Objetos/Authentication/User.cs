using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DAL;

namespace ET
{
    public class ContextUser : models<User>
    {
        public ContextUser(): base("Id_User", "User","Security")
        {
            ActiveRecordsInTransactionOnly = true;
            ActivateActiveRecordsOnly = true;
        }
        //public List<PermissionUser> PermissionByUserAndModule(l loginModel)
        //{
        //    string query = "Select code, name from Permission where id_permission=1 ";
        //    return SelectRaw<PermissionUser>(query);
        //}
    }

   
    public class User
    {
        public int Id_User { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Active { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Nombre de usuario")]
        public string Modulo { get; set; }
    }

}
