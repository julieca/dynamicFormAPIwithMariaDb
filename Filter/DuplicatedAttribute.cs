using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.Filter
{
    public class DuplicatedAttribute : ValidationAttribute
    {
        private string _name;

        public DuplicatedAttribute(string name)
        {
            _name = name;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return new ValidationResult("The parameter " + _name + " cannot be null or empty.");
            }
            return ValidationResult.Success;
        }
    }
}
