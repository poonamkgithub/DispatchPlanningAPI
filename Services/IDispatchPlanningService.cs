using DispatchPlanningApi.DTOs;
using DispatchPlanningApi.Models;

namespace DispatchPlanningApi.Services;

public interface IDispatchPlanningService
{
    Task<DispatchPlan> CreateAsync(CreateDispatchRequest request);
    Task<IReadOnlyList<DispatchPlan>> GetAllAsync(string? status, DateTime? from, DateTime? to);
}
