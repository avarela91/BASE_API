using DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class PurcharseInvoiceContext : models<PurchaseDeliveryNotes>
    {

        public PurcharseInvoiceContext() : base("DocEntry", "", "OrdenRecibido")
        {

        }

        // funcion para insertar el log de creacion de facturas de proveedores
        public async Task SetLogInvoiceCreated(LogCreatePurchInvoices log)
        {
            //Insertar registro de factura de proveeedores recien insertada
            string data = ConvertModelToString(log);
            string query = $"EXEC SW_PI_SetLogCreatedInvoice {data}";
            int ret = await QueryRaw(query);
        }

        private string ConvertModelToString(LogCreatePurchInvoices model)
        {
            Type type = typeof(LogCreatePurchInvoices);
            PropertyInfo[] properties = type.GetProperties();

            string result = string.Empty;

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(model);

                if (value is string)
                {
                    string stringValue = value.ToString();
                    stringValue = stringValue.Replace("'", "''"); // Escapar las comillas simples dentro del valor string
                    result += $"'{stringValue}',";
                }
                else
                {
                    result += $"{value},";
                }
            }

            result = result.TrimEnd(','); // Eliminar la última coma

            return result;
        }

    }

    public class LogCreatePurchInvoices
    {
        public string EM_NumAtCard { get; set; }
        public int EM_DocNum { get; set;}
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public long FP_DocEntry { get; set; }

    }

   

}
