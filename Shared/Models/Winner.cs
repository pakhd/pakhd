#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakhd.Shared.Models
{
    public class Winner
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WinnerId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public int LotteryId { get; set; }
        public Lottery Lottery { get; set; }
        public int Place { get; set; }
        public string PakhdUserId { get; set; }
        public PakhdUser PakhdUser { get; set; }
    }
}
