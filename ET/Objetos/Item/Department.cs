using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos.Item
{
    public class ContextDepartment:models<OTIB>
    {
        public ContextDepartment():base("Id_OTIB", "OTIB","ERP")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly= true;
        }
    }

    public class OTIB
    {
        public int Id_OTIB { get; set; }
        public string Name { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Active { get; set; }
    }
}
