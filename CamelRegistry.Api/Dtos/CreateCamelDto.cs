using System.ComponentModel.DataAnnotations;

namespace CamelRegistry.Api.Dtos;

public record  CreateCamelDto(
    [Required][StringLength(50)] string Name,
    [StringLength(15)]string Color,
    [Range(1,2)] int HumpCount,
    DateTime LastFed
);
