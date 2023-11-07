namespace WSS.API.Application.Models.ViewModels;

public class RevenueProfixRespponse
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public double? Revenue { get; set; }
    public double? Profix { get; set; }
}