using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.ComponentModel.DataAnnotations;
namespace ET.Objetos
{
    public class ContextLogItemsRequisas : models<LogTrasladosItemsRequisas>
    {
        public ContextLogItemsRequisas() : base("IdLog", "LogTrasladosItemsRequisas", "Compras")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
    }
    public class LogTrasladosItemsRequisas
    {
        [Key]
        public int IdLog { get; set; }
        public int IdRequisa { get; set; }
        public string ItemCode{ get; set; }
        public decimal CantidadRequisada { get; set; }
        public decimal CantidadEntregada { get; set; }
        public decimal Saldo { get; set; }
        public string User_Create { get; set; }
        public DateTime Create_date { get; set; }
        public DateTime Modify_date { get; set; }
        public bool Activo { get; set; }
        public string NumAtCardRB { get; set; }
        public string UnitMedid { get; set; }
        public bool EntregadoCompleto { get; set; }
        public string ItemName { get; set; }
    }

    public class InformeTraslados
    {
        public List<Requisas> Requisas { get; set; }
        public List<LogEstadosRequisas> EstadosRequisas { get; set; } 
        public List<LogTrasladosItemsRequisas> LogTraslados{ get; set; } 
    }

}
