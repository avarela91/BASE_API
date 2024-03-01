using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class ImportOfferDataContext : models<ImportOfferDataModel>
    {
        public ImportOfferDataContext() : base("DocOffer", "ImportOfferDataModel", "Compras")
        {

        }

        public List<ImportOfferDataModel> SW_CP_GetAllOffer()
        {
            string query = "SELECT * FROM ImportOfferData ORDER BY CreateDate DESC";
            return SelectRaw<ImportOfferDataModel>(query);
        }

        public string SW_Ge_GetDocOfferAnalysis(string DocType)
        {
            string docOffer = "";
            string query = $"EXEC SW_Ge_GetDocOfferAnalysis {DocType}";
            var offer = SelectRaw<ImportOfferDataModel>(query);

            if (offer.Count > 0)
            {
                docOffer = offer[0].DocOffer;
            }

            return docOffer;
        }

        // endpoint encagado de obtener la información general de el analisis de ofertas de compra.
        public List<SaveAnalysisModel> SW_AO_GetListAnalysisOffer(string DocType)
        {
            string query = $"EXEC SW_AO_GetListAnalysisOffer {DocType}";
            return SelectRaw<SaveAnalysisModel>(query);
        }

        public SaveAnalysisModel SW_AO_GetAnalysisOfferById(int Id)
        {
            var analysisOffer = new SaveAnalysisModel();
            string query = $"EXEC SW_AO_GetAnalysisOfferById {Id}";
            var ana = SelectRaw<SaveAnalysisModel>(query);

            if(ana.Count > 0)
            {
                analysisOffer = ana[0];
            }

            return analysisOffer;
        }

        public SaveAnalysisModel SW_AO_GetAnalysisOfferByIdAndDocType(int Id, string DocType)
        {
            var analysisOffer = new SaveAnalysisModel();
            string query = $"EXEC SW_AO_GetAnalysisOfferByIdAndDocType {Id}, {DocType}";
            var ana = SelectRaw<SaveAnalysisModel>(query);

            if (ana.Count > 0)
            {
                analysisOffer = ana[0];
            }

            return analysisOffer;
        }

        public List<SaveDetailAnalysisModel> SW_AO_GetListAnalysisOfferById(int Id)
        {
            string query = $"EXEC SW_AO_GetListAnalysisOfferById {Id}";
            return SelectRaw<SaveDetailAnalysisModel>(query);
        }

        public List<SaveListVendorModel> SW_AO_GetListVendorsByIdAndFk(int IdDoc, int IdItem)
        {
            string query = $"EXEC SW_AO_GetListVendorsByIdAndFk {IdDoc}, {IdItem}";
            return SelectRaw<SaveListVendorModel>(query);
        }

        public void SW_AO_UpdateAnalysisOffer(SaveAnalysisModel model, string DocType)
        {
            string updateGen = "set dateformat dmy; UPDATE AnalysisOffer SET Description = @Description, Comment = @Comment, UpdateDate = @UpdateDate," +
                " UserSign2 = @UserSign2 WHERE DocOffer = @DocOffer AND DocType = @DocType";

            string selectGen = "SELECT TOP 1 Id FROM AnalysisOffer WHERE DocOffer = @DocOffer AND DocType = @DocType";

            string updateDetail = "UPDATE AnalysisOfferDetail SET Cost = @Cost, Quantity = @Quantity WHERE ItemCode = @ItemCode" +
                " AND FK_AnalysisOffer = @FK_AnalysisOffer";

            string selectDetail = "SELECT TOP 1 Id FROM AnalysisOfferDetail WHERE ItemCode = @ItemCode AND " +
                "FK_AnalysisOffer = @FK_AnalysisOffer";

            string updateVendor = "UPDATE AnalysisOfferVendir SET Expense = @Expense, Price = @Price, Quantity = @Quantity, " +
                "Selected = @Selected WHERE CardCode = @CardCode AND FK_AnalysisOffer = @FK_AnalysisOffer " +
                "AND FK_AnalysisOfferDetail = @FK_AnalysisOfferDetail";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
            {
                try
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        var idGen = 0;

                        conn.Open();
                        transaction = conn.BeginTransaction();

                        using (SqlCommand command = new SqlCommand(updateGen, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@Description", model.Description);
                            command.Parameters.AddWithValue("@Comment", model.Comment);
                            command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@UserSign2", model.UserName);
                            command.Parameters.AddWithValue("@DocOffer", model.DocOffer);
                            command.Parameters.AddWithValue("@DocType", DocType);

                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand(selectGen, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DocOffer", model.DocOffer);
                            command.Parameters.AddWithValue("@DocType", DocType);

                            idGen = Convert.ToInt32(command.ExecuteScalar());
                        }

                        foreach(var item in model.Items)
                        {
                            var idDetail = 0;

                            using (SqlCommand command = new SqlCommand(updateDetail, conn, transaction))
                            {
                                command.Parameters.AddWithValue("@Cost", item.Cost);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                command.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);

                                command.ExecuteNonQuery();
                            }

                            using (SqlCommand command = new SqlCommand(selectDetail, conn, transaction))
                            {
                                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                command.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);

                                idDetail = Convert.ToInt32(command.ExecuteScalar());
                            }

                            foreach(var vendor in item.Vendors)
                            {
                                using(SqlCommand command = new SqlCommand(updateVendor, conn, transaction))
                                {
                                    command.Parameters.AddWithValue("@Expense", vendor.Expense);
                                    command.Parameters.AddWithValue("@Price", vendor.Price);
                                    command.Parameters.AddWithValue("@Quantity", vendor.Quantity);
                                    command.Parameters.AddWithValue("@Selected", vendor.Selected);
                                    command.Parameters.AddWithValue("@CardCode", vendor.CardCode);
                                    command.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);
                                    command.Parameters.AddWithValue("@FK_AnalysisOfferDetail", idDetail);

                                    command.ExecuteNonQuery();
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

        public string SW_AO_SaveAnalysisOffer(SaveAnalysisModel model, string folio)
        {
            string InsertAnalysisGen = "set dateformat dmy; INSERT INTO AnalysisOffer (DocOffer, UserName, Description, Comment, " +
                "CreateDate, DocStatus, DocOfferA, DocType) VALUES (@DocOffer, @UserName,  @Description, @Comment, @CreateDate, " +
                "@DocStatus, @DocOfferA, @DocType)";

            string SelectAnalysisGen = "SELECT TOP 1 Id FROM AnalysisOffer WHERE DocType = 'PO' ORDER BY Id DESC";

            string InsertAnalysisItem = "set dateformat dmy; INSERT INTO AnalysisOfferDetail (LineNum, ItemCode, ItemName, " +
                "TaxCode, UomCode, LineStatus, DateLastEP, Cost, Quantity, FK_AnalysisOffer) VALUES (@LineNum, @ItemCode, " +
                "@ItemName, @TaxCode, @UomCode, @LineStatus, @DateLastEP, @Cost, @Quantity, @FK_AnalysisOffer)";

            string SelectAnalysisItem = "SELECT TOP 1 Id FROM AnalysisOfferDetail WHERE FK_AnalysisOffer = @FK_AnalysisOffer " +
                "ORDER BY LineNum DESC";

            string InsertAnalysisVendir = "INSERT INTO AnalysisOfferVendir (CardCode, CardName, Expense, Currency, Folio, " +
                "Price, Quantity, Selected, FK_AnalysisOffer, FK_AnalysisOfferDetail) VALUES (@CardCode, @CardName, @Expense, " +
                "@Currency, @Folio, @Price, @Quantity, @Selected, @FK_AnalysisOffer, @FK_AnalysisOfferDetail)";

            string selectAnalysisTypeA = "SELECT TOP 1 DocOffer FROM AnalysisOffer WHERE DocType = 'PO' ORDER BY Id DESC";

            string closeAnalysisTypeA = "set dateformat dmy; UPDATE AnalysisOffer SET DocStatus = @DocStatus, UpdateDate = @UpdateDate, " +
                "UserSign2 = @UserSign2 WHERE DocOffer = @DocOffer AND DocType = @DocType";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
            {
                try
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        int idGen = 0;
                        //string DocOfferNew = "";

                        conn.Open();
                        transaction = conn.BeginTransaction();

                        //// obtiene el ultimo docoffer de los analisis de tipo A
                        //SqlCommand commandSe = new SqlCommand(selectAnalysisTypeA, conn, transaction);
                        //var docOfferLast = Convert.ToString(commandSe.ExecuteScalar());

                        //if (string.IsNullOrEmpty(docOfferLast))
                        //    DocOfferNew = "Doc1";
                        //else
                        //{
                        //    Match m = Regex.Match(docOfferLast, "(\\d+)");
                        //    int f = int.Parse(m.Value) + 1;
                        //    DocOfferNew = "Doc" + f;
                        //}

                        // AGREGAR DATOS DE ANALISIS DE OFERTAS DE COMPRA 
                        using (SqlCommand command = new SqlCommand(InsertAnalysisGen, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DocOffer", folio);                       // DocOffer en el cual se basa.
                            command.Parameters.AddWithValue("@UserName", model.UserName);
                            command.Parameters.AddWithValue("@Description", model.Description);
                            command.Parameters.AddWithValue("@Comment", model.Comment);
                            command.Parameters.AddWithValue("@CreateDate", model.CreateDate);
                            command.Parameters.AddWithValue("@DocStatus", "O");

                            command.Parameters.AddWithValue("@DocOfferA", model.DocOffer);   
                            command.Parameters.AddWithValue("@DocType", "PO");                               // SIGNIFICA PRE ORDEN DE COMPRA.

                            command.ExecuteNonQuery();
                        }

                        SqlCommand commandS = new SqlCommand(SelectAnalysisGen, conn, transaction);
                        idGen = Convert.ToInt32(commandS.ExecuteScalar());

                        foreach (var item in model.Items)
                        {
                            int idItem = 0;

                            // insertar detalle del analisis de solicitudes de compra.
                            using (SqlCommand command = new SqlCommand(InsertAnalysisItem, conn, transaction))
                            {
                                command.Parameters.AddWithValue("@LineNum", model.Items.IndexOf(item));
                                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                command.Parameters.AddWithValue("@ItemName", item.ItemName);
                                command.Parameters.AddWithValue("@TaxCode", item.TaxCode);
                                command.Parameters.AddWithValue("@UomCode", item.UomCode);
                                command.Parameters.AddWithValue("@LineStatus", "O");
                                command.Parameters.AddWithValue("@DateLastEP", item.DateLastEP.ToString("yyyy-MM-dd") == "0001-01-01" ? Convert.ToDateTime("01/01/1753") : item.DateLastEP);
                                command.Parameters.AddWithValue("@Cost", item.Cost);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                command.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);

                                command.ExecuteNonQuery();
                            }

                            SqlCommand commandI = new SqlCommand(SelectAnalysisItem, conn, transaction);
                            commandI.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);
                            idItem = Convert.ToInt32(commandI.ExecuteScalar());

                            foreach (var vendor in item.Vendors)
                            {
                                // INSERTAR DATOS DE PROVEEDORES RELACIONADOS.
                                using (SqlCommand command = new SqlCommand(InsertAnalysisVendir, conn, transaction))
                                {
                                    command.Parameters.AddWithValue("@CardCode", vendor.CardCode);
                                    command.Parameters.AddWithValue("@CardName", vendor.CardName);
                                    command.Parameters.AddWithValue("@Expense", vendor.Expense);
                                    command.Parameters.AddWithValue("@Currency", vendor.Currency);
                                    command.Parameters.AddWithValue("@Folio", vendor.Folio);
                                    command.Parameters.AddWithValue("@Price", vendor.Price);
                                    command.Parameters.AddWithValue("@Quantity", vendor.Quantity);
                                    command.Parameters.AddWithValue("@Selected", vendor.Selected);
                                    command.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);
                                    command.Parameters.AddWithValue("@FK_AnalysisOfferDetail", idItem);

                                    command.ExecuteNonQuery();
                                }
                            }

                        }

                        using(SqlCommand command = new SqlCommand(closeAnalysisTypeA, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DocStatus", "C");
                            command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@UserSign2", model.UserName);
                            command.Parameters.AddWithValue("@DocOffer", model.DocOffer);
                            command.Parameters.AddWithValue("@DocType", "A");

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        return folio;
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

        //Obtiene la información general de una oferta de compra
        public ImportOfferDataModel SW_CP_GetDocOfferById(string DocOffer)
        {
            ImportOfferDataModel OfferAnality = new ImportOfferDataModel();
            string query = $"EXEC SW_CP_GetDocOfferById {DocOffer}";
            var offer = SelectRaw<ImportOfferDataModel>(query);

            if(offer.Count > 0)
            {
                OfferAnality = offer[0];
            }

            return OfferAnality;
        }

        public OfferAnalysisModel SW_CP_GetDocOfferByDocOfferAndDocType(string DocOffer, string DocType)
        {
            OfferAnalysisModel offerAnalysis = new OfferAnalysisModel();
            string query = $"EXEC SW_CP_GetDocOfferByDocOfferAndDocType {DocOffer}, {DocType}";
            var offer = SelectRaw<OfferAnalysisModel>(query);

            if (offer.Count > 0)
            {
                offerAnalysis = offer[0];
            }

            return offerAnalysis;
        }

        //Obtiene el detalle de una oferta realizada
        public List<SaveDetailAnalysisModel> SW_CP_GetOfferDetailById(int Id)
        {
            string query = $"EXEC SW_CP_GetOfferDetailById {Id}";
            return SelectRaw<SaveDetailAnalysisModel>(query);
        }

        //cambiar el estado de un documento
        public void SW_CP_ChangeStatusOffer(string DocOffer, string status, string UserSign2, string DocType)
        {
            string query = "UPDATE AnalysisOffer SET DocStatus = @DocStatus, UpdateDate = GETDATE(), " +
                "UserSign2 = @UserSign2 " +
                "WHERE DocOffer = @DocOffer AND DocType = @DocType";
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
                {
                    conn.Open();

                    command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@DocStatus", status);
                    command.Parameters.AddWithValue("@DocOffer", DocOffer);
                    command.Parameters.AddWithValue("@UserSign2", UserSign2);
                    command.Parameters.AddWithValue("@DocType", DocType);

                    //adapter.DeleteCommand = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void SW_CP_ChangeStatusOfferByiD(int Id, string status, string UserSign2, string DocType)
        {
            string query = "UPDATE AnalysisOffer SET DocStatus = @DocStatus, UpdateDate = GETDATE(), " +
                "UserSign2 = @UserSign2 " +
                "WHERE Id = @Id AND DocType = @DocType";
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
                {
                    conn.Open();

                    command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@DocStatus", status);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@UserSign2", UserSign2);
                    command.Parameters.AddWithValue("@DocType", DocType);

                    //adapter.DeleteCommand = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void SW_CP_AnalysisOffer(SaveAnalysisModel model, string DocOffer)
        {
            string InsertAnalysisGen = "set dateformat dmy; INSERT INTO AnalysisOffer (DocOffer, UserName, Description, Comment, " +
                "CreateDate, DocStatus, DocType) VALUES (@DocOffer, @UserName,  @Description, @Comment, @CreateDate, @DocStatus, " +
                "@DocType)";

            string SelectAnalysisGen = "SELECT TOP 1 Id FROM AnalysisOffer WHERE DocType = 'A' ORDER BY Id DESC";

            string InsertAnalysisItem = "set dateformat dmy; INSERT INTO AnalysisOfferDetail (LineNum, ItemCode, ItemName, " +
                "TaxCode, UomCode, LineStatus, Quantity, FK_AnalysisOffer) VALUES (@LineNum, @ItemCode, @ItemName, @TaxCode, " +
                "@UomCode, @LineStatus, @Quantity, @FK_AnalysisOffer)";

            string SelectAnalysisItem = "SELECT TOP 1 Id FROM AnalysisOfferDetail WHERE FK_AnalysisOffer = @FK_AnalysisOffer " +
                "ORDER BY LineNum DESC";

            string InsertAnalysisVendir = "INSERT INTO AnalysisOfferVendir (CardCode, CardName, Currency, Folio, Price," +
                "FK_AnalysisOffer, FK_AnalysisOfferDetail) VALUES (@CardCode, @CardName, @Currency, @Folio, @Price, " +
                "@FK_AnalysisOffer, @FK_AnalysisOfferDetail)";

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

                        // AGREGAR DATOS DE ANALISIS DE OFERTAS DE COMPRA 
                        using (SqlCommand command = new SqlCommand(InsertAnalysisGen, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DocOffer", DocOffer);
                            command.Parameters.AddWithValue("@UserName", model.UserName);
                            command.Parameters.AddWithValue("@Description", model.Description);
                            command.Parameters.AddWithValue("@Comment", model.Comment);
                            command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@DocStatus", "O");
                            command.Parameters.AddWithValue("@DocType", "A");

                            command.ExecuteNonQuery();
                        }

                        SqlCommand commandS = new SqlCommand(SelectAnalysisGen, conn, transaction);
                        idGen = Convert.ToInt32(commandS.ExecuteScalar());

                        foreach (var item in model.Items)
                        {
                            int idItem = 0;

                            // insertar detalle del analisis de solicitudes de compra.
                            using (SqlCommand command = new SqlCommand(InsertAnalysisItem, conn, transaction))
                            {
                                command.Parameters.AddWithValue("@LineNum", model.Items.IndexOf(item));
                                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                command.Parameters.AddWithValue("@ItemName", item.ItemName);
                                command.Parameters.AddWithValue("@TaxCode", item.TaxCode);
                                command.Parameters.AddWithValue("@UomCode", item.UomCode);
                                command.Parameters.AddWithValue("@LineStatus", "O");
                                //command.Parameters.AddWithValue("@DateLastEP", item.DateLastEP.ToString("yyyy-MM-dd") == "0001-01-01" ? Convert.ToDateTime("01/01/1753") : item.DateLastEP);
                                //command.Parameters.AddWithValue("@Cost", item.Cost);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                command.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);

                                command.ExecuteNonQuery();
                            }

                            SqlCommand commandI = new SqlCommand(SelectAnalysisItem, conn, transaction);
                            commandI.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);
                            idItem = Convert.ToInt32(commandI.ExecuteScalar());

                            foreach (var vendor in item.Vendors)
                            {
                                // INSERTAR DATOS DE PROVEEDORES RELACIONADOS.
                                using (SqlCommand command = new SqlCommand(InsertAnalysisVendir, conn, transaction))
                                {
                                    command.Parameters.AddWithValue("@CardCode", vendor.CardCode);
                                    command.Parameters.AddWithValue("@CardName", vendor.CardName);
                                    //command.Parameters.AddWithValue("@Expense", vendor.Expense);
                                    command.Parameters.AddWithValue("@Currency", vendor.Currency);
                                    command.Parameters.AddWithValue("@Folio", vendor.Folio);
                                    command.Parameters.AddWithValue("@Price", vendor.Price);
                                    //command.Parameters.AddWithValue("@Quantity", vendor.Quantity);
                                    //command.Parameters.AddWithValue("@Selected", vendor.Selected);
                                    command.Parameters.AddWithValue("@FK_AnalysisOffer", idGen);
                                    command.Parameters.AddWithValue("@FK_AnalysisOfferDetail", idItem);

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
                catch (Exception ex)
                {
                    conn.Close();

                    throw new Exception(ex.Message);
                }

            }
        }

        public void SW_CP_SaveOffer(GeneralImportInfoET data)
        {
            string general = "INSERT INTO ImportOfferData (DocOffer, Description, Comment, UserName, CreateDate) VALUES (@DocOffer, @Description, @Comment, @UserName, getdate())";
            string detail = "INSERT INTO ImportOfferDataDetail (LineNum, CardCode, CardName, Currency, Folio, ItemCode, ItemName, UomCode, Quantity, Price, DocOffer, TaxCode) VALUES (@LineNum, @CardCode, @CardName, @Currency, @Folio, @ItemCode, @ItemName, @UomCode, @Quantity, @Price, @DocOffer, @TaxCode)";

            string selectGen = "SELECT TOP 1 DocOffer FROM ImportOfferData ORDER BY CreateDate DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
            {
                try
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        string DocGen = "";

                        conn.Open();
                        transaction = conn.BeginTransaction();

                        using (SqlCommand command = new SqlCommand(general, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DocOffer", data.DocOffer);
                            command.Parameters.AddWithValue("@Description", data.Description);
                            command.Parameters.AddWithValue("@Comment", data.Comment);
                            command.Parameters.AddWithValue("@UserName", data.UserName);

                            command.ExecuteNonQuery();
                        }

                        SqlCommand commandS = new SqlCommand(selectGen, conn, transaction);
                        DocGen = Convert.ToString(commandS.ExecuteScalar());

                        foreach (var item in data.Items)
                        {
                            using (SqlCommand command = new SqlCommand(detail, conn, transaction))
                            {
                                command.Parameters.AddWithValue("@LineNum", data.Items.IndexOf(item));
                                command.Parameters.AddWithValue("@CardCode", item.CardCode);
                                command.Parameters.AddWithValue("@CardName", item.CardName);
                                command.Parameters.AddWithValue("@Currency", item.Currency);
                                command.Parameters.AddWithValue("@Folio", item.Folio);
                                command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                                command.Parameters.AddWithValue("@ItemName", item.ItemName);
                                command.Parameters.AddWithValue("@UomCode", item.UomCode);
                                command.Parameters.AddWithValue("@TaxCode", item.TaxCode);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                command.Parameters.AddWithValue("@Price", item.Price);
                                command.Parameters.AddWithValue("@DocOffer", DocGen);

                                command.ExecuteNonQuery();
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
                catch (Exception ex)
                {
                    conn.Close();

                    throw new Exception(ex.Message);
                }
            }
        }

        //public bool SW_CP_SaveOffer(string docoffer, string descript, string comment, string username)
        //{
        //    bool ret = false;

        //    SqlCommand command;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    string query = "";

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
        //        {
        //            conn.Open();

        //            //query = string.Format("INSERT INTO ImportOfferData (DocOffer, Description, Comment, UserName, CreateDate) VALUES ('{0}', '{1}', '{2}', '{3}', getdate())",docoffer,descript,comment,username);
        //            query = string.Format("INSERT INTO ImportOfferData (DocOffer, Description, Comment, UserName, CreateDate) VALUES (@DocOffer, @Description, @Comment, @UserName, getdate())");

        //            command = new SqlCommand(query, conn);
        //            command.Parameters.AddWithValue("@DocOffer", docoffer);
        //            command.Parameters.AddWithValue("@Description", descript);
        //            command.Parameters.AddWithValue("@Comment", comment);
        //            command.Parameters.AddWithValue("@UserName", username);

        //            //adapter.InsertCommand = new SqlCommand(query, conn);
        //            command.ExecuteNonQuery();

        //            command.Dispose();
        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return ret;
        //}

        //public bool SW_CP_SaveOfferDetail(int LineNum, string CardCode, string CardName, string Currency, string ItemCode, string ItemName, string UomCode, string TaxCode, decimal Quantity, decimal Price, string Folio, string DocOffer)
        //{
        //    bool ret = false;

        //    SqlCommand command;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    string query = "";

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
        //        {
        //            conn.Open();

        //            //query = string.Format("INSERT INTO ImportOfferDataDetail (LineNum, CardCode, ItemCode, ItemName, Quantity, Price, DocOffer) VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5}, '{6}')", LineNum, CardCode, ItemCode, ItemName, Quantity, Price, DocOffer);
        //            query = "INSERT INTO ImportOfferDataDetail (LineNum, CardCode, CardName, Currency, Folio, ItemCode, ItemName, UomCode, Quantity, Price, DocOffer, TaxCode) VALUES (@LineNum, @CardCode, @CardName, @Currency, @Folio, @ItemCode, @ItemName, @UomCode, @Quantity, @Price, @DocOffer, @TaxCode)";

        //            command = new SqlCommand(query, conn);
        //            command.Parameters.AddWithValue("@LineNum", LineNum);
        //            command.Parameters.AddWithValue("@CardCode", CardCode);
        //            command.Parameters.AddWithValue("@CardName", CardName);
        //            command.Parameters.AddWithValue("@Currency", Currency);
        //            command.Parameters.AddWithValue("@Folio", Folio);
        //            command.Parameters.AddWithValue("@ItemCode", ItemCode);
        //            command.Parameters.AddWithValue("@ItemName", ItemName);
        //            command.Parameters.AddWithValue("@UomCode", UomCode);
        //            command.Parameters.AddWithValue("@TaxCode", TaxCode);
        //            command.Parameters.AddWithValue("@Quantity", Quantity);
        //            command.Parameters.AddWithValue("@Price", Price);
        //            command.Parameters.AddWithValue("@DocOffer", DocOffer);

        //            //adapter.InsertCommand = new SqlCommand(query, conn);
        //            command.ExecuteNonQuery();

        //            command.Dispose();
        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return ret;
        //}

        public bool SW_CP_UpdateOfferUser(string DocOffer, string UserName)
        {
            bool ret = false;

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string query = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
                {
                    conn.Open();

                    query = "UPDATE ImportOfferData SET UserSign2 = @UserSign2, UpdateDate = GETDATE() WHERE DocOffer = @DocOffer";

                    command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@UserSign2", UserName);
                    command.Parameters.AddWithValue("@DocOffer", DocOffer);

                    //adapter.UpdateCommand = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        public bool SW_CP_UpdateStatusOfferDetail(string DocOffer)
        {
            bool ret = false;

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string query = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Compras"].ToString()))
                {
                    conn.Open();

                    query = "UPDATE ImportOfferDataDetail SET LineStatus = 'C' WHERE DocOffer = @DocOffer";

                    command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@DocOffer", DocOffer);

                    //adapter.UpdateCommand = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    command.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }
    }

    public class SaveAnalysisModel
    {
        [Key]
        public int Id { get; set; }
        public string DocOffer { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string DocStatus { get; set; }
        public List<SaveDetailAnalysisModel> Items { get; set; }
    }

    public class SaveDetailAnalysisModel
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string TaxCode { get; set; }
        public string UomCode { get; set; }
        public DateTime DateLastEP { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Expense { get; set; }

        public decimal CurrentPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public decimal Stock { get; set; }

        public int LineNum { get; set; }
        public string LineStatus { get; set; }
        public List<SaveListVendorModel> Vendors { get; set; }
        public LastEPModel LastEp { get; set; }
        public SpendingModel Spending { get; set; }
    }

    public class SaveListVendorModel
    {
        public int Id { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public decimal Expense { get; set; }
        public string Currency { get; set; }
        public string Folio { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public int Selected { get; set; }
    }

    public class ImportOfferDataModel
    {
        [Key]
        public string DocOffer { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string DocStatus { get; set; }
        public List<ImportOfferDataDetailModel> Detail { get; set; }
    }

    public class ImportOfferDataDetailModel
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
    }

    public class TypeCurrencyModel
    {
        public string CurrCode { get; set; }
        public string CurrName { get; set; }
        public string DocCurrCod { get; set; }
        public string FrgnName { get; set; }
    }

    public class GeneralImportInfoET
    {
        public string UserName { get; set; }
        public string DocOffer { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public List<DatosModelET> Items { get; set; }
    }

    public class DatosModelET
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UomCode { get; set; }
        public string TaxCode { get; set; }
        public decimal Quantity { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string Folio { get; set; }
    }

}
