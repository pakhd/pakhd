#nullable disable


namespace Pakhd.Shared.Models;

public class Currency
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CurrencyId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime ModifiedOn { get; set; } = DateTime.Now;

    [StringLength(25), Required]
    public string CurrencyCode { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string Color { get; set; }
}

