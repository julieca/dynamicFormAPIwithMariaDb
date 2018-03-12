using System;

namespace Medico.Service.DynamicForm
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class DoctorException : Exception
    {
        public DoctorException()
        { }

        public DoctorException(string message)
            : base(message)
        { }

        public DoctorException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
