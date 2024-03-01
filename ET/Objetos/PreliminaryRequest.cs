using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace ET.Objetos
{
    public class PreliminaryRequestContext : models<PreliminaryRequestModelET>
    {
        public PreliminaryRequestContext() : base("DocEntry", "ODRF", "SAP")
        {

        }

        public List<PreliminaryRequestModelET> GetAllPreliminar()
        {
            string query = "SELECT DocEntry, DocStatus, ObjType, DocNum, ReqDate, DocDate, Comments, U_FolioRB, Requester, ReqName, ISNULL((SELECT OUBR.name FROM OUBR WHERE OUBR.Code = ODRF.Branch), 'Ninguna sucursal asignada') AS Branch2, ISNULL(Branch,0) AS Branch, ISNULL((SELECT OUDP.name FROM OUDP WHERE OUDP.Code = ODRF.Department), 'Ningún departamento asignado') AS Department2, ISNULL(Department, 0) AS Department, ISNULL(Email, '') FROM ODRF WHERE ObjType = '1470000113'";
            return SelectRaw<PreliminaryRequestModelET>(query);
        }

        public List<PreliminaryRequestModelET> SW_SC_GetSCPreliminarByFolio(string folio, string ObjType)
        {
            var NewFolio = "";
            if (ObjType == "22")
                NewFolio = $"{folio}O";
            else
                NewFolio = $"{folio}D";

            string query = $"EXEC [SW_SC_GetSCPreliminarByFolio] {NewFolio}, {ObjType}";
            return SelectRaw<PreliminaryRequestModelET>(query);
        }

        // Obtener todos los documnetos 
        public List<PreliminaryRequestModelET> GetAllDrafts(string objType, int ignorar, int filas, string filtro, string fechaInicio, string fechaFin, string estado)
        {
            string query = "EXEC SW_DF_GetAllDraftsByObjType '" + objType + "', " + ignorar + ", " + filas + ", '" + filtro + "', '" + fechaInicio + "', '" + fechaFin + "', '" + estado + "'";
            return SelectRaw<PreliminaryRequestModelET>(query);
        }

        public List<PreliminaryRequestModelET> GetPurchaseRequestPreByFolio(string folio)
        {
            string query = "";
            return SelectRaw<PreliminaryRequestModelET>(query);
        }

        public List<PreliminaryModelETP> GetPreliminarInfoByDocEntry(string id)
        {
            string query = string.Format("SELECT DocEntry, DocStatus, ObjType, DocNum, ReqDate, Comments, U_FolioRB, Requester, ReqName, ISNULL((SELECT OUBR.name FROM OUBR WHERE OUBR.Code = ODRF.Branch), 'Ninguna sucursal asignada') AS Branch2, ISNULL(Branch,0) AS Branch, ISNULL((SELECT OUDP.name FROM OUDP WHERE OUDP.Code = ODRF.Department), 'Ningún departamento asignado') AS Department2, ISNULL(Department, 0) AS Department, ISNULL(Email, '') FROM ODRF WHERE ObjType = '1470000113' AND DocEntry = '{0}'", id);

            return SelectRaw<PreliminaryModelETP>(query);
        }

        public PreliminaryModelETP SW_DF_GetDraftsByObjTypeAndDocEntry(int DocEntry, string ObjType)
        {
            PreliminaryModelETP drafts = new PreliminaryModelETP();

            string query = $"EXEC SW_DF_GetDraftsByObjTypeAndDocEntry {DocEntry}, {ObjType}";
            var response = SelectRaw<PreliminaryModelETP>(query);

            if(response.Count > 0)
            {
                drafts = response[0];
            }

            return drafts;
        }

        public List<ContentPreliminaryModelETP> GetContentPreliminarByDocEntry(string id)
        {
            string query = string.Format("SELECT ItemCode, Dscription, UomCode, LineVendor, PQTReqDate, Quantity, PriceBefDi, TaxCode, DocDate FROM DRF1 WHERE DocEntry = '{0}'", id);
            return SelectRaw<ContentPreliminaryModelETP>(query);
        }

        public List<ContentPreliminaryModelETP> SW_DF_GetDetailDraftsByDocEntry(string DocEntry)
        {
            string query = $"EXEC SW_DF_GetDetailDraftsByDocEntry {DocEntry}";
            return SelectRaw<ContentPreliminaryModelETP>(query);
        }

        public List<PreliminaryModelETP> GetPreliminarInfoByU_FolioRB(string folio)
        {
            string query = string.Format("SELECT DocEntry, DocStatus, ObjType, DocNum, ReqDate, Comments, U_FolioRB, Requester, ReqName, ISNULL((SELECT OUBR.name FROM OUBR WHERE OUBR.Code = ODRF.Branch), 'Ninguna sucursal asignada') AS Branch2, ISNULL(Branch,0) AS Branch, ISNULL((SELECT OUDP.name FROM OUDP WHERE OUDP.Code = ODRF.Department), 'Ningún departamento asignado') AS Department2, ISNULL(Department, 0) AS Department, ISNULL(Email, '') FROM ODRF WHERE ObjType = '1470000113' AND U_FolioRB LIKE '%{0}%'", folio);

            return SelectRaw<PreliminaryModelETP>(query);
        }

        public List<PreliminaryModelETP> GetPreliminarInfoByFolio(string u_folio)
        {
            string query = string.Format("SELECT DocEntry, DocStatus, ObjType, DocNum, ReqDate, Comments, U_FolioRB, Requester, ReqName, ISNULL((SELECT OUBR.name FROM OUBR WHERE OUBR.Code = ODRF.Branch), 'Ninguna sucursal asignada') AS Branch2, ISNULL(Branch,0) AS Branch, ISNULL((SELECT OUDP.name FROM OUDP WHERE OUDP.Code = ODRF.Department), 'Ningún departamento asignado') AS Department2, ISNULL(Department, 0) AS Department, ISNULL(Email, '') FROM ODRF WHERE ObjType = '1470000113' AND U_FolioRB = '{0}'", u_folio);
            return SelectRaw<PreliminaryModelETP>(query);
        }

        public List<datosFolio> SW_GetFolioLast(string ObjType)
        {
            string query = $"EXEC SW_GE_GetFolioLast {ObjType}";
            return SelectRaw<datosFolio>(query);
        }

    }

    public class datosFolio
    {
        public string U_FolioRB { get; set; }
        public int DocEntry { get; set; }
    }

    public class datosFolio2
    {
        public string U_FolioRB { get; set; }
        public int DocEntry { get; set; }
    }

    public class PreliminaryRequestModelET
    {
        public int DocEntry { get; set; }
        public string DocStatus { get; set; }
        public string ObjType { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime DocDate { get; set; }
        public string Comments { get; set; }
        public string U_FolioRB { get; set; }
        public string Requester { get; set; }
        public string ReqName { get; set; }
        public string Branch2 { get; set; }
        public Int16 Branch { get; set; }             // cambio
        public string Department2 { get; set; }
        public Int16 Department { get; set; }           //cambio
        public int OwnerCode { get; set; }
        public int SlpCode { get; set; }
    }

    public class ContentPreliminaryModelET
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        //public List<UgpModel> uom {get;set;}
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
        public string LineStatus { get; set; }
        public string LineVendor { get; set; }
        public DateTime PQTReqDate { get; set; }  // CAMBIA //fecha requerida 
        public decimal Quantity { get; set; }
        public decimal PriceBefDi { get; set; }
        public string TaxCode { get; set; }
        public DateTime DocDate { get; set; } //fecha de publicación
        public decimal Weight { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class PreliminaryModelET
    {
        public string Session_Id { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int DocNum { get; set; }
        public string DocStatus { get; set; }
        public int DocEntry { get; set; }
        public string ObjType { get; set; }
        public int ReqType { get; set; }
        public string Requester { get; set; }
        public string ReqName { get; set; }
        public string Branch2 { get; set; }
        public Int16 Branch { get; set; }
        public string Department2 { get; set; }
        public Int16 Department { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime ReqDate { get; set; }
        public string Folio { get; set; }
        public string Comments { get; set; }
        public string OwnerCode2 { get; set; }
        public int OwnerCode { get; set; }
        public string SlpCode2 { get; set; }
        public int SlpCode { get; set; }
        public string DocumentType { get; set; }
        public List<ContentPreliminaryModelET> Content { get; set; }
    }

    public class ContentPreliminaryModelETP
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string UomCode { get; set; }
        public string LineStatus { get; set; }
        public string LineVendor { get; set; }
        public DateTime PQTReqDate { get; set; }  // CAMBIA //fecha requerida 
        public decimal Quantity { get; set; }
        public decimal PriceBefDi { get; set; }
        public string TaxCode { get; set; }
        public DateTime DocDate { get; set; } //fecha de publicación
        public decimal LineTotal { get; set; }
    }

    public class PreliminaryModelETP
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int DocNum { get; set; }
        public string DocStatus { get; set; }
        public int DocEntry { get; set; }
        public string ObjType { get; set; }
        public string Requester { get; set; }
        public string ReqName { get; set; }
        public string Branch2 { get; set; }
        public Int16 Branch { get; set; }
        public string Department2 { get; set; }
        public Int16 Department { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime ReqDate { get; set; }
        public string U_FolioRB { get; set; }                     // CAMBIA
        public string Comments { get; set; }
        public string OwnerCode2 { get; set; }
        public int OwnerCode { get; set; }
        public string SlpCode2 { get; set; }
        public int SlpCode { get; set; }
        public string Email { get; set; }
        public List<ContentPreliminaryModelETP> Content { get; set; }
    }

    public class ContentPreliminary
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string UomCode { get; set; }
        public string LineVendor { get; set; }
        public decimal Quantity { get; set; }
        public List<VendorModelET> Vendors { get; set; }
    }

    public class ContentPreliminary2
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string UomCode { get; set; }
        public string LineVendor { get; set; }
        public decimal Quantity { get; set; }
        public List<VendorModelET2> Vendors { get; set; }
    }

}
