#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum LotteryType
{
    Regular,
    Crypto
}

namespace Pakhd.Shared.Models
{
    public class Lottery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LotteryId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public string? ImageUrl { get; set; }
        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal TicketPrice { get; set; }

        [Column(TypeName = "decimal(32, 6)")]
        public decimal Prize { get; set; }

        public bool IsClosed { get; set; }

        public List<Item> Items { get; set; }

        
    }
}
