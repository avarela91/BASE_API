using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class OCRDContext : models<OCRD>
    {
        public OCRDContext() : base("CardCode", "OCRD", "SAP")
        {

        }

        //Valida si existe un item
        public bool SW_GetVendor(string cardcode)
        {
            bool existe = false;
            string query = "SELECT CardCode, CardName FROM OCRD WHERE /*validfor = 'Y' AND*/ CardCode = '" + cardcode + "'";
            var vendor = SelectRaw<OCRD>(query);

            if (vendor.Count > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string SW_GetNameVendor(string cardcode)
        {
            string namevendor = "";
            string query = "SELECT CardCode, CardName FROM OCRD WHERE /*validfor = 'Y' AND*/ CardCode = '" + cardcode + "'";
            var vendor = SelectRaw<OCRD>(query);

            if (vendor.Count > 0)
            {
                namevendor = vendor[0].CardName;
            }

            return namevendor;
        }
    }

    public class OCRD
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }

}
