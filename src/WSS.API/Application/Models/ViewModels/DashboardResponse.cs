namespace WSS.API.Application.Models.ViewModels;

public class DashboardResponse
{
    public List<Dictionary<string, double?>>? TotalRevenue { get; set; }
    public List<Dictionary<string, int>>? ServiceQuantity { get; set; }
    public List<Dictionary<string, int>>? ServiceOrder { get; set; }
    public List<Dictionary<string, float>>? ServiceFeedback { get; set; }
    public List<Dictionary<string, int>>? Partner { get; set; }
    public List<Dictionary<string, double?>>? PartnerPayment { get; set; }
}