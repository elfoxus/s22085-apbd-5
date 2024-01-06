using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public interface IOrderRepository
{
    Task<OrderDto?> FindOrderByProductIdAndAmountAndDateBeforeAsync(FindOrderDto findOrderDto);
}