using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Graph;
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System.Web.Services.Description;
using Microsoft.Graph.Models;
using System.IO;

namespace DAL
{
    public class models<T>
    {
        public string primary_key = "";
        public string table_name = "";
        public T Entidad = (T)Activator.CreateInstance(typeof(T));
        public string ConnectionStringsName = "";
        public bool ActivateActiveRecordsOnly = false;
        public bool ActiveRecordsInTransactionOnly = false;
        private readonly IConfiguration _configuration;
        public models(string primary_key, string table_name, string ConnectionStringsName = "DefaultConnection")
        {
            this.primary_key = primary_key;
            this.table_name = table_name;
            this.ConnectionStringsName = ConnectionStringsName;

            //obtener aqui a IConfiguration

        }
        public string GetConnectionString(string connectionStringName)
        {
            string connectionString = new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json",optional:true, reloadOnChange:true).Build().GetSection("ConnectionStrings").GetSection(this.ConnectionStringsName).Value;//_configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"No se encontro ninguna cadena de conexion con el nombre '{connectionStringName}'.");
            }

            return connectionString;
        }

        public List<T> Select(Dictionary<string, string> filtros = null)
        {
            if (filtros == null)
                filtros = new Dictionary<string, string>();

            if (ActivateActiveRecordsOnly == true)
                filtros.Add("Activo", "true");

            if (ActiveRecordsInTransactionOnly == true)
            {

            }

            DataTable dt = new DataTable();
            List<T> Resgistros = new List<T>();
            this.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(this.ConnectionStringsName)))
                {
                    conn.Open();

                    var query = new SqlCommand(table_name + "_Select", conn);
                    query.CommandType = CommandType.StoredProcedure;

                    Type t = Entidad.GetType();
                    System.Reflection.PropertyInfo[] properties = t.GetProperties();

                    foreach (System.Reflection.PropertyInfo p in properties)
                    {
                        string name = p.Name;
                        object p2 = p.GetValue(Entidad, null);
                        Type type = p.PropertyType;

                        if (filtros.ContainsKey(name))
                            query.Parameters.Add(new SqlParameter("@" + name, filtros[name]));
                    }

                    using (var dr = query.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }

                Resgistros = Map<T>(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Resgistros;
        }


        public object Insert(T Entidad)
        {
            DataTable dt = new DataTable();
            object result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(this.ConnectionStringsName)))
                {
                    conn.Open();

                    var query = new SqlCommand(table_name + "_Insert", conn);
                    query.CommandType = CommandType.StoredProcedure;

                    Type t = Entidad.GetType();
                    System.Reflection.PropertyInfo[] properties = t.GetProperties();

                    foreach (System.Reflection.PropertyInfo p in properties)
                    {
                        //object p = p.GetValue(Entidad, null);
                        string name = p.Name;
                        //asignar valor a create user y create Date
                        if (ActiveRecordsInTransactionOnly == true)
                        {
                            if (name == "Create_User")
                                p.SetValue(Entidad, p.GetValue(Entidad, null));
                            if (name == "Create_Date")
                                p.SetValue(Entidad, DateTime.Parse(DateTime.Now.ToString()));
                            if (name == "Modify_Date")
                                p.SetValue(Entidad, DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString()));

                        }
                        if (ActivateActiveRecordsOnly == true)
                        {
                            if (name == "Activo")
                                p.SetValue(Entidad, true);
                        }

                        object p2 = p.GetValue(Entidad, null);
                        Type type = p.PropertyType;
                        if (!primary_key.Equals(name))
                        {
                            query.Parameters.Add(new SqlParameter("@" + name, p2));
                        }
                    }

                    result = query.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }

            return result;
        }
        public object Update(T Entidad, string UserName)
        {
            DataTable dt = new DataTable();
            object result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(this.ConnectionStringsName)))
                {
                    conn.Open();

                    var query = new SqlCommand(table_name + "_Update", conn);
                    query.CommandType = CommandType.StoredProcedure;

                    Type t = Entidad.GetType();
                    System.Reflection.PropertyInfo[] properties = t.GetProperties();

                    foreach (System.Reflection.PropertyInfo p in properties)
                    {
                        string name = p.Name;

                        //asignar valor a modify user y modify Date
                        if (ActiveRecordsInTransactionOnly == true)
                        {
                            if (name == "Modify_User")
                                p.SetValue(Entidad, UserName);
                            if (name == "Modify_Date")
                                p.SetValue(Entidad, DateTime.Now);

                        }

                        object p2 = p.GetValue(Entidad, null);
                        Type type = p.PropertyType;

                        query.Parameters.Add(new SqlParameter("@" + name, p2));
                    }

                    result = query.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public List<T> SelectRaw<T>(string consulta)
        {
            DataTable dt = new DataTable();
            List<T> ObjetosMapeo = new List<T>();
            T ObjetoMapeo = (T)Activator.CreateInstance(typeof(T));

            try
            {


                using (SqlConnection conn = new SqlConnection(GetConnectionString(this.ConnectionStringsName)))
                {
                    conn.Open();

                    var query = new SqlCommand(consulta, conn);
                    query.CommandType = CommandType.Text;

                    using (var dr = query.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }

                ObjetosMapeo = Map<T>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjetosMapeo;
        }

        public List<T> SelectRaw2<T>(string consulta)
        {
            DataTable dt = new DataTable();
            List<T> ObjetosMapeo = new List<T>();
            T ObjetoMapeo = (T)Activator.CreateInstance(typeof(T));

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OrdenRecibido"].ToString()))
                {
                    conn.Open();

                    var query = new SqlCommand(consulta, conn);
                    query.CommandType = CommandType.Text;

                    using (var dr = query.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }

                ObjetosMapeo = Map<T>(dt);
            }
            catch (Exception ex)
            {
                throw;
            }

            return ObjetosMapeo;
        }

        public async Task<int> QueryRaw(string consulta)
        {
            int result;
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[this.ConnectionStringsName].ToString()))
                {
                    conn.Open();

                    var query = new SqlCommand(consulta, conn);
                    query.CommandType = CommandType.Text;

                    result = await query.ExecuteNonQueryAsync();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        public SqlDataReader consulta()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Server"].ToString());
            conn.Open();
            string consulta = " SELECT * FROM OPENROWSET('ADSDSOObject','adsdatasource','SELECT sAmaccountname  FROM ''LDAP://ferrebaratillo.com/DC=ferrebaratillo,DC=com'' WHERE givenname = ''Edwin'' ') ";
            var query = new SqlCommand(consulta, conn);
            query.CommandType = CommandType.Text;

            var dr = query.ExecuteReader();
            return dr;
        }

        public List<T> Map<T>(DataTable result)
        {
            T EntidadParse = (T)Activator.CreateInstance(typeof(T));
            Type t = EntidadParse.GetType();
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            List<T> Entidades = new List<T>();

            try
            {


                foreach (DataRow row in result.Rows)
                {
                    T Entidad = (T)Activator.CreateInstance(typeof(T));

                    foreach (System.Reflection.PropertyInfo p in properties)
                    {
                        string name = p.Name;
                        object p2 = p.GetValue(Entidad, null);
                        Type type = p.PropertyType;

                        if (result.Columns.Contains(name) && !name.Contains("Specified"))
                        {
                            //verificar si existe un campo Specified que indica si el campo se especifica o no
                            bool Specified = true;
                            List<System.Reflection.PropertyInfo> Listpropiedad = properties.Where(x => x.Name == p.Name + "Specified").ToList();
                            //si el campo es distinto de null verificar si existe campo Specified
                            if (!(row[name] is DBNull))
                            {
                                //si existe obtener el valor booleano
                                if (Listpropiedad.Count > 0)
                                {
                                    Listpropiedad[0].SetValue(Entidad, true);
                                }
                            }
                            else
                            {
                                //si existe obtener el valor booleano
                                if (Listpropiedad.Count > 0)
                                {
                                    Listpropiedad[0].SetValue(Entidad, false);
                                }
                            }

                            if (type.Equals(typeof(string)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, null);
                                else
                                    p.SetValue(Entidad, (string)row[name]);
                            }
                            else if (type.Equals(typeof(Int16)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, 0);
                                else
                                    p.SetValue(Entidad, (Int16)row[name]);
                            }
                            else if (type.Equals(typeof(int)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, 0);
                                else
                                    p.SetValue(Entidad, (int)row[name]);
                            }
                            else if (type.Equals(typeof(double)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, 0);
                                else
                                    p.SetValue(Entidad, (double)row[name]);
                            }
                            else if (type.Equals(typeof(DateTime)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue);
                                else
                                    p.SetValue(Entidad, (DateTime)row[name]);

                            }
                            else if (type.Equals(typeof(short)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, null);
                                else
                                    p.SetValue(Entidad, (short)row[name]);
                            }
                            else if (type.Equals(typeof(byte)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, 0);
                                else
                                    p.SetValue(Entidad, (byte)row[name]);
                            }
                            else if (type.Equals(typeof(bool)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, false);
                                else
                                    p.SetValue(Entidad, (bool)row[name]);
                            }
                            else if (type.Equals(typeof(decimal)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, 0.0m);
                                else
                                    p.SetValue(Entidad, (decimal)row[name]);
                            }
                            else if (type.Equals(typeof(long)))
                            {
                                if (row[name] is DBNull)
                                    p.SetValue(Entidad, 0);
                                else
                                    p.SetValue(Entidad, Convert.ToInt64(row[name]));
                            }
                            else if (row[name] == null)
                            {
                                p.SetValue(Entidad, null);
                            }
                        }
                    }
                    Entidades.Add(Entidad);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Entidades;
        }

        public void Clear()
        {
            Type t = Entidad.GetType();
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            try
            {
                foreach (System.Reflection.PropertyInfo p in properties)
                {
                    string name = p.Name;
                    object p2 = p.GetValue(Entidad, null);
                    Type type = p.PropertyType;

                    if (type.Equals(typeof(string)))
                    {
                        p.SetValue(Entidad, string.Empty);
                    }
                    else if (type.Equals(typeof(int)))
                    {
                        p.SetValue(Entidad, 0);
                    }
                    else if (type.Equals(typeof(double)))
                    {
                        p.SetValue(Entidad, 0);
                    }
                    else if (type.Equals(typeof(DateTime)))
                    {
                        p.SetValue(Entidad, DateTime.MinValue);

                    }
                    else if (type.Equals(typeof(short)))
                    {
                        p.SetValue(Entidad, string.Empty);
                    }
                    else if (type.Equals(typeof(byte)))
                    {
                        p.SetValue(Entidad, 0);
                    }
                    else if (type.Equals(typeof(bool)))
                    {
                        p.SetValue(Entidad, false);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
