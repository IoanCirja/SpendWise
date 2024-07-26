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


INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Travel Plan'),
    1200, 
    '80,100,100,50,75,50,85,100,175,85,125,75,50,50', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'StartUp Plan'),
    1300, 
    '200,100,100,150,75,75,85,115,175,75,150', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Wedding Plan'),
    1300, 
    '200,100,100,150,75,75,85,115,175,75,150', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Pet care Plan'),
    1425, 
    '200,100,100,150,75,75,85,115,175,75,150,125', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Family Plan'),
    1300, 
    '200,150,125,150,150,100,100,150,175', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Medical expensive plan'),
    1300, 
    '200,100,100,150,75,75,85,115,175,75,150', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Student Plan'),
    1300, 
    '250,275,100,200,150,75,100,150', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Emergency Fund Plan'),
    1200, 
    '200,100,100,150,100,100,85,115,175,75', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Holiday shopping Plan'),
    1200, 
    '250,150,100,150,75,75,85,115,175,75', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Fitness & Wellness Plan'),
    1300, 
    '200,100,100,150,75,75,85,115,175,75,150', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Default plan'),
    1500, 
    '750,450,300', 
    'Demo',
    '2500-10-10',
    0
);

INSERT INTO SpendWise.MonthlyPlan (plan_id, totalAmount, priceByCategory, status, date, amountSpent)
VALUES (
    (SELECT plan_id FROM SpendWise.PlanDetails WHERE name = 'Retirement Plan'),
    1250, 
    '200,100,100,150,75,75,85,140,175,150', 
    'Demo',
    '2500-10-10',
    0
);

END
GO