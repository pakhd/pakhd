#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakhd.Shared.Models
{
    public enum SalesOrderStatus
    {
        Open,
        Released,
        Posted
    }
    public class SalesOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SalesOrderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public DateTime PostingDate { get; set; }
        public string OrderNo { get; set; }
        public string PakhdUserId { get; set; }
        public PakhdUser PakhdUser { get; set; }
        public SalesOrderStatus Status { get; set; }


        public List<SalesLine> SalesLines { get; set; }
    }
}
