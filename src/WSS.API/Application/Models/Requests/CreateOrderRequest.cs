namespace WSS.API.Application.Models.Requests;

public class CreateOrderRequest
{
    public class CreateOrder
    {
        public Guid? CustomerId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? WeddingInformationId { get; set; }
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? ComboId { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalAmountRequest { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public int? StatusPayment { get; set; }
    }
    public class CreateOrderDetail
    {
        public Guid? OrderId { get; set; }
        public Guid? ServiceId { get; set; }
        public string? Address { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Price { get; set; }
        public double? Total { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
    }
    public class CreateWeddingInformation
    {
        public string? NameGroom { get; set; }
        public string? NameBride { get; set; }
        public string? NameBrideFather { get; set; }
        public string? NameBrideMother { get; set; }
        public string? NameGroomFather { get; set; }
        public string? NameGroomMother { get; set; }
        public DateTime? WeddingDay { get; set; }
        public string? ImageUrl { get; set; }
    }
}