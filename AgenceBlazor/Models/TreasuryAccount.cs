namespace AgenceBlazor.Models;

public class TreasuryAccount
{
    public int Id { get; set; }
    public string Name { get; set; } = ""; // "general" or "agency_owner"
    public string DisplayName { get; set; } = ""; // "الخزينة العامة" or "خزينة صاحب الوكالة"
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}