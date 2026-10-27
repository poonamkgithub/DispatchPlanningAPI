using System.ComponentModel.DataAnnotations;

namespace DispatchPlanningApi.Models;

public class DispatchPlan
{
    public int Id { get; set; }
    [Required, MaxLength(40)] public string DispatchNumber { get; set; } = "";
    [Required, MaxLength(50)] public string OrderNumber { get; set; } = "";
    [Required, MaxLength(50)] public string ProductCode { get; set; } = "";
    [Required, MaxLength(50)] public string SourceLocation { get; set; } = "";
    [Required, MaxLength(50)] public string DestinationLocation { get; set; } = "";
    [Range(1, int.MaxValue)] public int PlannedQuantity { get; set; }
    public DateTime PlannedDispatchDate { get; set; }
    public DateTime? ActualDispatchDate { get; set; }
    [MaxLength(30)] public string Priority { get; set; } = "Normal";
    [MaxLength(30)] public string Status { get; set; } = "Planned";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
