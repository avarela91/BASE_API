using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.ComponentModel.DataAnnotations;

namespace ET.Objetos
{
    public class SaldoArticulosComprasContext : models<ItemsRequestModel>
    {
        public SaldoArticulosComprasContext() : base("Articulo", "ItemsRequestModel", "SAP")
        {

        }

        public List<ItemsRequestModel> pd_FERB_SaldoArticulosCompras(string fecha, string fecha2, string producto)
        {
            string query = "EXEC pd_FERB_SaldoArticulosCompras '"+ fecha +"','"+ fecha2 + "','"+ producto + "'";
            return SelectRaw<ItemsRequestModel>(query);
        }

    }

    public class ItemsRequestModel
    {
        public string Articulo { get; set; }
        public string Descripcion { get; set; }
        public decimal StockAlmacen { get; set; }
        public decimal Stock { get; set; }
        
        public decimal EntradaMercancia { get; set; }
        
        public decimal EntradaProducto { get; set; }
        
        public decimal DevolucionesCliente { get; set; }
        
        public decimal Facturas { get; set; }
        
        public decimal SalidaMercancia { get; set; }
        
        public decimal DevolucionMercancia { get; set; }
    }
}
