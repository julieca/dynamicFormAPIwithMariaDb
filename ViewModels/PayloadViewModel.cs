using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.ViewModels
{
    public class PayloadViewModel<T>
    {
        public bool Status { get; set; }
        public string Error { get; set; }
        public string Messages { get; set; }
        public ICollection<T> Payload { get; set; }
    }
}
