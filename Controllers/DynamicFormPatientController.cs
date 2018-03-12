using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Medico.Service.DynamicForm.ViewModels;
using Medico.Service.DynamicForm.Enums;
using Newtonsoft.Json;
using RawRabbit;
using Microsoft.Extensions.Logging;
using Medico.Service.DynamicForm.Interfaces;

namespace Medico.Service.DynamicForm.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DynamicFormPatientController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly ILogger<DynamicFormPatientController> _logger;
        private readonly IRepository<TrDynamicFormPatient> _repo;
        private readonly ILoggerFactory _loggerPatient;

        public DynamicFormPatientController(IRepository<TrDynamicFormPatient> repo, IBusClient busClient, ILoggerFactory logger)
        {
            _busClient = busClient;
            _logger = logger.CreateLogger<DynamicFormPatientController>();
            _loggerPatient = logger;
            _repo = repo;
        }

        [HttpGet]
        public JsonResult Get(string tenantId, string siteId, ulong? isActive, string msDynamicFormTemplateId = null,
            string patientId = null, string referenceId = null, string processType = null)
        {
            ApiResult result = new ApiResult();

            IEnumerable<TrDynamicFormPatient> _payload = new List<TrDynamicFormPatient>();
            _payload = _repo.GetAll().Where(x => x.IsDeleted == 0);

            if (!string.IsNullOrEmpty(tenantId))
            {
                _payload = _payload.Where(x => x.TenantId.ToLower() == tenantId.ToLower());
            }
            if (!string.IsNullOrEmpty(siteId))
            {
                _payload = _payload.Where(x => x.SiteId.ToLower() == siteId.ToLower());
            }
            if (!string.IsNullOrEmpty(msDynamicFormTemplateId))
            {
                _payload = _payload.Where(x => x.MsDynamicFormTemplateId.ToLower() == msDynamicFormTemplateId.ToLower());
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                _payload = _payload.Where(x => x.PatientId.ToLower() == patientId.ToLower());
            }
            if (!string.IsNullOrEmpty(referenceId))
            {
                _payload = _payload.Where(x => x.ReferenceId.ToLower() == referenceId.ToLower());
            }
            if (!string.IsNullOrEmpty(processType))
            {
                _payload = _payload.Where(x => x.ProcessType.ToLower() == processType.ToLower());
            }
            if (isActive.HasValue)
            {
                _payload = _payload.Where(x => x.IsActive == isActive);
            }
            _payload = _payload.OrderByDescending(x => x.AddDate).ToList();

            if (_payload.Any())
            {
                List<DynamicFormPatientViewModel> dataArr = new List<DynamicFormPatientViewModel>();
                foreach (var item in _payload)
                {
                    var data = dataArr.FirstOrDefault(x => item.MsDynamicFormTemplateId == x.MsDynamicFormTemplateId &&
                            item.PatientId == x.PatientId &&
                            item.TenantId == x.TenantId &&
                            item.SiteId == x.SiteId &&
                            item.ReferenceId == x.ReferenceId &&
                            item.ProcessType == x.ProcessType);

                    if (data == null)
                    {
                        data = new DynamicFormPatientViewModel()
                        {
                            MsDynamicFormTemplateId = item.MsDynamicFormTemplateId,
                            PatientId = item.PatientId,
                            TenantId = item.TenantId,
                            SiteId = item.SiteId,
                            ReferenceId = item.ReferenceId,
                            ProcessType = item.ProcessType,
                            AddDate = item.AddDate,
                            AddBy = item.AddBy,
                            EditDate = item.EditDate,
                            EditBy = item.EditBy,
                            IsActive = item.IsActive,
                            IsDeleted = item.IsDeleted,
                            KeyValue = new List<DynamicFormPatientKeyValueViewModel>()
                            {
                                new DynamicFormPatientKeyValueViewModel(){
                                    TrDynamicFormPatientId = item.TrDynamicFormPatientId,
                                    Key = item.Key,
                                    Value = item.Value
                                }
                            }
                        };
                        dataArr.Add(data);
                    }
                    else
                    {
                        data.KeyValue.Add(new DynamicFormPatientKeyValueViewModel()
                        {
                            TrDynamicFormPatientId = item.TrDynamicFormPatientId,
                            Key = item.Key,
                            Value = item.Value
                        });
                    }
                }
                
                result.Status = true;
                result.Payload = dataArr;
            }
            else
            {
                result.Status = true;
                result.Code = ApiErrorCode.DATA_NOT_FOUND;
                result.Messages = "No data found.";
            }
            return Json(result, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            ApiResult result = new ApiResult();

            if (string.IsNullOrEmpty(id))
            {
                return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter referenceId cannot be null or empty.", Payload = null });
            }
            if (!Guid.TryParse(id, out Guid ReferenceId))
            {
                return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter referenceId must be in UUID/GUID format", Payload = null });
            }
            var _payload = _repo.GetAll().Where(x => x.ReferenceId == id).ToList();
            if (_payload.Count == 0)
            {
                return Json(new ApiResult() { Status = false, Code = ApiErrorCode.DATA_NOT_FOUND, Messages = "Data Not Found.", Payload = null });
            }

            DynamicFormPatientViewModel data = null;
            foreach (var item in _payload)
            {
                if(data == null)
                {
                    data = new DynamicFormPatientViewModel() {
                        MsDynamicFormTemplateId = item.MsDynamicFormTemplateId,
                        PatientId = item.PatientId,
                        TenantId = item.TenantId,
                        SiteId = item.SiteId,
                        ReferenceId = item.ReferenceId,
                        ProcessType = item.ProcessType,
                        AddDate = item.AddDate,
                        AddBy = item.AddBy,
                        EditDate = item.EditDate,
                        EditBy = item.EditBy,
                        IsActive = item.IsActive,
                        IsDeleted = item.IsDeleted,
                        KeyValue = new List<DynamicFormPatientKeyValueViewModel>()
                        {
                            new DynamicFormPatientKeyValueViewModel(){
                                TrDynamicFormPatientId = item.TrDynamicFormPatientId,
                                Key = item.Key,
                                Value = item.Value
                            }
                        }
                    };
                }
                else
                {
                    if(item.MsDynamicFormTemplateId == data.MsDynamicFormTemplateId &&
                        item.PatientId == data.PatientId &&
                        item.TenantId == data.TenantId &&
                        item.SiteId == data.SiteId &&
                        item.ReferenceId == data.ReferenceId &&
                        item.ProcessType == data.ProcessType)
                    {
                        data.KeyValue.Add(new DynamicFormPatientKeyValueViewModel()
                        {
                            TrDynamicFormPatientId = item.TrDynamicFormPatientId,
                            Key = item.Key,
                            Value = item.Value
                        });
                    }
                }
            }

            result.Status = true;
            result.Payload = data;
            return Json(result, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Post([FromBody]DynamicFormPatientViewModel model)
        {
            if (model != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(model.TenantId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter TenantId cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.TenantId, out Guid TenantId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter TenantId must be in UUID/GUID format." });
                    }
                    if (string.IsNullOrEmpty(model.SiteId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter SiteId cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.SiteId, out Guid SiteId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter SiteId must be in UUID/GUID format." });
                    }
                    if (string.IsNullOrEmpty(model.MsDynamicFormTemplateId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter MsDynamicFormTemplateId cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.MsDynamicFormTemplateId, out Guid MsDynamicFormTemplateId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter MsDynamicFormTemplateId must be in UUID/GUID format." });
                    }
                    if (string.IsNullOrEmpty(model.AddBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter AddBy cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.AddBy, out Guid AddBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter AddBy must be in UUID/GUID format." });
                    }
                    if(model.KeyValue.Count == 0)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter KeyValue cannot be null or empty. " });
                    }

                    var check = true;
                    if (!string.IsNullOrEmpty(model.PatientId))
                    {
                        if (Guid.TryParse(model.PatientId, out Guid PatientId))
                        {
                            check = false;
                        }
                        else
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter PatientId must be in UUID/GUID format." });
                        }
                    }
                    if (!string.IsNullOrEmpty(model.ReferenceId))
                    {
                        if (Guid.TryParse(model.ReferenceId, out Guid ReferenceId))
                        {
                            check = false;
                        }
                        else
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter ReferenceId must be in UUID/GUID format." });
                        }
                    }
                    if (check)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter PatientId or ReferenceId cannot be null or empty." });
                    }

                    check = true;
                    if (model.PatientId != null && model.ReferenceId != null) {
                        var formPatientCheck = _repo.GetAll().Where(x => x.IsDeleted == 0 &&
                            x.MsDynamicFormTemplateId == model.MsDynamicFormTemplateId &&
                            x.PatientId == model.PatientId &&
                            x.ReferenceId == model.ReferenceId &&
                            x.TenantId == model.TenantId &&
                            x.SiteId == model.SiteId
                        ).ToList();
                        if(formPatientCheck.Count > 0)
                        {
                            check = false;
                        }
                    }
                    if (check)
                    {
                        var formPatient = new TrDynamicFormPatient()
                        {
                            MsDynamicFormTemplateId = model.MsDynamicFormTemplateId,
                            PatientId = model.PatientId,
                            TenantId = model.TenantId,
                            SiteId = model.SiteId,
                            ReferenceId = model.ReferenceId,
                            ProcessType = model.ProcessType,
                            AddDate = DateTime.Now,
                            AddBy = model.AddBy,
                            EditDate = DateTime.Now,
                            EditBy = model.AddBy,
                            IsActive = model.IsActive.HasValue ? model.IsActive.Value : 0,
                            IsDeleted = 0
                        };


                        foreach (var item in model.KeyValue)
                        {
                            formPatient.TrDynamicFormPatientId = Guid.NewGuid().ToString();
                            formPatient.Key = item.Key;
                            formPatient.Value = item.Value;
                            var insertedData = _repo.Add(formPatient);
                            item.TrDynamicFormPatientId = insertedData.TrDynamicFormPatientId;
                        }
                        model.AddDate = formPatient.AddDate;
                        model.EditDate = formPatient.EditDate;
                        model.EditBy = formPatient.EditBy;
                        model.IsActive = formPatient.IsActive;
                        model.IsDeleted = formPatient.IsDeleted;

                        return Json(new ApiResult() { Status = true, Payload = model }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                    }
                    else
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_ADD, Messages = "The data has been there." });
                    }
                    
                }
                catch (Exception e)
                {
                    return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_ADD, Messages = e.Message });
                }
            }
            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameters cannot be null or empty." });
        }
        [HttpPatch]
        public JsonResult Delete([FromBody]DynamicFormPatientViewModel model)
        {
            if (model != null)
            {
                try
                {
                    if(model.KeyValue.Count == 0)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = "The parameters cannot be null or empty." });
                    }

                    foreach (var item in model.KeyValue)
                    {
                        if (string.IsNullOrEmpty(item.TrDynamicFormPatientId))
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter TrDynamicFormPatientId cannot be null or empty." });
                        }
                        if (!Guid.TryParse(item.TrDynamicFormPatientId, out Guid TrDynamicFormPatientId))
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter TrDynamicFormPatientId must be in UUID/GUID format." });
                        }

                        var data = _repo.GetAll().FirstOrDefault(x => x.TrDynamicFormPatientId == item.TrDynamicFormPatientId);
                        if(data == null)
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = "Key Value cannot be found." });
                        }
                        data.EditBy = model.EditBy;
                        data.EditDate = DateTime.Now;
                        data.IsDeleted = 1;
                        _repo.Update(data);
                    }
                    return Json(new ApiResult() { Status = true, Payload = model }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                }
                catch (Exception e)
                {
                    return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = e.Message });
                }
            }
            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameters cannot be null or empty." });
        }


        [HttpPut]
        public JsonResult Put([FromBody]DynamicFormPatientViewModel model)
        {
            List<Tuple<TrDynamicFormPatient, TrDynamicFormPatient>> result = new List<Tuple<TrDynamicFormPatient, TrDynamicFormPatient>>();
            if (model != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(model.TenantId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter TenantId cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.TenantId, out Guid TenantId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter TenantId must be in UUID/GUID format." });
                    }
                    if (string.IsNullOrEmpty(model.SiteId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter SiteId cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.SiteId, out Guid SiteId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter SiteId must be in UUID/GUID format." });
                    }
                    if (string.IsNullOrEmpty(model.MsDynamicFormTemplateId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter MsDynamicFormTemplateId cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.MsDynamicFormTemplateId, out Guid MsDynamicFormTemplateId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter MsDynamicFormTemplateId must be in UUID/GUID format." });
                    }

                    var check = true;
                    if (!string.IsNullOrEmpty(model.PatientId))
                    {
                        if (Guid.TryParse(model.PatientId, out Guid PatientId))
                        {
                            check = false;
                        }
                        else
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter PatientId must be in UUID/GUID format." });
                        }
                    }
                    if (!string.IsNullOrEmpty(model.ReferenceId))
                    {
                        if (Guid.TryParse(model.ReferenceId, out Guid ReferenceId))
                        {
                            check = false;
                        }
                        else
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter ReferenceId must be in UUID/GUID format." });
                        }
                    }
                    if (check)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter PatientId or ReferenceId cannot be null or empty." });
                    }

                    if (string.IsNullOrEmpty(model.EditBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter EditBy cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.EditBy, out Guid EditBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter EditBy must be in UUID/GUID format." });
                    }

                    var template = _repo.GetAll().Where(x => x.MsDynamicFormTemplateId == model.MsDynamicFormTemplateId && 
                        x.SiteId == model.SiteId && x.TenantId== model.TenantId && x.IsDeleted == 0);
                    if (!String.IsNullOrEmpty(model.ReferenceId))
                    {
                        template = template.Where(x => x.ReferenceId == model.ReferenceId);
                    }
                    if (!String.IsNullOrEmpty(model.PatientId))
                    {
                        template = template.Where(x => x.PatientId == model.PatientId);

                    }
                    if (template.Count() == 0)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = "Dynamic Form Patient data cannot be found." });
                    }
                    foreach (var item in model.KeyValue)
                    {
                        var data = template.FirstOrDefault(x => x.TrDynamicFormPatientId == item.TrDynamicFormPatientId);
                        if (data == null)
                        {
                            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = "Key Value cannot be found." });
                        }
                        data.TenantId = model.TenantId;
                        data.SiteId = model.SiteId;
                        data.PatientId = model.PatientId;
                        data.ReferenceId = model.ReferenceId;
                        data.ProcessType = model.ProcessType;
                        data.Key = item.Key;
                        data.Value = item.Value;
                        data.EditBy = model.EditBy;
                        data.EditDate = DateTime.Now;
                        data.IsActive = model.IsActive;
                        var updatedData = _repo.Update(data);
                        result.Add(updatedData);
                    }

                    return Json(new ApiResult() { Status = true, Payload = model}, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                }
                catch (Exception e)
                {
                    return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = e.Message });
                }
            }
            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameters cannot be null or empty." });
        }
        
    }
}