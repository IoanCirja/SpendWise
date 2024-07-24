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
    '150,100,100,100,200,100,150,100',
    '75,25,50,50,100,50,75,75'
);

INSERT INTO SpendWise.MonthlyPlan (user_id, plan_id, date, totalAmount, amountSpent, priceByCategory, spentOfCategory)
VALUES (
    (SELECT user_id FROM SpendWise.Users WHERE name = 'Cirja Ioan'),
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Family Plan'),
    '2024-07-01', 
    1500, 
    700, 
    '150,200,150,150,200,150,150,200,150', 
    '75,150,50,75,50,50,50,150,50' 
);


INSERT INTO SpendWise.MonthlyPlan (user_id, plan_id, date, totalAmount, amountSpent, priceByCategory, spentOfCategory)
VALUES (
    (SELECT user_id FROM SpendWise.Users WHERE name = 'Cazamir Andrei'),
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Travel Plan'),
    '2024-07-01', 
    1200, 
    600, 
    '80,100,100,50,75,50,85,100,175,85,125,75,50,50', 
    '40,50,50,25,37.5,25,42.5,50,87.5,42.5,62.5,37.5,25,25' 
);



END
GO