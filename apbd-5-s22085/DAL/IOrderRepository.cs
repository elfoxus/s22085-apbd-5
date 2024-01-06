using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public interface IOrderRepository
{
    Task FindOrderByProductIdAndAmountBeforeAsync(FindOrderDto findOrderDto);
    
    Task UpdateFulfillmentDateInOrderAsync(int orderId, DateTime fulfillmentDate);
}