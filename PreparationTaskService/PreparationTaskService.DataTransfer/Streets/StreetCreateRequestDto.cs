using PreparationTaskService.DataTransfer.Streets.Models;
using System.Collections.Generic;

namespace PreparationTaskService.DataTransfer.Streets;

public class StreetCreateRequestDto
{
    public required string Name { get; set; }
    public required List<StreetPoint> Points{ get; set; }
    public required int Capacity { get; set; }
}