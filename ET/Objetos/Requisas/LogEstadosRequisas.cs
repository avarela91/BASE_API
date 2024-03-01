using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos
{
    public class ContextLogEstadosRequisas: models<LogEstadosRequisas>
    {
        public ContextLogEstadosRequisas():base("idLogEstadoRequisa", "LogEstadosRequisas", "Compras")
        {
            ActiveRecordsInTransactionOnly = true;
            ActivateActiveRecordsOnly = true;
        }
    }
    public class LogEstadosRequisas
    {
        public int IdLogEstadoRequisa { get; set; }
        public int IdRequisa { get; set; }
        public string ValorViejo { get; set; }
        public string ValorNuevo { get; set; }
        public string Create_User { get; set; }
        public string EstadoInicial { get; set; }
        public string EstadoFinal { get; set; }
        public bool Activo { get; set; }
        public DateTime Create_date { get; set; }
    }
}
