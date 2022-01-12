#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Pakhd.Shared.Services;

namespace Pakhd.Shared.Models;


public class PakhdUser : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
    public int MaxReservation { get; set; }

    public List<Reservation> Reservations { get; set;}

    public List<SalesOrder> SalesOrders { get; set; }
    public string ReferralCode { get; set; }

}

