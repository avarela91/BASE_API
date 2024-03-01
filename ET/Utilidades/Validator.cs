using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Utilidades
{
    interface IValidator
    {
        Tuple<bool, List<ValidationResult>> ValidateObject(IValidable objToValidate);
    }

    public interface IValidable
    {
        bool IsValid { get; set; }
    }
    public sealed class Validator : IValidator
    {
        private Validator()
        {

        }
        public static Validator validator = new Validator();

        public static Tuple<bool, List<ValidationResult>> Validate(IValidable objToValidate)
        {
            if (validator == null)
                validator = new Validator();
            return validator.ValidateObject(objToValidate);
        }

        public Tuple<bool, List<ValidationResult>> ValidateObject(IValidable objToValidate)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(objToValidate, null, null);
            var obj = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(objToValidate, context, results, true);

            return new Tuple<bool, List<ValidationResult>>(obj, results);
        }

    }
}
