using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.ComponentModel.DataAnnotations;

namespace ET.Objetos
{
    public class ContextEmpleado : models<Empleado>
    {
        public ContextEmpleado()
            : base("Id_Empleado", "Empleado", "RRHH")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }

        public List<Empleado> GetDeptoEmpleado(string CodEmpleado)
        {
            DateTime now = DateTime.Now;
            string query = "select Id_Empleado,t.Nombre NombreTienda,E.Id_Depto,Id_Area,CodigoEmpleado,Identidad,Nombres,Apellidos,E.Activo,D.Nombre NombreDepto,Correo" +
                           " from Empleado e" +
                           " inner join Depto d on e.Id_Depto = D.Id_Depto" +
                           " inner join tienda t on e.id_tienda = t.id_tienda" +
                           " where CodigoEmpleado='" + CodEmpleado + "'";
            return SelectRaw<Empleado>(query);
        }
        public List<Empleado> GetEmpleado(string CodEmpleado)
        {
            DateTime now = DateTime.Now;
            string query = "select Id_Empleado,t.Nombre NombreTienda,E.Id_Depto,Id_Area,CodigoEmpleado,Identidad,Nombres,Apellidos,E.Activo,D.Nombre NombreDepto" +
                           " from Empleado e" +
                           " inner join Depto d on e.Id_Depto = D.Id_Depto" +
                           " inner join tienda t on e.id_tienda = t.id_tienda" +
                           " where CodigoEmpleado='" + CodEmpleado + "'";
            return SelectRaw<Empleado>(query);
        }
    }

}

public class Empleado

{
    [Key]
    public int Id_Empleado { get; set; }
    public int Id_Tienda { get; set; }
    public int Id_Depto { get; set; }
    public int Id_Area { get; set; }
    public string CodigoEmpleado { get; set; }
    public string Identidad { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string NombreDepto { get; set; }
    public string NombreTienda { get; set; }
    public bool Activo { get; set; }
    public string Correo { get; set; }
}