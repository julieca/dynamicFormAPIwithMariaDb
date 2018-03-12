using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.Filter
{
    public class UuidAttribute : ValidationAttribute
    {
        private string _name;

        public UuidAttribute(string name)
        {
            _name = name;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return new ValidationResult("The parameter " + _name + " cannot be null or empty.");
            }
            if (!Guid.TryParse((string)value, out Guid uuid))
            {
                return new ValidationResult("The parameter "+ _name + " must be in UUID/GUID format.");
            }
            return ValidationResult.Success;
        }
    }
}
