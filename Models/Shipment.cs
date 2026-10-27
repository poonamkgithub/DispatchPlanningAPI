using System.ComponentModel.DataAnnotations;

namespace DispatchPlanningApi.Models;

public class Shipment
{
    public int Id { get; set; }
    [Required, MaxLength(40)] public string TrackingNumber { get; set; } = "";
    [Required, MaxLength(40)] public string DispatchNumber { get; set; } = "";
    [MaxLength(80)] public string CarrierName { get; set; } = "";
    public DateTime? EstimatedArrivalDate { get; set; }
    public DateTime? ActualArrivalDate { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Created";
}
