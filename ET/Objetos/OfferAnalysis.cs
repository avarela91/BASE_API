using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class OfferAnalysisContext : models<LastEPModel>
    {
        public OfferAnalysisContext() : base("DocEntry", "LastEPModel", "SAP")
        {

        }

        public LastEPModel SW_OC_GetLastEPByItemCode(string ItemCode)
        {
           
            LastEPModel LastEp = new LastEPModel();

            try
            {
                string query = string.Format("EXEC SW_OC_GetLastEPByItemCode '{0}'", ItemCode);
                List<LastEPModel> ListLastEp = SelectRaw<LastEPModel>(query);

                if (ListLastEp.Count > 0)
                {
                    LastEp = ListLastEp[0];
                }

                return LastEp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SpendingModel SW_OC_GetSpending(int docEntry)
        {
            SpendingModel spending = new SpendingModel();

            try
            {
                string query = string.Format("EXEC SW_OC_GetLastSpendingByItemCode '{0}'", docEntry);
                List<SpendingModel> ListSpending = SelectRaw<SpendingModel>(query);

                if(ListSpending.Count > 0)
                {
                    spending = ListSpending[0];
                }

                return spending;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RateModel SW_OC_GetLastRate()
        {
            RateModel rateModel = new RateModel();

            try
            {
                string query = "EXEC SW_OC_GetLastRate 'USD', 'I'";
                List<RateModel> ListRate = SelectRaw<RateModel>(query);

                if(ListRate.Count > 0)
                {
                    rateModel = ListRate[0];
                }

                return rateModel;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }

    public class OfferAnalysisModel
    {
        [Key]
        public int Id { get; set; }
        public string DocOffer { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string DocStatus { get; set; }
        public List<OfferAnalysisDetailModel> Detail { get; set; }
    }

    public class OfferAnalysisDetailModel
    {
        [Key]
        public int Id { get; set; }
        public int LineNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Currency { get; set; }
        public string Folio { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UomCode { get; set; }
        public string TaxCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string LineStatus { get; set; }
        public string DocOffer { get; set; }
        public LastEPModel LastEp { get; set; }
        public SpendingModel Spending { get; set; }
    }

    public class LastEPModel
    { 
        public int DocEntry { get; set; } 
        public int DocNum { get; set; }
        public string DocCur { get; set; }
        public decimal DocRate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DocDate { get; set; }
        public string SlpName { get; set; }
        public string NumAtCard { get; set; }
        public string Comments { get; set; }
    }

    public class SpendingModel
    {
        public decimal Spending { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
    }

    public class RateModel
    {
        public DateTime RateDate { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }
}
