using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class ItemsContext : models<ItemModelET>
    {
        public ItemsContext() : base("ItemCode", "ItemModelET", "SAP")
        {

        }

        public List<ItemModelET> GetItemsByCardCode(string id)
        {
            //string query = string.Format("SELECT ItemCode, ItemName, OnHand FROM OITM WHERE validFor = 'Y' AND frozenFor = 'N' AND CardCode = '{0}'", id);
            string query = string.Format("EXEC SW_SP_GetItemByCardCode '{0}'", id);
            return SelectRaw<ItemModelET>(query);
        }

    }

    public class ItemModelET
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string UomCode { get; set; }
        public decimal OnHand { get; set; }
        public string LineVendor { get; set; }
        public string TaxCode { get; set; }
    }
}
