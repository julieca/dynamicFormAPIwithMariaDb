using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.Filter
{
    public class NullOrEmptyListAttribute : ValidationAttribute
    {
        private string _name;

        public NullOrEmptyListAttribute(string name)
        {
            _name = name;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList;
            if (list == null)
            {
                return new ValidationResult("The parameter " + _name + " cannot be null or empty.");
            }
            if (list.Count == 0)
            {
                return new ValidationResult("The parameter " + _name + " cannot be null or empty.");
            }
            return ValidationResult.Success;
        }
    }
}
