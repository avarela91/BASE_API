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
    public class ContextRequisitions : models<Requisas>
    {
        public ContextRequisitions() : base("IdRequisa", "Requisas", "Compras")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
        public List<ItemsRequisas> SelectGenericoItems(string IdRequisa)
        {
            string query = "RQ_DetallesRequisa'" + IdRequisa + "'";

            return SelectRaw<ItemsRequisas>(query);
        }
        public List<Requisas> RequisasId(int IdRequisa)
        {
            string query = "RQ_RequisasId'" + IdRequisa + "'";

            return SelectRaw<Requisas>(query);
        }
        public List<Requisas> RequisasAceptadas(string Estado, string FechaInicio, string FechaFin, string AlmacenCode)
        {
            string query = "RQ_RequisasAceptadas'" + Estado + "','" + FechaInicio + "','" + FechaFin + "','"+ AlmacenCode + "'";

            return SelectRaw<Requisas>(query);
        }
           
   
        public List<Requisas> HojaEntrega(string IdRequisa)
        {
            string query = "RQ_HojaEntrega'" + IdRequisa + "'";

            return SelectRaw<Requisas>(query);
        }

    }



    public class Requisas
    {
        [Key]
        public int IdRequisa { get; set; }
        public string CodigoEstado { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public string Almacen { get; set; }
        public string Motivo { get; set; }
        public string UserName { get; set; }
        public string UserModify { get; set; }
        public bool Activo { get; set; }
        public DateTime Create_Date { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Comentario { get; set; }
        public string AlmacenName { get; set; }
        public List<ItemsRequisas> ItemsRequisas { get; set; }

        public string Estado { get; set; }
        public string Departamento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin{ get; set; }
        public string ComentarioJefe { get; set; }
        public string Tienda { get; set; }
        public string Solicitante { get; set; }
        public string PropServicio { get; set; }
        public string ComentarioAuditoria { get; set; }
        public decimal DocTotal { get; set; }
        public string NumAtCardRB { get; set; }//para mostrar las hojas de carga de la Requisa
    }
    public class ContextItemsRequisitions : models<ItemsRequisas>
    {
        public ContextItemsRequisitions() : base("IdItem", "ItemsRequisas", "Compras")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
    }
    public class ItemsRequisas
    {
        [Key]
        public int IdItem { get; set; }
        public string ItemCode { get; set; }
        public string Nombre { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CantidadPendiente { get; set; }
        public decimal Costo { get; set; }
        public int IdRequisa { get; set; }
        public string UnitMed { get; set; }
        public string UserName { get; set; }
        public string User_Modify { get; set; }
        public DateTime Create_Date { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Activo { get; set; }
        public decimal CantidadEntregada { get; set; }
        public decimal Peso { get; set; }
        public string Currency { get; set; }
        public int UoMEntry { get; set; }
        public bool Entregado { get; set; }
        public string UseBaseUnits { get; set; }


    }
    public class ModelSend
    {
        public byte[] Reporte { get; set; }
        public string userName { get; set; }
        public int IdRequisa { get; set; }
        public List<string> CorreosAutorizados { get; set; }
    }
    public class EditarEP
    {
        public Requisas Requisas { get; set; }
        public string NumAtCard { get; set; }

        public string UserCreate { get; set; }
    }
    public class ModelCambioEstado
    {
        public int idRequisa { get; set; }
        public string CodigoEstado { get; set; }
        public string ComentarioJefe { get; set; }
        public string ComentarioAuditor { get; set; }
        public string UserModify { get; set; }
        public string TipoRequisa { get; set; }
        public string EstadoInicial { get; set; }
        public string ValorViejo { get; set; }
        public string ValorNuevo { get; set; }
    }
}
