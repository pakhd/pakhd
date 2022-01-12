#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakhd.Shared.Models
{
    public enum ReservationStatus
    {
        Active,
        Sold,
        Removed
    }
    public class Reservation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public string PakhdUserId { get; set; }
        public PakhdUser PakhdUser { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
