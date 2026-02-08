using System.ComponentModel.DataAnnotations;

namespace CamelRegistry.Api.Dtos;

public record CamelDto(
    int Id,
    string Name,
    string Color,
    int HumpCount,
    DateTime LastFed
);
