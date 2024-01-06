using System.ComponentModel.DataAnnotations;

namespace apbd_5_s22085.DTOs;

public record ProductInWarehouseRequestDto(
    [Required]
    int IdProduct,
    [Required]
    int IdWarehouse,
    [Required]
    [Range(1, int.MaxValue)]
    int Amount,
    [Required]
    DateTime CreatedAt
    );