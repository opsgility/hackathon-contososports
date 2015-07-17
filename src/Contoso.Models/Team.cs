using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Models
{
    public class Team
    {
        [ScaffoldColumn(false)]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AbbreviatedName { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string LogoImageSrc { get; set; }

        public List<Player> Players { get; set; }
    }
}
