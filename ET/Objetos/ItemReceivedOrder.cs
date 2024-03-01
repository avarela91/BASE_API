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
    public class ItemReceivedOrderContext : models<ItemReceivedOrderModel2>
    {

        public ItemReceivedOrderContext() : base("ItemCode", "OITM", "SAP")
        {

        }

        public List<ItemReceivedOrderModel3> ObtenerItemByCodeBar(string id)
        {
            //string query = string.Format("SELECT OITM.ItemCode, OITM.ItemName, OBCD.BcdCode AS CodeBars FROM OITM INNER JOIN OBCD ON OITM.ItemCode = OBCD.ItemCode WHERE BcdCode = '{0}'", id);
            string query = string.Format("SELECT DISTINCT t1.ItemCode, t1.ItemName, OBCD.BcdCode AS CodeBars, OUOM.UomCode AS UnitMed FROM OITM t1 FULL JOIN ITM1 ON ITM1.ItemCode = t1.ItemCode INNER JOIN OITW w ON W.ItemCode = t1.ItemCode LEFT JOIN OBCD ON t1.ItemCode = OBCD.ItemCode INNER JOIN OUOM ON OBCD.UomEntry = OUOM.UomEntry WHERE OBCD.BcdCode = '{0}'", id);
            return SelectRaw<ItemReceivedOrderModel3>(query);
        }

        public List<UgpModel2> ObtenerDetailItemByCodeBar(string ItemCode)
        {
            string query = string.Format("SELECT UGP1.LineNum, OUOM.UomEntry, OUOM.UomName, OUOM.UomCode FROM OITM INNER JOIN OUGP ON OITM.UgpEntry = OUGP.UgpEntry INNER JOIN UGP1 ON OUGP.UgpEntry = UGP1.UgpEntry INNER JOIN OUOM ON OUOM.UomEntry = UGP1.UomEntry WHERE OITM.ItemCode = '{0}'", ItemCode);
            return SelectRaw<UgpModel2>(query);
        }

        public void AddItemReceived(string ItemCode, string ItemName, decimal Quantity, decimal QuantityWH10, string CodeBars, string UomCode, string UomName, string Comment, int DocEntry)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OrdenRecibido"].ToString()))
                {
                    conn.Open();

                    sql = "INSERT INTO RO1 (ItemCode, ItemName, Quantity, QuantityWH10, CodeBars, UomCode, UomName, Comment, DocEntry, LineStatus) VALUES ('" + ItemCode + "', '"+ ItemName + "', "+ Quantity + ", "+ QuantityWH10 + ", '"+ CodeBars + "', '"+ UomCode + "', '"+ UomName + "', '"+ Comment + "', "+ DocEntry + ", 'O')";

                    command = new SqlCommand(sql, conn);

                    adapter.InsertCommand = new SqlCommand(sql, conn);
                    adapter.InsertCommand.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

    public class ItemReceivedOrderModel2
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string CodeBars { get; set; }
    }

    public class ItemReceivedOrderModel3
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string CodeBars { get; set; }
        public string UnitMed { get; set; }
        public List<UgpModel2> UnitM { get; set; }
    }

    public class UgpModel2
    {
        public int LineNum { get; set; }
        public int UomEntry { get; set; }
        public string UomName { get; set; }
        public string UomCode { get; set; }
    }

}
