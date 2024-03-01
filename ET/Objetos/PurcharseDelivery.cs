using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace ET.Objetos
{
    public class PurcharseDeliveryContext : models<ReceivedOrderModel4>
    {
        public PurcharseDeliveryContext() : base("DocEntry", "ODRF", "SAP")
        {

        }

        public List<ReceivedOrderModel4> GetEndDocEntryEMP()
        {
            //Obtener la ultima Entrada de mercancia preliminar
            string query = "SELECT DocEntry, DocNum, DocDate, CardCode, CardName, NumAtCard, DocStatus,'Borrador' as Tipo, Comments FROM ODRF WHERE CANCELED != 'Y' AND	ObjType = 20 ORDER BY DocEntry desc";
            return SelectRaw<ReceivedOrderModel4>(query);
        }

        public List<ReceivedOrderModel4> GetEMPreliminarByDocEntry(int DocEntry)
        {
            //Entrada de mercancia preliminar
            string query = "SELECT DocEntry, DocNum, DocDate, CardCode, CardName, NumAtCard, DocStatus,'Borrador' as Tipo, Comments FROM ODRF WHERE CANCELED != 'Y' AND	ObjType = 20 AND DocEntry = " + DocEntry;
            return SelectRaw<ReceivedOrderModel4>(query);
        }

        public List<Detail4> GetDetailtEMPreliminar(int DocEntry)
        {
            //Detalle Entrada de mercancia preliminar
            string query = "SELECT DocEntry, LineNum, BaseEntry, ItemCode, Dscription, Quantity, Price, TaxCode, DocDate, UomCode FROM DRF1 WHERE DocEntry =" + DocEntry + "ORDER BY LineNum ASC";
            return SelectRaw<Detail4>(query);
        }


        public List<ReceivedOrderModel4> GetListPurchaseDelivery()
        {
            //Obtener la ultima Entrada de mercancia preliminar
            string query = "SELECT DocEntry, DocNum, DocDate, CardCode, CardName, NumAtCard, DocStatus,'Borrador' as Tipo, Comments FROM ODRF WHERE CANCELED != 'Y' AND	ObjType = 20 ORDER BY DocEntry desc";
            return SelectRaw<ReceivedOrderModel4>(query);
        }

        public List<PurchaseDeliveryNotes> GetListPDeliveriesByDateRange(string Fecha1, string Fecha2)
        {
            //Obtener la lista de Entradas de mercancias abiertas en un rango de fechas
            string query = $"EXEC SW_PD_GetPurchaseDeliveryByDates '{Fecha1}', '{Fecha2}'";
            return SelectRaw<PurchaseDeliveryNotes>(query);
        }
        public List<PDNLines> GetlinesPurchaseDelivery(int DocEntry)
        {
            //Obtener el detalle de Entradas de mercancias
            string query = $"EXEC [SW_PD_GetPurchaseDeliveryLines] {DocEntry}";
            return SelectRaw<PDNLines>(query);
        }
        public string GetPurchaseDeliveryStatus(int DocEntry)
        {
            //Obtener el detalle de Entradas de mercancias
            string query = $"SELECT DocStatus FROM OPDN WHERE DocEntry={DocEntry}";
            var res = SelectRaw<Status>(query);

            if (res.Count == 0)
                throw new Exception("Error al consultar estado de la entrada de mercancías");

            return res[0].DocStatus;
        }

    }

    public class PurchaseDeliveryNotes
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }     
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string NumAtCard { get; set; }
        public string DocStatus { get; set; }
        public string ObjType { get; set; }
        public string Comments { get; set; }
        public string WhsCode { get; set; }
        public List<PDNLines> Lines { get; set; }
    }

    public class PDNLines
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public int BaseEntry { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string WhsCode { get; set; }
        public string TaxCode { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
    }

    public class Status
    {       
        public string DocStatus { get; set; }      
    }

    public class PurchaseInvoicesModel
    {
        public string Session_Id { get; set;}
        public List<PurchaseDeliveryNotes> purchaseDeliveryNotes { get; set; }    

    }
}
