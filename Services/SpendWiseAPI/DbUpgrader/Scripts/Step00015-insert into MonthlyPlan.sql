USE SpendWiseDB;
GO
IF  EXISTS (SELECT * FROM sys.tables WHERE name = 'MonthlyPlan' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN

INSERT INTO SpendWise.MonthlyPlan (user_id, plan_id, date, totalAmount, amountSpent, priceByCategory, spentOfCategory)
VALUES (
    (SELECT user_id FROM SpendWise.Users WHERE name = 'Sabina Barila'),
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Student plan'),
    '2024-07-01',
    1000,
    500,
    'Example price breakdown',
    'Example spending breakdown'
);

INSERT INTO SpendWise.MonthlyPlan (user_id, plan_id, date, totalAmount, amountSpent, priceByCategory, spentOfCategory)
VALUES (
    (SELECT user_id FROM SpendWise.Users WHERE name = 'Cirja Ioan'),
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Family Plan'),
    '2024-07-01', 
    1500, 
    700, 
    'Another example price breakdown', 
    'Another example spending breakdown' 
);


INSERT INTO SpendWise.MonthlyPlan (user_id, plan_id, date, totalAmount, amountSpent, priceByCategory, spentOfCategory)
VALUES (
    (SELECT user_id FROM SpendWise.Users WHERE name = 'Cazamir Andrei'),
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Travel Plan'),
    '2024-07-01', 
    1200, 
    600, 
    'Third example price breakdown', 
    'Third example spending breakdown' 
);



END