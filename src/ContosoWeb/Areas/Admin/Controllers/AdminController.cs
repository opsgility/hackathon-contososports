using System.Web.Mvc;

namespace ContosoWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = AdminConstants.Role)]
    public abstract class AdminController : Controller
    {
    }
}