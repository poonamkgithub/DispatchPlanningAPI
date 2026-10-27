using DispatchPlanningApi.Data;
using DispatchPlanningApi.DTOs;
using DispatchPlanningApi.Models;
using DispatchPlanningApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DispatchPlanningApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DispatchPlansController : ControllerBase
{
    private readonly IDispatchPlanningService _service;
    private readonly DispatchDbContext _db;

    public DispatchPlansController(IDispatchPlanningService service, DispatchDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status,
        [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            return BadRequest(new { message = "'from' must be on or before 'to'." });
        return Ok(await _service.GetAllAsync(status, from, to));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _db.DispatchPlans.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound(new { message = "Dispatch plan not found." }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDispatchRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateDispatchStatusRequest request)
    {
        var allowed = new[] { "Planned", "Ready", "Dispatched", "Delivered", "Cancelled" };
        if (!allowed.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { message = $"Status must be one of: {string.Join(", ", allowed)}." });

        var plan = await _db.DispatchPlans.FirstOrDefaultAsync(x => x.Id == id);
        if (plan is null) return NotFound(new { message = "Dispatch plan not found." });
        if (plan.Status == "Delivered" || plan.Status == "Cancelled")
            return Conflict(new { message = "Delivered or cancelled dispatch plans cannot be changed." });

        plan.Status = allowed.First(x => x.Equals(request.Status, StringComparison.OrdinalIgnoreCase));
        if (plan.Status == "Dispatched") plan.ActualDispatchDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(plan);
    }

    [HttpPost("{id:int}/shipments")]
    public async Task<IActionResult> CreateShipment(int id, [FromBody] CreateShipmentRequest request)
    {
        var plan = await _db.DispatchPlans.FirstOrDefaultAsync(x => x.Id == id);
        if (plan is null) return NotFound(new { message = "Dispatch plan not found." });
        if (plan.Status != "Dispatched")
            return Conflict(new { message = "Mark the dispatch plan as Dispatched before creating a shipment." });

        if (await _db.Shipments.AnyAsync(x => x.TrackingNumber == request.TrackingNumber))
            return Conflict(new { message = "Tracking number already exists." });

        var shipment = new Shipment
        {
            TrackingNumber = request.TrackingNumber.Trim(),
            DispatchNumber = plan.DispatchNumber,
            CarrierName = request.CarrierName.Trim(),
            EstimatedArrivalDate = request.EstimatedArrivalDate,
            Status = "Created"
        };
        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync();
        return Ok(shipment);
    }

    [HttpGet("shipments")]
    public async Task<IActionResult> GetShipments() =>
        Ok(await _db.Shipments.AsNoTracking().OrderBy(x => x.EstimatedArrivalDate).ToListAsync());
}

public class UpdateDispatchStatusRequest
{
    public string Status { get; set; } = "";
}

public class CreateShipmentRequest
{
    public string TrackingNumber { get; set; } = "";
    public string CarrierName { get; set; } = "";
    public DateTime? EstimatedArrivalDate { get; set; }
}
