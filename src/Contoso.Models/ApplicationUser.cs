using Microsoft.AspNet.Identity.EntityFramework;

namespace Contoso.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}