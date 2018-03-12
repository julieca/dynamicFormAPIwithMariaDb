using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.ViewModels
{
    public class DynamicFormPatientViewModel
    {
        public string MsDynamicFormTemplateId { get; set; }
        public string PatientId { get; set; }
        public string TenantId { get; set; }
        public string SiteId { get; set; }
        public string ReferenceId { get; set; }
        public string ProcessType { get; set; }
        public DateTime? AddDate { get; set; }
        public string AddBy { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditBy { get; set; }
        public ulong? IsDeleted { get; set; }
        public ulong? IsActive { get; set; }
        public List<DynamicFormPatientKeyValueViewModel> KeyValue { get; set; }
    }
}
