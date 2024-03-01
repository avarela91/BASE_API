using System;

namespace DAL
{
    public class MyCustomAttribute : Attribute
    {
        public bool ActivateActiveRecordsOnly { get; set; }
        public bool ActiveRecordsInTransactionOnly { get; set; }
    }

    public class IgnoreAtributoBasedeDatos : Attribute
    {
    }
}