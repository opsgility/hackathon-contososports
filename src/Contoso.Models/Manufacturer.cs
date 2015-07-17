using System.ComponentModel.DataAnnotations;

namespace Contoso.Models
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}