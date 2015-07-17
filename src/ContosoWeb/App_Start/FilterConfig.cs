using System.Web.Mvc;
using ContosoWeb.Utils;

namespace ContosoWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LayoutDataAttribute());
        }
    }
}