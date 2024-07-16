USE SpendWiseDB;
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'SpendWise')
BEGIN
    EXEC('CREATE SCHEMA SpendWise')
END
GO