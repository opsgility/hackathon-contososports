using System.ComponentModel.DataAnnotations;

namespace Contoso.Models
{
    public class Player
    {
        [ScaffoldColumn(false)]
        public int PlayerId { get; set; }
        
        [Required]
        public string Name { get; set; }

        public int GoalsScored { get; set; }
    }
}
