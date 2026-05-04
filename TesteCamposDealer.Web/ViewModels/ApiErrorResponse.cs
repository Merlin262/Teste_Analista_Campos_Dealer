using System.Collections.Generic;

namespace TesteCamposDealer.Web.ViewModels
{
    public class ApiErrorResponse
    {
        public string message { get; set; }
    }

    public class ApiValidationErrorResponse
    {
        public Dictionary<string, IEnumerable<string>> errors { get; set; }
    }
}
