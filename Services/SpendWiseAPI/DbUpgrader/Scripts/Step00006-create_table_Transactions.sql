USE SpendWiseDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Transactions' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
    CREATE TABLE SpendWise.Transactions(
        transaction_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        name NVARCHAR(40) NOT NULL,
		monthlyPlan_id UNIQUEIDENTIFIER,
		date DATE NOT NULL,
		category NVARCHAR(30) NOT NULL,
		amount INT NOT NULL,
		FOREIGN KEY (monthlyPlan_id) REFERENCES SpendWise.MonthlyPlan(monthlyPlan_id)
    );
END
GO