using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakhd.Shared.Models
{
    public class Referral
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReferralId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        
        public string? PakhdUserId { get; set; }
        public PakhdUser PakhdUser { get; set; }


        [ForeignKey("ReferredToUser")]
        public string? ReferredToId { get; set; }
        public PakhdUser ReferredToUser { get; set; }

    }
}
