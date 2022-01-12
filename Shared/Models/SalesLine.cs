#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakhd.Shared.Models
{
    public class SalesLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesLineId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public int LineNo { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal Price { get; set; }
    }
}
