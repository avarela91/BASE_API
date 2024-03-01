using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.ComponentModel.DataAnnotations;
namespace ET
{
    public class ContextSolicitudCorreo : models<SolicitudCorreo>
    {
        public ContextSolicitudCorreo() : base("Id_SolicitudCorreo", "SolicitudCorreo", "ConsolaCorreos")
        {
            ActiveRecordsInTransactionOnly = true;
            ActivateActiveRecordsOnly = true;
        }


    }
    [Serializable]
    public class SolicitudCorreo
    {
        [Key]
        public int Id_SolicitudCorreo { get; set; }
        public string CodigoModulo { get; set; }
        public bool Aprobado { get; set; }
        public bool Procesado { get; set; }
        public string Nota { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Activo { get; set; }
    }

    public class ContextConfiguracion : models<Configuracion>
    {
        public ContextConfiguracion() : base("Id", "Configuracion", "ConsolaCorreos")
        {
            ActiveRecordsInTransactionOnly = true;
            ActivateActiveRecordsOnly = true;
        }
        public List<Configuracion> SelectGenerico(string Codigo)
        {
            string query = "Select [Id_Configuracion],[Modulo],[Codigo],[Descripcion],[EndPoint],[Create_User],[Create_Date],[Modify_User],[Modify_Date],[Activo]" +
                           " From [ConsolaCorreos].[dbo].[Configuracion]" +
                           " where Codigo ='" + Codigo + "'";

            return SelectRaw<Configuracion>(query);

        }
    }
    [Serializable()]
    public class Configuracion 
    {
        public int Id_Configuracion { get; set; }
        public string Modulo { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        //public string CorreoAutorizado { get; set; }
        public string EndPoint { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Activo { get; set; }
    }
    public class ContextConfiguracion_CorreosAuth : models<Configuracion_CorreosAuth>
    {
        public ContextConfiguracion_CorreosAuth() : base("Id_ConfiguracionCorreosAuth", "Configuracion_CorreosAuth", "ConsolaCorreos")
        {
            ActiveRecordsInTransactionOnly = true;
            ActivateActiveRecordsOnly = true;
        }
        public List<Configuracion_CorreosAuth> SelectGenerico(int Idconfiguracion)
        {
             string query = "SELECT [Id_Configuracion_CorreosAuth],[Id_Configuracion] ,[CorreoAutorizado],[Create_User],[Create_Date],[Modify_User],[Modify_Date],[Activo]"+
                            " FROM[ConsolaCorreos].[dbo].[Configuracion_CorreosAuth]"+
                            " where Id_Configuracion ='"+Idconfiguracion+"'";

            return SelectRaw<Configuracion_CorreosAuth>(query);

        }

    }
    public class Configuracion_CorreosAuth
    {
        public int Id_Configuracion_CorreosAuth { get; set; }
        public int Id_Configuracion { get; set; }
        public string CorreoAutorizado { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Activo { get; set; }
    }
    public class Respuesta 
    {
        public string status { get; set; }
        public string msj { get; set; }

    }
}