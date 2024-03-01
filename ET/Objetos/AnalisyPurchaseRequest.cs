using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class AnalisyPurchaseRequestContext : models<AnalisySCModel>
    {
        public AnalisyPurchaseRequestContext() : base("Id", "AnalysisPurchaseOffers", "Compras")
        {
        }

        public List<AnalysisSC> SW_SC_ListAnalysis()
        {
            string query = "EXEC [SW_SC_GetListAnalysisSC]";
            return SelectRaw<AnalysisSC>(query);
        }

        public AnalysisSC SW_SC_GetAnalysis(int id)
        {
            AnalysisSC analysisSC = new AnalysisSC();
            string query = $"EXEC [SW_SC_GetAnalysisSCById] {id}";

            List<AnalysisSC> ListAnalysis = SelectRaw<AnalysisSC>(query);

            if (ListAnalysis.Count > 0)
                analysisSC = ListAnalysis[0];

            return analysisSC;
        }

        // lista de item por id de análisis principal
        public List<AnalysisSCDetail> SW_SC_ListItemAnalysis(int ID)
        {
            string query = $"EXEC [SW_SC_GetListItemsByIdAnalysisSC] {ID}";
            return SelectRaw<AnalysisSCDetail>(query);
        }

        // list vendors by id analysis and id item
        public List<VendorByAnalysisSC> SW_SC_ListVendorsByItem(int IdGen, int IdItem)
        {
            string query = $"EXEC [SW_SC_ListVendorsByItem] {IdGen}, {IdItem}";
            return SelectRaw<VendorByAnalysisSC>(query);
        }

        public void SW_SC_ChangeStatusAnalysisSCById(int id, string userName, string status)
        {
            string query = "UPDATE AnalysisPurchaseOffers SET DocStatus = @DocStatus, UserUpdated = @UserUpdated, UpdateDate = @UpdateDate " +
                "WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
            {
                try
                {
                    conn.Open();

                    // insertar información general del analisis solicitudes de compra
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@DocStatus", status);
                        command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@UserUpdated", userName);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();

                    throw new Exception(ex.Message);
                }
            }
        }

        public void SW_SC_ChangeStatusAnalysisOCById(int id, string userName, string status)
        {
            string query = "UPDATE AnalysisOffer SET DocStatus = @DocStatus, UserSign2 = @UserSign2, " +
                "UpdateDate = @UpdateDate WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
            {
                try
                {
                    conn.Open();

                    // insertar información general del analisis solicitudes de compra
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@DocStatus", status);
                        command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@UserSign2", userName);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();

                    throw new Exception(ex.Message);
                }
            }
        }

        public void Sw_SC_UpdateAnalysis(int Id, AnalisySCModel data, string newFolio)
        {
            string updateGen = "UPDATE [dbo].[AnalysisPurchaseOffers] SET [DateNeeded] = @DateNeeded" +
                ",[DocStatus] = @DocStatus" +
                ",[UpdateDate] = @UpdateDate" +
                ",[UserUpdated] = @UserUpdated" +
                ",[Comments] = @Comments" +
                " WHERE Id = @Id";

            string updateItem = "UPDATE [dbo].[AnalysisPurchaseOffersDetail] SET [UomEntry] = @UomEntry" +
                ",[Quantity] = @Quantity" +
                " WHERE [FK_AnalysisPurchaseOffers] = @FK AND Id = @Id";

            string selectVendor = "SELECT Id, CardCode, CardName, FK_AnalysisPurchaseOffers, FK_AnalysisPurchaseOffersDetail " +
                " FROM VendorByPurchaseRequest " +
                " WHERE FK_AnalysisPurchaseOffers = @FK AND FK_AnalysisPurchaseOffersDetail = @IdItem";

            string insertVendor = "INSERT INTO VendorByPurchaseRequest(CardCode, CardName, FK_AnalysisPurchaseOffers, FK_AnalysisPurchaseOffersDetail)" +
                " VALUES(@CardCode, @CardName, @FK_AnalysisPurchaseOffers, @FK_AnalysisPurchaseOffersDetail)";


            string queryItem = "INSERT INTO AnalysisPurchaseOffersDetail(LineNum, LineStatus, ItemCode, Dscription, UomEntry, Stock10, StockGen, UCC," +
                "Balance, Suggestion, Tax, Quantity, FK_AnalysisPurchaseOffers) VALUES(@LineNum, @LineStatus, @ItemCode, @Dscription, @UomEntry, @Stock10, @StockGen, @UCC," +
                "@Balance, @Suggestion, @Tax, @Quantity, @FK_AnalysisPurchaseOffers)";

            string selectItem = "SELECT TOP 1 Id FROM AnalysisPurchaseOffersDetail WHERE FK_AnalysisPurchaseOffers = @FK_AnalysisPurchaseOffers ORDER BY Id DESC";

            string queryVendor = "INSERT INTO VendorByPurchaseRequest(CardCode, CardName, FK_AnalysisPurchaseOffers, FK_AnalysisPurchaseOffersDetail)" +
                "VALUES(@CardCode, @CardName, @FK_AnalysisPurchaseOffers, @FK_AnalysisPurchaseOffersDetail)";

            //string deleteVendor = "DELETE FROM VendorByPurchaseRequest WHERE FK_AnalysisPurchaseOffers = @FK_Gen AND FK_AnalysisPurchaseOffersDetail = @FK_Item";
            string deleteVendor = "DELETE FROM VendorByPurchaseRequest WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
            {
                try
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();

                        // ACTUALIZAR DATOS DE SOLICITUD DE COMPRA PRLIMINAR 
                        using (SqlCommand command = new SqlCommand(updateGen, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DateNeeded", data.RequriedDate);
                            command.Parameters.AddWithValue("@DocStatus", data.DocStatus);
                            command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@UserUpdated", data.UserName);
                            command.Parameters.AddWithValue("@Comments", data.Comments);
                            command.Parameters.AddWithValue("@Id", data.Id);

                            command.ExecuteNonQuery();
                        }

                        foreach (var item in data.Content)
                        {
                            using (SqlCommand command = new SqlCommand(updateItem, conn, transaction))
                            {
                                // command.Parameters.AddWithValue("@LineStatus", item.);
                                command.Parameters.AddWithValue("@UomEntry", item.UomEntry);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                command.Parameters.AddWithValue("@FK", Id);
                                command.Parameters.AddWithValue("@Id", item.Id);

                                command.ExecuteNonQuery();
                            }

                            List<ListVendor> listVendor = new List<ListVendor>();
                            SqlCommand selectV = new SqlCommand(selectVendor, conn, transaction);
                            selectV.Parameters.AddWithValue("@FK", Id);
                            selectV.Parameters.AddWithValue("@IdItem", item.Id);
                            SqlDataReader reader = selectV.ExecuteReader();

                            while (reader.Read())
                            {
                                var vendor = new ListVendor()
                                {
                                    Id = reader.GetInt32(0),
                                    CardCode = reader.GetString(1),
                                    CardName = reader.GetString(2),
                                    FK_AnalysisPurchaseOffers = reader.GetInt32(3),
                                    FK_AnalysisPurchaseOffersDetail = reader.GetInt32(4)
                                };

                                listVendor.Add(vendor);
                            }

                            reader.Close();

                            // inserta los nuevos elementos para de los proveedores.
                            foreach (var vendor in item.Vendors)
                            {
                                bool venExist = false;

                                foreach (var v in listVendor)
                                {
                                    if (v.CardCode == vendor.CardCode)
                                        venExist = true;
                                }

                                if (!venExist && item.Id != -1)
                                {
                                    using (SqlCommand command = new SqlCommand(insertVendor, conn, transaction))
                                    {
                                        command.Parameters.AddWithValue("@CardCode", vendor.CardCode);
                                        command.Parameters.AddWithValue("@CardName", vendor.CardName);
                                        command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffers", Id);
                                        command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffersDetail", item.Id);

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }

                            //elimina los proveedores que se deseleccionan
                            foreach (var v in listVendor)
                            {
                                bool venExist = false;

                                foreach (var vendor in item.Vendors)
                                {
                                    if (v.CardCode == vendor.CardCode)
                                        venExist = true;
                                }

                                if (!venExist && item.Id != -1)
                                {
                                    using (SqlCommand command = new SqlCommand(deleteVendor, conn, transaction))
                                    {
                                        command.Parameters.AddWithValue("@Id", v.Id);
                                        //command.Parameters.AddWithValue("@FK_Gen", Id);
                                        //command.Parameters.AddWithValue("@FK_Item", item.Id);

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }

                            // agregar nuevos items
                            if (item.Id == -1)
                            {
                                int idItem = 0;

                                // insertar detalle del analisis de solicitudes de compra.
                                using (SqlCommand command = new SqlCommand(queryItem, conn, transaction))
                                {
                                    command.Parameters.AddWithValue("@LineNum", data.Content.IndexOf(item));
                                    command.Parameters.AddWithValue("@LineStatus", 'O');
                                    command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                    command.Parameters.AddWithValue("@Dscription", item.ItemName);
                                    command.Parameters.AddWithValue("@UomEntry", item.UomEntry);
                                    command.Parameters.AddWithValue("@Stock10", item.Stock10);
                                    command.Parameters.AddWithValue("@StockGen", item.StockG);
                                    command.Parameters.AddWithValue("@UCC", item.UCC);
                                    command.Parameters.AddWithValue("@Balance", item.Saldo);
                                    command.Parameters.AddWithValue("@Suggestion", item.Sugerido);
                                    command.Parameters.AddWithValue("@Tax", item.TaxCode);
                                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                    command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffers", Id);

                                    command.ExecuteNonQuery();
                                }

                                // 
                                SqlCommand commandI = new SqlCommand(selectItem, conn, transaction);
                                commandI.Parameters.AddWithValue("@FK_AnalysisPurchaseOffers", Id);
                                idItem = Convert.ToInt32(commandI.ExecuteScalar());

                                foreach (var vendor in item.Vendors)
                                {
                                    // INSERTAR DATOS DE PROVEEDORES RELACIONADOS.
                                    using (SqlCommand command = new SqlCommand(queryVendor, conn, transaction))
                                    {
                                        command.Parameters.AddWithValue("@CardCode", vendor.CardCode);
                                        command.Parameters.AddWithValue("@CardName", vendor.CardName);
                                        command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffers", Id);
                                        command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffersDetail", idItem);

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }

                        }

                        transaction.Commit();

                    }
                    catch(Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            throw new Exception(ex2.Message);
                        }

                        throw new Exception(ex.Message);
                    }
                    
                }
                catch(Exception ex)
                {
                    conn.Close();

                    throw new Exception(ex.Message);
                }
            }
        }

        public void SW_SC_SaveAnalysis(AnalisySCModel data, string folio)
        {
            string queryGen = "INSERT INTO AnalysisPurchaseOffers(DateNeeded, DocStatus, CreatedDate, UpdateDate, UserCreated, StardDate, " +
                "EndDate, Requester, RequesterName, Branch, Depto, Comments, Folio) VALUES(@DateNeeded, @DocStatus, @CreatedDate, @UpdateDate, " +
                "@UserCreated, @StardDate, @EndDate, @Requester, @RequesterName, @Branch, @Depto, @Comments, @Folio)";

            string selectGen = "SELECT TOP 1 Id FROM AnalysisPurchaseOffers ORDER BY CreatedDate DESC";

            string queryItem = "INSERT INTO AnalysisPurchaseOffersDetail(LineNum, LineStatus, ItemCode, Dscription, UomEntry, Stock10, StockGen, UCC," +
                "Balance, Suggestion, Tax, Quantity, FK_AnalysisPurchaseOffers) VALUES(@LineNum, @LineStatus, @ItemCode, @Dscription, @UomEntry, @Stock10, @StockGen, @UCC," +
                "@Balance, @Suggestion, @Tax, @Quantity, @FK_AnalysisPurchaseOffers)";

            string selectItem = "SELECT TOP 1 Id FROM AnalysisPurchaseOffersDetail WHERE FK_AnalysisPurchaseOffers = @FK_AnalysisPurchaseOffers ORDER BY Id DESC";

            string queryVendor = "INSERT INTO VendorByPurchaseRequest(CardCode, CardName, FK_AnalysisPurchaseOffers, FK_AnalysisPurchaseOffersDetail)" +
                "VALUES(@CardCode, @CardName, @FK_AnalysisPurchaseOffers, @FK_AnalysisPurchaseOffersDetail)";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
            {
                try
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        int idGen = 0;

                        conn.Open();
                        transaction = conn.BeginTransaction();

                        // insertar información general del analisis solicitudes de compra
                        using (SqlCommand command = new SqlCommand(queryGen, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DateNeeded", data.RequriedDate);
                            command.Parameters.AddWithValue("@DocStatus", 'O');
                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@UserCreated", data.UserName);
                            command.Parameters.AddWithValue("@StardDate", data.FechaI);
                            command.Parameters.AddWithValue("@EndDate", data.FechaF);
                            command.Parameters.AddWithValue("@Requester", data.Requester);
                            command.Parameters.AddWithValue("@RequesterName", data.RequesterName);
                            command.Parameters.AddWithValue("@Branch", data.RequesterBranch);
                            command.Parameters.AddWithValue("@Depto", data.RequesterDepartment);
                            command.Parameters.AddWithValue("@Comments", data.Comments);
                            command.Parameters.AddWithValue("@Folio", folio);

                            command.ExecuteNonQuery();
                        }

                        SqlCommand commandS = new SqlCommand(selectGen, conn, transaction);
                        idGen = Convert.ToInt32(commandS.ExecuteScalar());

                        foreach (var item in data.Content)
                        {
                            int idItem = 0;

                            // insertar detalle del analisis de solicitudes de compra.
                            using (SqlCommand command = new SqlCommand(queryItem, conn, transaction))
                            {
                                command.Parameters.AddWithValue("@LineNum", data.Content.IndexOf(item));
                                command.Parameters.AddWithValue("@LineStatus", 'O');
                                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                command.Parameters.AddWithValue("@Dscription", item.ItemName);
                                command.Parameters.AddWithValue("@UomEntry", item.UomEntry);
                                command.Parameters.AddWithValue("@Stock10", item.Stock10);
                                command.Parameters.AddWithValue("@StockGen", item.StockG);
                                command.Parameters.AddWithValue("@UCC", item.UCC);
                                command.Parameters.AddWithValue("@Balance", item.Saldo);
                                command.Parameters.AddWithValue("@Suggestion", item.Sugerido);
                                command.Parameters.AddWithValue("@Tax", item.TaxCode);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffers", idGen);

                                command.ExecuteNonQuery();
                            }
                            
                            // 
                            SqlCommand commandI = new SqlCommand(selectItem, conn, transaction);
                            commandI.Parameters.AddWithValue("@FK_AnalysisPurchaseOffers", idGen);
                            idItem = Convert.ToInt32(commandI.ExecuteScalar());

                            foreach(var vendor in item.Vendors)
                            {
                                // INSERTAR DATOS DE PROVEEDORES RELACIONADOS.
                                using (SqlCommand command = new SqlCommand(queryVendor, conn, transaction))
                                {
                                    command.Parameters.AddWithValue("@CardCode", vendor.CardCode);
                                    command.Parameters.AddWithValue("@CardName", vendor.CardName);
                                    command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffers", idGen);
                                    command.Parameters.AddWithValue("@FK_AnalysisPurchaseOffersDetail", idItem);

                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            throw new Exception(ex2.Message);
                        }

                        throw new Exception(ex.Message);
                    }
                }
                catch(Exception ex)
                {
                    conn.Close();

                    throw new Exception(ex.Message);
                }
                    
            }


        }
    }

    public class AnalysisSC
    {
        public int Id { get; set; }
        public int DocEntry { get; set; }
        public DateTime DateNeeded { get; set; }
        public string DocStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UserCreated { get; set; }
        public string UserUpdated { get; set; }
        public DateTime StardDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Requester { get; set; }
        public string RequesterName { get; set; }
        public int Branch { get; set; }
        public int Depto { get; set; }
        public string Comments { get; set; }
        public string Folio { get; set; }
        public int ContDocumentPreClose { get; set; }
    }

    public class AnalysisSCDetail
    {
        public int Id { get; set; }
        public int LineNum { get; set; }
        public string LineStatus { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public int UomEntry { get; set; }
        public decimal Stock10 { get; set; }
        public decimal StockGen { get; set; }
        public decimal UCC { get; set; }
        public decimal Balance { get; set; }
        public decimal Suggestion { get; set; }
        public string Tax { get; set; }
        public decimal Quantity { get; set; }
        public int FK_AnalysisPurchaseOffers { get; set; }
    }

    public class VendorByAnalysisSC
    {
        public int Id { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int FK_AnalysisPurchaseOffers { get; set; }
        public int FK_AnalysisPurchaseOffersDetail { get; set; }
    }

    public class AnalisySCModel
    {
        public string Session_Id { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        public string DocStatus { get; set; }
        public string ObjType { get; set; }
        public int ReqType { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public int RequesterBranch { get; set; }
        public int RequesterDepartment { get; set; }
        public DateTime FechaI { get; set; }
        public DateTime FechaF { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public DateTime RequriedDate { get; set; }
        public int Series { get; set; }
        public int SlpCode { get; set; }
        public int OwnerCode { get; set; }
        public List<ContentAnalisy> Content { get; set; }
        public string Comments { get; set; }
        public string Folio { get; set; }
    }

    public class ContentAnalisy
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
        public decimal Stock10 { get; set; }
        public decimal StockG { get; set; }
        public decimal UCC { get; set; }
        public decimal Saldo { get; set; }
        public decimal Sugerido { get; set; }
        public DateTime RequiredDate { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string TaxCode { get; set; }
        public List<ListVendor> Vendors { get; set; }
    }

    public class ListVendor
    {
        public int Id { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int FK_AnalysisPurchaseOffers { get; set; }
        public int FK_AnalysisPurchaseOffersDetail { get; set; }
    }
}
