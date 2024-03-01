using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class VendorContext : models<VendorModelET>
    {
        public VendorContext() : base("CardCode", "VendorModelET", "SAP")
        {

        }

        //Obtiene los tipos de moneda por proveedor
        public List<TypeCurrencyModel> SW_OC_GetTypeCurrencyByCardCode(string CardCode)
        {
            string query = $"EXEC SW_OC_GetTypeCurrencyByCardCode '{CardCode}'";
            return SelectRaw<TypeCurrencyModel>(query);
        }

        public List<VendorModelET> GetAllVendor()
        {
            string query = "SELECT CardCode, CardName FROM OCRD WHERE CardType = 'S' AND validFor = 'Y'";
            return SelectRaw<VendorModelET>(query);
        }

        public List<VendorModelET> SW_SP_GetVendorByItemCode(string id)
        {
            string query = string.Format("EXEC SW_SP_GetVendorByItemCode '{0}'", id);
            return SelectRaw<VendorModelET>(query);
        }

        public List<VendorModelET> GetVendorByItemCode(string id)
        {
            string query = string.Format("SELECT CardCode, CardName FROM OCRD WHERE CardType = 'S' AND AND validFor = 'Y' AND CardCode = (SELECT CardCode FROM OITM WHERE ItemCode = '{0}')", id);
            return SelectRaw<VendorModelET>(query);
        }

        public string GetTaxByItemCode(string id)
        {
            string query = string.Format("SELECT CASE WHEN VATLiable = 'Y' THEN 'ISV' ELSE 'EXE' END AS VATLiable FROM OITM WHERE ItemCode = '{0}'", id);

            string tax = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SAP"].ToString()))
            {

                SqlCommand cmd = new SqlCommand(query, cn);
                //cmd.Parameters.AddWithValue("@item", id);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                tax = cmd.ExecuteScalar().ToString();
            }

            return tax;
        }

    }

    public class TemplateModel
    {
        public List<ItemModelET> Items { get; set; }
        public List<VendorModelET> Vendors { get; set; }
    }

    public class VendorModelET
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public bool Selected { get; set; }
    }

    public class VendorModelET2
    {
        public string ItemCode { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }

    public class ListItemForVendor
    {
        public string VendorName { get; set; }
        public string Vendor { get; set; }
        public string Folio { get; set; }
        public List<ItemModelET> ListItem { get; set; }
    }

}
