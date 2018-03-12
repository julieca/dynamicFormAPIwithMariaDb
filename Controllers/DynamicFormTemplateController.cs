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
    public class DynamicFormTemplateController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly ILogger<DynamicFormTemplateController> _logger;
        private readonly IRepository<MsDynamicFormTemplates> _repo;
        private readonly ILoggerFactory _loggerPatient;

        public DynamicFormTemplateController(IRepository<MsDynamicFormTemplates> repo, IBusClient busClient, ILoggerFactory logger)
        {
            _busClient = busClient;
            _logger = logger.CreateLogger<DynamicFormTemplateController>();
            _loggerPatient = logger;
            _repo = repo;
        }

        [HttpGet]
        public JsonResult Get(string tenantId, string siteId, ulong? isActive)
        {
            ApiResult result = new ApiResult();

            IEnumerable<MsDynamicFormTemplates> _payload = new List<MsDynamicFormTemplates>();
            _payload = _repo.GetAll().Where(x => x.IsDeleted == 0);

            if (!string.IsNullOrEmpty(tenantId))
            {
                _payload = _payload.Where(x => x.TenantId.ToLower() == tenantId.ToLower());
            }
            if (!string.IsNullOrEmpty(siteId))
            {
                _payload = _payload.Where(x => x.SiteId.ToLower() == siteId.ToLower());
            }
            if (isActive.HasValue)
            {
                _payload = _payload.Where(x => x.IsActive == isActive);
            }
            _payload = _payload.OrderByDescending(x => x.AddDate).ToList();

            if (_payload.Any())
            {
                result.Status = true;
                result.Payload = _payload;
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
                return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter id cannot be null or empty.", Payload = null });
            }
            if (!Guid.TryParse(id, out Guid DynamicFormTemplateId))
            {
                return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter id must be in UUID/GUID format", Payload = null });
            }
            var _payload = _repo.Find(id);
            if (_payload == null)
            {
                return Json(new ApiResult() { Status = false, Code = ApiErrorCode.DATA_NOT_FOUND, Messages = "Data Not Found.", Payload = null });
            }

            result.Status = true;
            result.Payload = _payload;
            return Json(result, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Post([FromBody]MsDynamicFormTemplates model)
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
                    if (string.IsNullOrEmpty(model.AddBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter AddBy cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.AddBy, out Guid AddBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter AddBy must be in UUID/GUID format." });
                    }

                    var template = new MsDynamicFormTemplates()
                    {
                        DynamicFormTemplateId = Guid.NewGuid().ToString(),
                        TenantId = model.TenantId,
                        SiteId = model.SiteId,
                        DynamicFormTemplateName = model.DynamicFormTemplateName,
                        Template = model.Template,
                        AddDate = DateTime.Now,
                        AddBy = model.AddBy,
                        EditDate = DateTime.Now,
                        EditBy = model.AddBy,
                        IsActive = model.IsActive.HasValue ? model.IsActive.Value : 0,
                        IsDeleted = 0
                    };
                    var insertedData = _repo.Add(template);
                    return Json(new ApiResult() { Status = true, Payload = insertedData }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                }
                catch (Exception e)
                {
                    return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_ADD, Messages = e.Message });
                }
            }
            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameters cannot be null or empty." });
        }

        [HttpPut]
        public JsonResult Put([FromBody]MsDynamicFormTemplates model)
        {
            if (!string.IsNullOrEmpty(model?.DynamicFormTemplateId))
            {
                try
                {
                    if (!Guid.TryParse(model.DynamicFormTemplateId, out Guid DynamicFormTemplateId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter DynamicFormTemplateId must be in UUID/GUID format." });
                    }
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

                    if (model.IsActive != 1 && model.IsActive != 0)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter IsActive is invalid." });
                    }

                    if (string.IsNullOrEmpty(model.EditBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter EditBy cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.EditBy, out Guid EditBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter EditBy must be in UUID/GUID format." });
                    }

                    var template = _repo.GetAll().FirstOrDefault(x => x.DynamicFormTemplateId == model.DynamicFormTemplateId && x.IsDeleted == 0);
                    if (template == null)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = "DynamicFormTemplate data cannot be found." });
                    }

                    template.TenantId = model.TenantId;
                    template.SiteId = model.SiteId;
                    template.DynamicFormTemplateName = model.DynamicFormTemplateName;
                    template.Template = model.Template;
                    template.EditBy = model.EditBy;
                    template.EditDate = DateTime.Now;
                    template.IsActive = model.IsActive;
                    var updatedData = _repo.Update(template);
                    return Json(new ApiResult() { Status = true, Payload = updatedData }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                }
                catch (Exception e)
                {
                    return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = e.Message });
                }
            }
            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter DynamicFormTemplateId cannot be null or empty." });
        }

        [HttpPatch]
        public JsonResult Delete([FromBody]MsDynamicFormTemplates model)
        {
            if (!string.IsNullOrEmpty(model?.DynamicFormTemplateId))
            {
                try
                {
                    if (!Guid.TryParse(model.DynamicFormTemplateId, out Guid DynamicFormTemplateId))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter DynamicFormTemplateId must be in UUID/GUID format." });
                    }

                    if (string.IsNullOrEmpty(model.EditBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter EditBy cannot be null or empty." });
                    }
                    if (!Guid.TryParse(model.EditBy, out Guid EditBy))
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter EditBy must be in UUID/GUID format." });
                    }

                    var template = _repo.Find(model.DynamicFormTemplateId);
                    if (template == null)
                    {
                        return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = "DynamicFormTemplate data cannot be found." });
                    }

                    template.IsDeleted = 1;
                    template.EditBy = model.EditBy;
                    template.EditDate = DateTime.Now;
                    var updatedData = _repo.Update(template);
                    return Json(new ApiResult() { Status = true, Payload = updatedData }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                }
                catch (Exception e)
                {
                    return Json(new ApiResult() { Status = false, Code = ApiErrorCode.ERROR_UPDATE, Messages = e.Message });
                }
            }
            return Json(new ApiResult() { Status = false, Code = ApiErrorCode.INVALID_PARAMETER, Messages = "The parameter DynamicFormTemplateId cannot be null or empty." });
        }
    }
}