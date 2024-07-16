USE SpendWiseDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MonthlyPlan' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
    CREATE TABLE SpendWise.MonthlyPlan(
        monthlyPlan_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
		user_id UNIQUEIDENTIFIER,
		plan_id UNIQUEIDENTIFIER,
		date DATE NOT NULL,
		totalAmount INT NOT NULL,
		amountSpent INT NOT NULL,
		priceByCategory NVARCHAR(200),
		spentOfCategory NVARCHAR(200),
		FOREIGN KEY (user_id) REFERENCES SpendWise.Users(user_id),
		FOREIGN KEY (plan_id) REFERENCES SpendWise.PlanDetails(plan_id)
    );
END
GO
