namespace apbd_5_s22085.DTOs;

public record FindOrderDto(
    int productId,
    int amount,
    DateTime createdAt);