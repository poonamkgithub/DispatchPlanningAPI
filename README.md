# Dispatch Planning API — ASP.NET Core 8 + SQL Server

Sample project for learning/interview demonstration.

## Features
- Create and view dispatch plans
- Filter plans by status and planned date range
- Update dispatch status
- Create shipments after dispatch
- SQL Server schema and sample data
- Swagger UI

## Business rules in this demo
1. Dispatch number must be unique.
2. Source and destination must differ.
3. Planned dispatch date cannot be in the past.
4. Status transitions are restricted to the listed statuses; Delivered/Cancelled plans cannot be changed.
5. A shipment can be created only after a plan is marked Dispatched.
6. Tracking numbers must be unique.

This is a starter demo. Real dispatch planning usually also checks ATP/allocation, carrier capacity, vehicle capacity, route, warehouse cut-off, loading sequence, delivery windows, and transaction/concurrency rules.

## Run
1. Open `DispatchPlanningApi.csproj` in Visual Studio 2022.
2. Execute `Database/DispatchPlanningDb.sql` in SSMS.
3. If needed, update `ConnectionStrings:DefaultConnection` in `appsettings.json`.
4. Restore NuGet packages and press F5.
5. Use the Swagger URL opened by Visual Studio.

## Example endpoints
- `GET /api/dispatchplans`
- `GET /api/dispatchplans?status=Planned&from=2026-10-28&to=2026-11-10`
- `GET /api/dispatchplans/1`
- `POST /api/dispatchplans`
- `PATCH /api/dispatchplans/1/status`
- `POST /api/dispatchplans/1/shipments`
- `GET /api/dispatchplans/shipments`

### POST /api/dispatchplans sample
```json
{
  "dispatchNumber": "DSP-1002",
  "orderNumber": "ORD-1002",
  "productCode": "SKU-2001",
  "sourceLocation": "MUM-WH1",
  "destinationLocation": "NASHIK-DC",
  "plannedQuantity": 40,
  "plannedDispatchDate": "2026-10-30T09:00:00",
  "priority": "High"
}
```

### PATCH /api/dispatchplans/1/status sample
```json
{ "status": "Dispatched" }
```
