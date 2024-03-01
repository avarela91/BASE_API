using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos
{
    public class ContextOACT : models<Cuentas>
    {
        public ContextOACT() : base("IDOACT", "Cuentas", "Compras")
        {

            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
        public List<Cuentas> RellenarOACT()
        {
            ConnectionStringsName = "SAP";
            string query = "select null IdOACT," +
                             "AcctCode,ActId,Finanse,FatherNum,AcctName, " +
                             "null Tipo, null UserName,null Create_Date, null User_Modify, null Modify_Date, null Activo " +
                             "from OACT WHERE  FrozenFor = 'N' AND Postable = 'Y' and AcctName not like('*%') " +
                             "union all " +
                             "select null Id_OACT, " +
                             "AcctCode,ActId,Finanse,FatherNum,AcctName, " +
                             "null Tipo, null Create_User,null Create_Date, null Modify_User, null Modify_Date, null Activo " +
                             "from OACT WHERE  FrozenFor = 'Y' AND GETDATE() not between FrozenTo and FrozenFrom";
            return SelectRaw<Cuentas>(query);
        }
    }
    public class Cuentas : ICloneable
    {
        public int IDOACT { get; set; }
        public string AcctCode { get; set; }
        public string AcctName { get; set; }
        public DateTime Create_Date { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Activo { get; set; }
        public string UserName { get; set; }
        public string User_Modify { get; set; }
        public string Almacen { get; set; }
        public object Clone()
        {
            return Utiles.Copia(this);
        }
    }
}
