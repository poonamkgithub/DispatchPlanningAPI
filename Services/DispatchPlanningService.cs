using DispatchPlanningApi.Data;
using DispatchPlanningApi.DTOs;
using DispatchPlanningApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DispatchPlanningApi.Services;

public class DispatchPlanningService : IDispatchPlanningService
{
    private readonly DispatchDbContext _db;
    public DispatchPlanningService(DispatchDbContext db) => _db = db;

    public async Task<DispatchPlan> CreateAsync(CreateDispatchRequest request)
    {
        if (request.PlannedDispatchDate.Date < DateTime.Today)
            throw new ArgumentException("Planned dispatch date cannot be in the past.");

        var duplicate = await _db.DispatchPlans.AnyAsync(x => x.DispatchNumber == request.DispatchNumber);
        if (duplicate) throw new InvalidOperationException("Dispatch number already exists.");

        if (request.SourceLocation.Trim().Equals(request.DestinationLocation.Trim(),
                StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Source and destination locations must be different.");

        var plan = new DispatchPlan
        {
            DispatchNumber = request.DispatchNumber.Trim(),
            OrderNumber = request.OrderNumber.Trim(),
            ProductCode = request.ProductCode.Trim(),
            SourceLocation = request.SourceLocation.Trim(),
            DestinationLocation = request.DestinationLocation.Trim(),
            PlannedQuantity = request.PlannedQuantity,
            PlannedDispatchDate = request.PlannedDispatchDate,
            Priority = string.IsNullOrWhiteSpace(request.Priority) ? "Normal" : request.Priority.Trim(),
            Status = "Planned",
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.DispatchPlans.Add(plan);
        await _db.SaveChangesAsync();
        return plan;
    }

    public async Task<IReadOnlyList<DispatchPlan>> GetAllAsync(string? status, DateTime? from, DateTime? to)
    {
        var query = _db.DispatchPlans.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(x => x.Status == status);
        if (from.HasValue)
            query = query.Where(x => x.PlannedDispatchDate >= from.Value);
        if (to.HasValue)
            query = query.Where(x => x.PlannedDispatchDate < to.Value.Date.AddDays(1));
        return await query.OrderBy(x => x.PlannedDispatchDate)
            .ThenByDescending(x => x.Priority == "High")
            .ToListAsync();
    }
}
