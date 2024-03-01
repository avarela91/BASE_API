using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL;
using System.Collections.Generic;

namespace ET
{

    public class SucursalContext : models<Sucursal>
    {

        public SucursalContext()
            : base("Sucursal_Id", "Sucursal", "BaseDatosvariables")
        {
            ActivateActiveRecordsOnly = false;
            ActiveRecordsInTransactionOnly = false;
        }
    }

    [Serializable()]
    public class Sucursal : ICloneable
    {
        public int Sucursal_Id { get; set; }
        public string Codigo { get; set; }
        public string IP { get; set; }
        public int Puerto { get; set; }
        public string Nombre { get; set; }

        [ForeignKey("Sucursal_Id")]
        public List<Tienda> Tienda { get; set; }

        public object Clone()
        {
            return Utiles.Copia(this);
        }
    }

    [Serializable()]
    [Table(name: "Tienda")]
    [MyCustom(ActivateActiveRecordsOnly = false, ActiveRecordsInTransactionOnly = false)]
    public class Tienda : ICloneable
    {
        [Key]
        public int Tienda_Id { get; set; }
        public int Sucursal_Id { get; set; }
        public string Nombre { get; set; }

        public object Clone()
        {
            return Utiles.Copia(this);
        }
    }
}
