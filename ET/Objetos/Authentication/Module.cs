using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos.Authentication
{
    public class ContextModule:models<Module>
    {
        public ContextModule(): base("Id_Module", "Module", "Security")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
    }

    public class Module
    {
        public int Id_Module { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Active { get; set; }
    }
}
