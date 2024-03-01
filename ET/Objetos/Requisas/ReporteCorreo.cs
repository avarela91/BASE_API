using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos
{
    public class ContextReporteCorreo : models<ReporteCorreo>
    {
        public ContextReporteCorreo() : base("IdCorreo", "ReporteCorreo", "Compras")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
        public List<ReporteCorreo> ReporteValidacion(string CodigoEstado)//tiene la funcion de pantalla Registro...
        {
            string query = "RQ_RequisasValidacion '" + @CodigoEstado + "'";
            return SelectRaw<ReporteCorreo>(query);
        }

    }
    [Serializable]
    public class ReporteCorreo : ICloneable
    {
        [Key]
        public int IdCorreo { get; set; }
        public string UserName { get; set; }
        public DateTime Create_Date { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Activo { get; set; }
        public int IdRequisa { get; set; }
        public int IdSolicitudCorreo { get; set; }
        public bool Procesado { get; set; }

        public object Clone()
        {
            return Utiles.Copia(this);
        }







    }
}
