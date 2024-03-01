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
    public class CodeBarContext : models<CodeBarModel>
    {
        public CodeBarContext() : base("BcdEntry", "OBCD", "SAP")
        {

        }

        public List<CodeBarModel> SW_SP_GetBarCodeByItemCodeAndUomCode(string ItemCode, string UomCode)
        {
            string query = "EXEC [SW_SP_GetBarCodeByItemCodeAndUomCode]'" + ItemCode + "','"+ UomCode + "'";
            return SelectRaw<CodeBarModel>(query);
        }

        public CodeBarModel SW_SP_GetBarCodeByBcdEntry(int BcdEntry)
        {
            CodeBarModel resp = new CodeBarModel();
            string query = "EXEC [SW_SP_GetBarCodeByBcdEntry]" + BcdEntry;
            var response = SelectRaw<CodeBarModel>(query);

            if(response.Count > 0)
            {
                resp = response[0];
            }

            return resp;
        }
    }

    public class CodeBarModel
    {
        public int BcdEntry { get; set; }
        public string BcdCode { get; set; }
        public string BcdName { get; set; }
    }

}
