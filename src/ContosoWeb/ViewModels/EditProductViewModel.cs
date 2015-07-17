using System.Collections.Generic;
using System.Web.Mvc;
using Contoso.Models;

namespace ContosoWeb.ViewModels
{
    public class EditProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}