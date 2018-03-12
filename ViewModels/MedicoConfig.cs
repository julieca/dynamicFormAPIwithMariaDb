using System.Collections.Generic;

namespace Medico.Service.DynamicForm.ViewModels
{
    public class MedicoConfig
    {
        public string ApiUrl { set; get; }
        public string ApiKey { set; get; }
        public ApiModule ApiModule { set; get; }
        public Admission Admission { set; get; }
        public StaticData StaticData { set; get; }
    }

    public class ApiModule
    {
        public string Base { set; get; }
    }

    public class Admission
    {
        public Emergency Emergency { set; get; }
        public Emergency IPD { set; get; }
    }

    public class Emergency
    {
        public string LobCode { set; get; }
    }

    public class StaticData
    {
        public BedStatus BedStatus { set; get; }
    }

    public class BedStatus
    {
        public string Vacant { set; get; }
        public string Occupied { set; get; }
        public string ToBeCleaned { set; get; }
        public string NotInUse { set; get; }
    }
}
