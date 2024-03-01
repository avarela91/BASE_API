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
    public class UnitMeaContext : models<UomEntryByUomCodeModel>
    {
        public UnitMeaContext() : base("UomEntry", "OUOM", "SAP")
        {

        }

        public List<UomEntryByUomCodeModel> SW_SP_GetUnitMeaByItemCode(string ItemCode)
        {
            string query = "EXEC [SW_SP_GetUnitMeaByItemCode]'" + ItemCode + "'";
            return SelectRaw<UomEntryByUomCodeModel>(query);
        }

        public List<UomEntryByUomCodeModel> SW_SP_UomEntryByUomCodeModel(string UomCode)
        {
            string query = "SELECT UomEntry FROM OUOM WHERE UomCode = '"+UomCode+"'";
            return SelectRaw<UomEntryByUomCodeModel>(query);
        }

        public decimal SW_OC_GetWeightByItemCode(string ItemCode)
        {
            decimal res = 0;

            string query = $"EXEC SW_OC_GetWeightByItemCode {ItemCode}";

            var resp = SelectRaw<WeightByItem>(query);

            if (resp.Count > 0)
                res = resp[0].Weight;

            return res;
        }
    }

    public class UomEntryByUomCodeModel
    {
        public int UomEntry { get; set; }
    }

    public class WeightByItem
    {
        public decimal Weight { get; set; }
    }

}
