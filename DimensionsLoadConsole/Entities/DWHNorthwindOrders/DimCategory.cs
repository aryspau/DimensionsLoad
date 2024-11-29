
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DimensionsLoadConsole.Entities.DWHNorthwindOrders
{

    [Table("DimCategories")]
    public partial class DimCategory
    {
        [Key]
        public int CategoryKey { get; set; }

        public string? CategoryName { get; set; }

        public int CategoryId { get; set; }
    }
}
