namespace WSS.API.Application.Models.ViewModels;

public class DashboardResponse
{
    public List<Dictionary<string, double?>>? TotalRevenueMonth { get; set; }
    public List<Dictionary<string, double?>>? TotalOrderMonth { get; set; }
    public List<Dictionary<string, double?>>? TotalRevenueYear { get; set; }
    public List<Dictionary<string, int>>? ServiceQuantity { get; set; }
    public List<Dictionary<string, int>>? ServiceOrder { get; set; }
    public List<Dictionary<string, float>>? ServiceFeedback { get; set; }
    public List<Dictionary<string, int>>? Partner { get; set; }
    public List<Dictionary<string, int>>? Staff { get; set; }
    public List<Dictionary<string, double?>>? PartnerPayment { get; set; }
    public int? TotalFeedback { get; set; }
    public int? NegativeFeedback { get; set; }
    public int? TotalStaff { get; set; }
    public int? TotalPartner { get; set; }
    public int? TaskInProgress { get; set; }
    public int? TaskToDo { get; set; }
    public int? TotalTask { get; set; }
    public int? TaskDone { get; set; }
    public int? TotalOrderDone { get; set; }
    
    public int? TotalOrder { get; set; }
    
}