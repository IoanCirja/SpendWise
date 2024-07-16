USE SpendWiseDB;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PlanDetails' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
    ALTER TABLE SpendWise.PlanDetails
    ADD created_by NVARCHAR(255);
END