using System.Web.Mvc;
using TesteCamposDealer.Filters;

namespace TesteCamposDealer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalExceptionFilter());
        }
    }
}
