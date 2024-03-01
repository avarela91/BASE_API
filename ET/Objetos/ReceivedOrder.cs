using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace ET.Objetos
{
    public class ReceivedOrderContext: models<ReceivedOrderModel2>
    {
        public ReceivedOrderContext() : base("DocEntry", "ORO", "OrdenRecibido")
        {
        }

        public List<ReceivedOrderModel2> ObtenerOrdenesRecibido()
        {
            string query = "SELECT DocEntry, InvoiceNum AS NumFac, UserName, DocStatus, CreateDate, Comment FROM ORO";
            return SelectRaw2<ReceivedOrderModel2>(query);
        }

        public List<ReceivedOrderModel2> ObtenerOrdenRecibido(string NumFac)
        {
            string query = "SELECT TOP 1 DocEntry ,InvoiceNum AS NumFac, UserName, CreateDate, Comment FROM ORO WHERE InvoiceNum = '" + NumFac + "' AND DocStatus = 'O'";
            return SelectRaw2<ReceivedOrderModel2>(query);
        }

        public List<Detail2> ObtenerDetalleORO(int DocEntry)
        {
            //string query = "SELECT ItemCode, ItemName, Quantity, UomCode, Comment FROM RO1 WHERE DocEntry = '" + DocEntry + "' ORDER BY ItemCode";
            string query = "SELECT ItemCode, ItemName, Quantity, QuantityFac, QuantityWH10, CodeBars, UomCode, Comment FROM RO1 WHERE DocEntry = '" + DocEntry + "' AND LineStatus = 'O' ORDER BY ItemCode";
            return SelectRaw2<Detail2>(query);
        }

        public List<Detail2> GetItemByDocEntry(int DocEntry, string ItemCode, string UomCode)
        {
            string query = "SELECT ItemCode, ItemName, Quantity, QuantityFac, QuantityWH10, UomCode, Comment FROM RO1 WHERE DocEntry = " + DocEntry + " AND ItemCode = '" + ItemCode + "' AND UomCode = '"+UomCode+"'";
            return SelectRaw2<Detail2>(query);
        }

        public void DeleteReceivedOrder(string NumFac, int UserSign2)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OrdenRecibido"].ToString()))
                {
                    conn.Open();

                    sql = "UPDATE ORO SET DocStatus = 'C', UserSign2 = "+ UserSign2 + " WHERE InvoiceNum = '" + NumFac+"'";

                    command = new SqlCommand(sql, conn);

                    adapter.DeleteCommand = new SqlCommand(sql, conn);
                    adapter.DeleteCommand.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteItemOR(int DocEntry, string ItemCode, string UomCode)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OrdenRecibido"].ToString()))
                {
                    conn.Open();

                    //sql = "DELETE FROM RO1 WHERE DocEntry = "+DocEntry+" AND ItemCode = '" + ItemCode + "'";
                    sql = "UPDATE RO1 SET LineStatus = 'C' WHERE DocEntry = " + DocEntry+" AND ItemCode = '" + ItemCode + "' AND UomCode = '" + UomCode + "'";

                    command = new SqlCommand(sql, conn);

                    adapter.DeleteCommand = new SqlCommand(sql, conn);
                    adapter.DeleteCommand.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateOrderR(string NumFac, int UserSign2, string Comment)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OrdenRecibido"].ToString()))
                {
                    conn.Open();

                    sql = "UPDATE ORO SET Comment = '"+Comment+"', UpdateDate = getdate(), UserSign2 = "+UserSign2+" WHERE InvoiceNum = '"+NumFac+"'";

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

        public void UpdateOrderRDetail(string ItemCode, decimal Quantity, decimal QuantityFac, decimal QuantityWH10, string UomCode, string Comment, int DocEntry)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OrdenRecibido"].ToString()))
                {
                    conn.Open();

                    sql = "UPDATE RO1 SET Quantity = "+Quantity+ ", QuantityFac = "+QuantityFac+", QuantityWH10 = " + QuantityWH10 + ",Comment = '"+Comment+"' WHERE ItemCode = '"+ItemCode+ "' AND UomCode = '"+ UomCode + "' AND DocEntry =" + DocEntry;

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

    public class ReceivedOrderModel2
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int DocEntry { get; set; }
        public string NumFac { get; set; }
        public string UserName { get; set; }
        public string DocStatus { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public string Comment { get; set; }
    }

    public class Detail2
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal Quantity { get; set; }
        public decimal QuantityFac { get; set; }
        public decimal QuantityWH10 { get; set; }
        public string CodeBars { get; set; }
        public string UomCode { get; set; }
        public string Comment { get; set; }
    }

    public class ReceivedOrderModel3
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int DocEntry { get; set; }
        public string NumFac { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Comment { get; set; }
        public List<Detail2> Detail { get; set; }
    }


    public class ReceivedOrderModel4
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string NumAtCard { get; set; }
        public string DocStatus { get; set; }
        public string Tipo { get; set; }
        public string Comments { get; set; }
        public List<Detail4> Detail { get; set; }
    }

    public class Detail4
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public int BaseEntry { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string TaxCode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DocDate { get; set; }
        public string UomCode { get; set; }
    }

}
