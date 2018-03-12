namespace Medico.Service.DynamicForm.ViewModels
{
    public class ApiResult
    {
        public bool Status { get; set; } = false;
        public string Code { get; set; } = null;
        public string Messages { get; set; } = null;
        public object Payload { get; set; } = null;

    }
}
