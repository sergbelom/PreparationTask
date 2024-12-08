using PreparationTaskService.DataTransfer.Streets.Models;

namespace PreparationTaskService.DataTransfer.Streets;

public class StreetAddPointRequestDto
{
    public required string Name { get; set; }
    public required StreetPoint NewPoint { get; set; }
}