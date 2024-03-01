using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace ET.Objetos.Item
{
    public class ContextCategory:models<OITC>
    {
        public ContextCategory() : base("Id_OITC", "OITC", "ERP")
        {
            ActivateActiveRecordsOnly = true;
            ActiveRecordsInTransactionOnly = true;
        }
        public List<CategoryIndex> getCategoryIndex()
        {
            string query = "select c.Id_OITC, c.name categoria,d.Id_OTIB, d.Name departamento from OITC c inner join OTIB d on c.Id_OTIB=d.Id_OTIB where c.Active=1 and d.Active=1";
            return SelectRaw<CategoryIndex>(query);
        }

    }

    public class OITC
    {
        public int Id_OITC { get; set; }
        public int Id_OTIB { get; set; }
        public string Name { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string Modify_User { get; set; }
        public DateTime Modify_Date { get; set; }
        public bool Active { get; set; }
    }

    public class CategoryIndex
    {
        public int Id_OITC { get; set; }
        public string Category { get; set; }
        public int Id_OTIB { get; set; }
        public string Department { get; set; }
    }
}
