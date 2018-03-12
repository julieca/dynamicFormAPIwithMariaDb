using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.Filter
{
    public class ActiveAttribute : ValidationAttribute
    {
        private string _name;

        public ActiveAttribute(string name)
        {
            _name = name;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((ulong?)value != 1 && (ulong?)value != 0)
            {
                return new ValidationResult("The parameter " + _name + " is invalid.");
            }
            return ValidationResult.Success;
        }
    }
}
