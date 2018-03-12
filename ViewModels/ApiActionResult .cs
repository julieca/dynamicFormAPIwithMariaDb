using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Medico.Service.DynamicForm.ViewModels
{
    public class ApiActionResult : IActionResult
    {
        private readonly ApiResult _result;

        public ApiActionResult(ApiResult result)
        {
            _result = result;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
