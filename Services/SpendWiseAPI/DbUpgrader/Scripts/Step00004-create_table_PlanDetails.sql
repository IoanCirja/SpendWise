USE SpendWiseDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PlanDetails' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
    CREATE TABLE SpendWise.PlanDetails(
        plan_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        name NVARCHAR(40) NOT NULL,
		description NVARCHAR(1000) NOT NULL,
		noCategory INT NOT NULL,
        category NVARCHAR(200) NOT NULL,
        isActive BIT,
        created_by UNIQUEIDENTIFIER,
        FOREIGN KEY (created_by) REFERENCES SpendWise.Users(user_id)
    );
END
GO
