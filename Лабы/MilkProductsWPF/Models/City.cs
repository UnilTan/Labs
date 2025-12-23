using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkProductsWPF.Models
{
    [Table("City")]
    public partial class City
    {
        public City()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int CityId { get; set; }

        [Required]
        [StringLength(100)]
        public string CityName { get; set; } = null!;

        [StringLength(100)]
        public string? Region { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        public int Population { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}