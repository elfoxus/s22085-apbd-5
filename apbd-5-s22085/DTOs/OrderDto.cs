namespace apbd_5_s22085.DTOs;

public class OrderDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
}