using PreparationTaskService.DataTransfer.Streets.Models;

namespace PreparationTaskService.DataTransfer.Streets;

public class StreetResponseDto
{
    public required StreetOperationStates State { get; set; }
    public string? Message { get; set; }
}