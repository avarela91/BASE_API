using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class PurchaseRequestContext : models<PurchaseRequest>
    {
        public PurchaseRequestContext() : base("DocEntry", "PurchaseRequest", "SAP")
        {

        }

        public List<PurchaseRequest> SW_PR_GetListPurchaseRequestByFolio(string folio)
        {
            var NewFolio = $"{folio}D";

            string query = $"EXEC SW_PR_GetListPurchaseRequestByFolio '{NewFolio}'";
            return SelectRaw<PurchaseRequest>(query);
        }

        public PurchaseRequest SW_PR_GetPurchaseRequestByDocEntry(long DocEntry)
        {
            PurchaseRequest purchase = new PurchaseRequest();
            string query = $"EXEC SW_PR_GetPurchaseRequestByDocEntry {DocEntry}";
            var resp = SelectRaw<PurchaseRequest>(query);

            if (resp.Count > 0)
                purchase = resp[0];

            return purchase;
        }

        public PurchaseRequest SW_PR_GetPurchaseRequestByFolio(string Folio)
        {
            PurchaseRequest purchase = new PurchaseRequest();
            string query = $"EXEC SW_PR_GetPurchaseRequestByFolio {Folio}";
            var resp = SelectRaw<PurchaseRequest>(query);

            if (resp.Count > 0)
                purchase = resp[0];

            return purchase;
        }

        public List<DocumentPurchaseLine> SW_PR_GetDetailPurchaseRequestByDocEntry(long DocEntry)
        {
            string query = $"EXEC SW_PR_GetDetailPurchaseRequestByDocEntry {DocEntry}";
            return SelectRaw<DocumentPurchaseLine>(query);
        }
    }

    public class PurchaseRequest
    {
        public long DocEntry { get; set; }
        public long DocNum { get; set; }
        public DocumentDocType DocType { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string NumAtCard { get; set; }
        public decimal DocTotal { get; set; }
        public string DocCur { get; set; }
        public decimal DocRate { get; set; }
        public string Comments { get; set; }
        public Int16 DocTime { get; set; }
        public int SlpCode { get; set; }
        public Int16 Series { get; set; }
        public DateTime TaxDate { get; set; }
        public string ShipToCode { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string PaymentRef { get; set; }
        public int TransId { get; set; }
        public decimal VatSum { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime CancelDate { get; set; }
        public string PeyMethod { get; set; }
        public DateTime VatDate { get; set; }
        public long OwnerCode { get; set; }
        public DocumentDocumentStatus DocStatus { get; set; }
        public string ManualNum { get; set; }
        public DateTime ClsDate { get; set; }
        public string SeriesStr { get; set; }
        public decimal DiscSum { get; set; }
        public DocumentCancelled CANCELED { get; set; }
        public long ReqType { get; set; }
        public string U_FolioRB { get; set; }
        public string Requester { get; set; }
        public string ReqName { get; set; }
        public string Branch2 { get; set; }
        public Int16 Branch { get; set; }
        public string Department2 { get; set; }
        public Int16 Department { get; set; }
        public string SlpCode2 { get; set; }
        public string OwnerCode2 { get; set; }
        public List<DocumentPurchaseLine> DocumentLines { get; set; }
    }

    public class DocumentPurchaseLine
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public decimal Quantity { get; set; }
        public DateTime ShipDate { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfVAT { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string VendorNum { get; set; }
        public string SerialNum { get; set; }
        public string WhsCode { get; set; }
        public int SlpCode { get; set; }
        public string AcctCode { get; set; }
        public string Project { get; set; }
        public string CodeBars { get; set; }
        public decimal Height1 { get; set; }
        public decimal Height2 { get; set; }
        public decimal Length1 { get; set; }
        public decimal Length2 { get; set; }
        public decimal Weight1 { get; set; }
        public decimal Weight2 { get; set; }
        public decimal Factor1 { get; set; }
        public decimal Factor2 { get; set; }
        public decimal Factor3 { get; set; }
        public decimal Factor4 { get; set; }
        public int BaseType { get; set; }
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public decimal Width1 { get; set; }
        public decimal Width2 { get; set; }
        public string TaxCode { get; set; }
        public DocumentDocumentLineTaxType TaxType { get; set; }
        public DocumentDocumentLineWTLiable WtLiable { get; set; }
        public decimal LineTotal { get; set; }
        public decimal VatSum { get; set; }
        public decimal PriceBefDi { get; set; }
        public DocumentDocumentLineLineStatus LineStatus { get; set; }
        public DocumentDocumentLineLineType LineType { get; set; }
        public DateTime PQTReqDate { get; set; }                                 // RequiredDate
        public int ActBaseLn { get; set; }
        public int DocEntry { get; set; }
        public string LineVendor { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
    }

    public enum DocumentDocType
    {

        /// <remarks/>
        dDocument_Items,

        /// <remarks/>
        dDocument_Service,
    }

    public enum DocumentDocumentStatus
    {

        /// <remarks/>
        bost_Open,

        /// <remarks/>
        bost_Close,

        /// <remarks/>
        bost_Paid,

        /// <remarks/>
        bost_Delivered,
    }

    public enum DocumentCancelled
    {

        /// <remarks/>
        tNO,

        /// <remarks/>
        tYES,
    }

    public enum DocumentDocumentLineTaxType
    {

        /// <remarks/>
        tt_Yes,

        /// <remarks/>
        tt_No,

        /// <remarks/>
        tt_UseTax,

        /// <remarks/>
        tt_OffsetTax,
    }

    public enum DocumentDocumentLineWTLiable
    {

        /// <remarks/>
        tNO,

        /// <remarks/>
        tYES,
    }

    public enum DocumentDocumentLineLineStatus
    {

        /// <remarks/>
        bost_Open,

        /// <remarks/>
        bost_Close,

        /// <remarks/>
        bost_Paid,

        /// <remarks/>
        bost_Delivered,
    }

    public enum DocumentDocumentLineLineType
    {

        /// <remarks/>
        dlt_Regular,

        /// <remarks/>
        dlt_Alternative,
    }
}
