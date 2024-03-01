using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class ParametrosPersonalizadosTransaction : Attribute //atributo personalizado que le permite a una clase estar disponible para la transaccion de datos de forma masiva
    {
        public bool DisponibleParaTransaccion { get; set; }
        public ParametrosPersonalizadosTransaction(bool opcion)
        {
            DisponibleParaTransaccion = opcion;
        }

    }

    public class modelsTransaction
    {
        public string ConnectionStringsName = "";
        public bool ActivateActiveRecordsOnly = false;
        public bool ActiveRecordsInTransactionOnly = false;

        public modelsTransaction(bool ActiveRecordsInTransactionOnly, bool ActivateActiveRecordsOnly, string ConnectionStringsName = "DefaultConnection")
        {
            this.ActiveRecordsInTransactionOnly = ActiveRecordsInTransactionOnly;
            this.ActivateActiveRecordsOnly = ActivateActiveRecordsOnly;
            this.ConnectionStringsName = ConnectionStringsName;
        }

        public object InsertByTransaction<M, D>(M Master, List<D> Detalles, string userName)
        {
            object masterIdentity = 0;
            string primaryKeyMasterName = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[this.ConnectionStringsName].ToString()))
                {
                    conn.Open();
                    SqlTransaction transaccion = conn.BeginTransaction();
                    Type t = Master.GetType();

                    using (SqlCommand comandoMaestro = new SqlCommand(t.Name + "_Insert", conn, transaccion))
                    {

                        comandoMaestro.CommandType = CommandType.StoredProcedure;
                        PropertyInfo[] properties = t.GetProperties();

                        foreach (PropertyInfo p in properties)
                        {

                            string name = p.Name;
                            if (ActiveRecordsInTransactionOnly == true)
                            {
                                if (name == "Create_User")
                                    p.SetValue(Master, userName);
                                if (name == "Create_Date")
                                    p.SetValue(Master, DateTime.Now);
                                if (name == "Modify_Date")
                                    p.SetValue(Master, DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString()));

                            }
                            if (ActivateActiveRecordsOnly == true)
                            {
                                if (name == "Activo")
                                    p.SetValue(Master, true);
                            }

                            object p2 = p.GetValue(Master, null);
                            Type type = p.PropertyType;
                            var primary_keyM = p.GetCustomAttribute(typeof(KeyAttribute));//propiedad datanotation [Key] para verificar quien si es la llave de master.
                            if (primary_keyM == null)
                            {
                                comandoMaestro.Parameters.Add(new SqlParameter("@" + name, p2));
                            }
                            else
                                primaryKeyMasterName = name;
                        }
                        masterIdentity = Convert.ToInt32(comandoMaestro.ExecuteScalar());
                    }
                    ///Detalles
                    foreach (D detalle in Detalles)
                    {
                        var tipo = detalle.GetType();
                        PropertyInfo[] propertiesD = tipo.GetProperties();

                        using (SqlCommand comandoDetalle = new SqlCommand(tipo.Name + "_Insert", conn, transaccion))
                        {
                            comandoDetalle.CommandType = CommandType.StoredProcedure;

                            PropertyInfo[] properties = tipo.GetProperties();

                            foreach (PropertyInfo p in properties)
                            {

                                string name = p.Name;
                                //asignar valor a create user y create Date
                                if (ActiveRecordsInTransactionOnly == true)
                                {
                                    if (name == "Create_User")
                                        p.SetValue(detalle, userName);
                                    if (name == "Create_Date")
                                        p.SetValue(detalle, DateTime.Now);
                                    if (name == "Modify_Date")
                                        p.SetValue(detalle, DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString()));

                                }
                                if (ActivateActiveRecordsOnly == true)
                                {
                                    if (name == "Activo")
                                        p.SetValue(detalle, true);
                                }


                                object p2 = p.GetValue(detalle, null);
                                var primary_keyD = p.GetCustomAttribute(typeof(KeyAttribute));
                                if (primary_keyD == null)
                                {
                                    if (name == primaryKeyMasterName)//asignamos el valir del identity al detalle
                                    {
                                        p.SetValue(detalle, Convert.ToInt32(masterIdentity));
                                        comandoDetalle.Parameters.Add(new SqlParameter("@" + name, Convert.ToInt32(masterIdentity)));
                                    }
                                    else
                                    {
                                        comandoDetalle.Parameters.Add(new SqlParameter("@" + name, p2));
                                    }

                                }
                            }

                            comandoDetalle.ExecuteNonQuery();
                        }
                    }
                    transaccion.Commit();
                    conn.Close();
                }
                return masterIdentity;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }

        }

        //Esta funcion descativa todos los detalles viejos e inserta todos los detalles como nuevos. El parametro desactivarDetallesList es una lista de string de los nombes de las tablas a las que deberan descativarse todos los registros relacionados de master

        public object UpdateByTransaction<M, D>(M Master, List<D> DetallesNuevos, List<string> desactivarDetallesList, string userName)
        {
            object masterIdentity = 0;
            string primaryKeyMasterName = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[this.ConnectionStringsName].ToString()))
                {
                    conn.Open();
                    SqlTransaction transaccion = conn.BeginTransaction();
                    Type t = Master.GetType();

                    using (SqlCommand comandoMaestro = new SqlCommand(t.Name + "_Update", conn, transaccion))
                    {

                        comandoMaestro.CommandType = CommandType.StoredProcedure;
                        PropertyInfo[] properties = t.GetProperties();

                        foreach (PropertyInfo p in properties)
                        {

                            string name = p.Name;
                            if (ActiveRecordsInTransactionOnly == true)
                            {

                                if (name == "Modify_User")
                                    p.SetValue(Master, userName);
                                if (name == "Modify_Date")
                                    p.SetValue(Master, DateTime.Now);

                            }

                            object p2 = p.GetValue(Master, null);
                            Type type = p.PropertyType;
                            var primary_keyM = p.GetCustomAttribute(typeof(KeyAttribute));//propiedad datanotation [Key] para verificar quien si es la llave de master.
                            if (primary_keyM != null)
                            {
                                masterIdentity = p2;
                                primaryKeyMasterName = name;
                            }

                            comandoMaestro.Parameters.Add(new SqlParameter("@" + name, p2));

                        }
                        comandoMaestro.ExecuteNonQuery();
                    }



                    ///Detalles
                    ///

                    foreach (string tabla in desactivarDetallesList)
                    {
                        string query = "update " + tabla + " set activo=0 where activo=1 and " + primaryKeyMasterName + "=" + masterIdentity + "";
                        using (SqlCommand comandoTabla = new SqlCommand(query, conn, transaccion))
                        {
                            comandoTabla.CommandType = CommandType.Text;
                            comandoTabla.ExecuteNonQuery();
                        }
                    }

                    //despues de desactivados todos los detalles del master, procedemos con el siguiente forecha a insertar todos los detalles como nuevos
                    foreach (D detalle in DetallesNuevos)
                    {
                        var tipo = detalle.GetType();
                        PropertyInfo[] propertiesD = tipo.GetProperties();

                        using (SqlCommand comandoDetalle = new SqlCommand(tipo.Name + "_Insert", conn, transaccion))
                        {
                            comandoDetalle.CommandType = CommandType.StoredProcedure;

                            PropertyInfo[] properties = tipo.GetProperties();

                            foreach (PropertyInfo p in properties)
                            {

                                string name = p.Name;

                                if (ActiveRecordsInTransactionOnly == true)
                                {
                                    if (name == "Create_User")
                                        p.SetValue(detalle, userName);
                                    if (name == "Create_Date")
                                        p.SetValue(detalle, DateTime.Now);
                                    if (name == "Modify_User")
                                        p.SetValue(detalle, userName);
                                    if (name == "Modify_Date")
                                        p.SetValue(detalle, DateTime.Now);
                                }
                                if (ActivateActiveRecordsOnly == true)
                                {
                                    if (name == "Activo")
                                        p.SetValue(detalle, true);
                                }


                                object p2 = p.GetValue(detalle, null);
                                var primary_keyD = p.GetCustomAttribute(typeof(KeyAttribute));
                                if (primary_keyD == null)
                                {
                                    if (name == primaryKeyMasterName)//asignamos el valir del identity al detalle
                                    {
                                        p.SetValue(detalle, Convert.ToInt32(masterIdentity));
                                        comandoDetalle.Parameters.Add(new SqlParameter("@" + name, Convert.ToInt32(masterIdentity)));
                                    }
                                    else
                                    {
                                        comandoDetalle.Parameters.Add(new SqlParameter("@" + name, p2));
                                    }

                                }
                            }

                            comandoDetalle.ExecuteNonQuery();
                        }
                    }
                    transaccion.Commit();
                    conn.Close();
                }
                return masterIdentity;
            }
            catch (Exception ex)
            {
                throw;
            }

        }



    }
}