using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace ET.Objetos
{
    public class PurchaseOrderContext : models<ReceivedOrderModel2>
    {
        public PurchaseOrderContext() : base("DocEntry", "OPDN", "SAP")
        {

        }

        public List<ReceivedOrderModel2> ObtenerOrdenCompra(string NumFac)
        {   
            //ejemplo para obtener la información de entrada de mercancia
            //string query = "SELECT OPDN.CardCode, OPDN.CardName, OPDN.DocEntry, OPDN.NumAtCard NumFac, (SELECT OSLP.SlpName FROM OSLP WHERE OSLP.SlpCode = OPDN.SlpCode) UserName, OPDN.DocDate CreateDate, OPDN.Comments Comment FROM OPDN WHERE NumAtCard = '" + NumFac + "'";
            
            //Orden de compra
            string query = "SELECT OPOR.CardCode, OPOR.CardName, OPOR.DocEntry, OPOR.NumAtCard NumFac, (SELECT OSLP.SlpName FROM OSLP WHERE OSLP.SlpCode = OPOR.SlpCode) UserName, OPOR.DocDate CreateDate, OPOR.Comments Comment FROM OPOR WHERE NumAtCard = '" + NumFac + "'";
            return SelectRaw<ReceivedOrderModel2>(query);
        }

        public List<Detail2> ObtenerDetalleORC(int DocEntry)
        {
            //ejemplo para obtener la información de entrada de mercancia
            //string query = "SELECT ItemCode, Dscription ItemName, Quantity, UomCode FROM PDN1 WHERE DocEntry = '" + DocEntry + "' ORDER BY ItemCode";
            
            //detalle orden de compra
            string query = "SELECT LineNum, ItemCode, Dscription ItemName, Quantity, UomCode FROM POR1 WHERE DocEntry = '" + DocEntry + "' ORDER BY LineNum";
            return SelectRaw<Detail2>(query);
        }

        public PurchaseRequest SW_PO_GetPurchaseOrderByFolio(string Folio)
        {
            PurchaseRequest purchase = new PurchaseRequest();
            string query = $"EXEC SW_PO_GetPurchaseOrderByFolio {Folio}";
            var resp = SelectRaw<PurchaseRequest>(query);

            if (resp.Count > 0)
                purchase = resp[0];

            return purchase;
        }

        public List<DocumentPurchaseLine> SW_PO_GetDetailPurchaseOrderByDocEntry(long DocEntry)
        {
            string query = $"EXEC SW_PO_GetDetailPurchaseOrderByDocEntry {DocEntry}";
            return SelectRaw<DocumentPurchaseLine>(query);
        }

    }
}
