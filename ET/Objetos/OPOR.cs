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
    public class OPORContext : models<OPOR>
    {
        public OPORContext() : base("DocEntry", "OPOR", "SAP")
        {

        }

        public List<OPOR> RB_SP_GetOPORByEntry(int docentry)
        {
            string query = string.Format("SELECT DocEntry, DocNum, DocStatus, CardCode, CardName, NumAtCard, DocCur, DocRate, CurSource, isnull(Comments, '') as Comments FROM OPOR WHERE DocEntry = {0}", docentry);
            return SelectRaw<OPOR>(query);
        }

        public List<POR1> RB_SP_GetPOR1ByEntry(int docentry)
        {
            string query = string.Format("SELECT LineNum, ItemCode, Dscription, Quantity, WhsCode, Price, UomEntry, TaxCode, Currency, Rate FROM POR1 WHERE DocEntry = {0}", docentry);
            return SelectRaw<POR1>(query);
        }
    }

    public class OPOR
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string DocStatus { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string NumAtCard { get; set; }
        public string DocCur { get; set; }
        public decimal DocRate { get; set; }
        public string CurSource { get; set; }
        public string Comments { get; set; }
    }

    public class POR1
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public decimal Quantity { get; set; }
        public string WhsCode { get; set; }
        public decimal Price { get; set; }
        public int UomEntry { get; set; }
        public string TaxCode { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }
}
