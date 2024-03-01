using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL; 

namespace ET
{
    public class ContextOWHS: models<OWHS>
    {
        public ContextOWHS() : base("WhsCode", "OWHS", "SAP")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }


        public List<OWHS> RellenarOWSH()/**Trae un camion Especifico para asignarlo a una gira**/
        {
            ConnectionStringsName = "SAP";
            string query = "select WhsCode,WhsName,U_Tipo from OWHS where U_RB_TipoAlmacen='TiendaRB'or U_RB_TipoAlmacen='TiendaSAP'";

            return SelectRaw<OWHS>(query);
        }
    }
     public class OWHS 
     {
        public string WhsCode { get; set; }
       public string WhsName { get; set; }
        public string U_Tipo { get; set; }
     }



}
