using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Objetos
{
    public class ResponseData
    {
        public string status { get; set; }
        public string msj { get; set; }
        public int CodeError { get; set; }
    }
    public class ResponseDataLoginApp
    {
        public string status { get; set; }
        public string msj { get; set; }
        public int CodeError { get; set; }
        public List<PermisoUser> permissions { get; set; }
    }
}
