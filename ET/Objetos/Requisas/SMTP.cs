using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos
{
    public class ContextSMTP : models<SMTP>
    {
        public ContextSMTP() : base("idSMTP", "SMTP", "Compras")
        {

            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
        }
    [Serializable]
    public class SMTP : ICloneable
    {

        public int idSMTP { get; set; }
        public string host { get; set; }
        public string port { get; set; }
        public string Correo { get; set; }
        public string pass { get; set; }
        public DateTime Create_Date { get; set; }
        public DateTime Modify_Date { get; set; }
        public string UserName { get; set; }
        public bool Activo { get; set; }
        public string Nombre { get; set; }
        public bool Enablessl { get; set; }

        public object Clone()
        {
            return Utiles.Copia(this);
        }
    }
}