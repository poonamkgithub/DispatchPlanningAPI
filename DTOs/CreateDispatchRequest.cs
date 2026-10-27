using System.ComponentModel.DataAnnotations;

namespace DispatchPlanningApi.DTOs;

public class CreateDispatchRequest
{
    [Required] public string DispatchNumber { get; set; } = "";
    [Required] public string OrderNumber { get; set; } = "";
    [Required] public string ProductCode { get; set; } = "";
    [Required] public string SourceLocation { get; set; } = "";
    [Required] public string DestinationLocation { get; set; } = "";
    [Range(1, int.MaxValue)] public int PlannedQuantity { get; set; }
    public DateTime PlannedDispatchDate { get; set; }
    public string Priority { get; set; } = "Normal";
}
