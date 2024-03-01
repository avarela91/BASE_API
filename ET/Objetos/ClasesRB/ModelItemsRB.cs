using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Sap_WEB.Models.ClaseRB
{
   public class ModelItemsRB
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public  string FrgnName { get; set; }
        public  string Peso { get; set; }
        public  string OnHand { get; set; }
        public  string UomEntry { get; set; }
       public string Currency { get; set; }
    }
    public class Currency
    {
        public List<string> ItemsCodes { get; set; }
        public string ConnectionString { get; set; }
        public string WHS { get; set; }


    }

    public class PriceList
    {
        public string Codigo { get; set; }
        public string DefaultPriceList { get; set; }
    }
}