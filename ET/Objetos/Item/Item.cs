using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos.Item
{
    public class ContextItem:models<OITM>
    {
        public ContextItem() : base("Id_OITM", "OITM", "ERP")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
    }

    public class OITM
    {
        public int Id_OITM { get; set; }
        public int Id_OTIB { get; set; }
        public int Id_OITC { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public bool VATLiable { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Active { get; set; }
    }
}
