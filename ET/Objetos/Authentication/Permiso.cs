using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL;

namespace ET
{
    public class ContextPermiso : models<Permiso>
    {
        public ContextPermiso() : base("Permiso_Id", "Permiso", "Security") {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }

        public List<PermisoUser> PermissionByUserAndModule(string UserName, string Modulo)
        {
            string query = "PermissionByUserAndModulo @UserName='" + UserName + "', @CodigoModulo='" + Modulo + "' ";
            return SelectRaw<PermisoUser>(query);
        }
    }

    [Serializable()]
    public class Permiso : ICloneable
    {
        public int Permiso_Id { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Codigo { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Modulo")]
        public int Modulo_Id { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Activo { get; set; }

        public object Clone()
        {
            return Utiles.Copia(this);
        }
    }

    [Serializable()]
    public class PermisoUser : ICloneable
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return Utiles.Copia(this);
        }
    }
}
