using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Contoso.Models
{
    public class CartItem
    {
        public CartItem()
        {
            DateCreated = DateTime.Now;
        }

        [Key]
        public int CartItemId { get; set; }

        [Required]
        public string CartId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}