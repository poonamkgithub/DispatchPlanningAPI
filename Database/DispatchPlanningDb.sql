CREATE DATABASE DispatchPlanningDb;
GO
USE DispatchPlanningDb;
GO

CREATE TABLE DispatchPlans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DispatchNumber NVARCHAR(40) NOT NULL UNIQUE,
    OrderNumber NVARCHAR(50) NOT NULL,
    ProductCode NVARCHAR(50) NOT NULL,
    SourceLocation NVARCHAR(50) NOT NULL,
    DestinationLocation NVARCHAR(50) NOT NULL,
    PlannedQuantity INT NOT NULL CHECK (PlannedQuantity > 0),
    PlannedDispatchDate DATETIME2 NOT NULL,
    ActualDispatchDate DATETIME2 NULL,
    Priority NVARCHAR(30) NOT NULL DEFAULT 'Normal',
    Status NVARCHAR(30) NOT NULL DEFAULT 'Planned',
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Shipments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TrackingNumber NVARCHAR(40) NOT NULL UNIQUE,
    DispatchNumber NVARCHAR(40) NOT NULL,
    CarrierName NVARCHAR(80) NOT NULL,
    EstimatedArrivalDate DATETIME2 NULL,
    ActualArrivalDate DATETIME2 NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Created'
);

INSERT INTO DispatchPlans
(DispatchNumber, OrderNumber, ProductCode, SourceLocation, DestinationLocation,
 PlannedQuantity, PlannedDispatchDate, Priority, Status)
VALUES
('DSP-1001', 'ORD-1001', 'SKU-1001', 'MUM-WH1', 'PUNE-DC', 25,
 DATEADD(day, 1, CAST(GETDATE() AS date)), 'High', 'Planned');
GO
