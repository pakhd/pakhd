#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakhd.Shared.Models
{
    public enum ItemStatus
    {
        Free,
        Reserved,
        Sold
    }

    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public int LotteryId { get; set; }
        public Lottery Lottery { get; set; }
        public int Number { get; set; }
        public ItemStatus Status { get; set; }

    }
}
