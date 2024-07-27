USE SpendWiseDB;
GO
IF  EXISTS (SELECT * FROM sys.tables WHERE name = 'Transactions' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
  INSERT INTO SpendWise.Transactions (name, monthlyPlan_id, date, category, amount)
VALUES 
    ('Food', (SELECT monthlyPlan_id FROM SpendWise.MonthlyPlan WHERE user_id = (SELECT user_id FROM SpendWise.Users WHERE name='Sabina Barila')), '2024-07-01 14:30:00', 'Food', 150),
    ('Rent', (SELECT monthlyPlan_id FROM SpendWise.MonthlyPlan WHERE user_id = (SELECT user_id FROM SpendWise.Users WHERE name='Sabina Barila')), '2024-07-02 15:47:19', 'Utilities', 75),
    ('Utilities', (SELECT monthlyPlan_id FROM SpendWise.MonthlyPlan WHERE user_id = (SELECT user_id FROM SpendWise.Users WHERE name='Cirja Ioan')), '2024-07-03 08:57:11', 'Health', 50),
    ('Entertainment', (SELECT monthlyPlan_id FROM SpendWise.MonthlyPlan WHERE user_id = (SELECT user_id FROM SpendWise.Users WHERE name='Cirja Ioan')), '2024-07-04 20:01:01', 'Utilities', 60),
    ('Clothes', (SELECT monthlyPlan_id FROM SpendWise.MonthlyPlan WHERE user_id = (SELECT user_id FROM SpendWise.Users WHERE name='Cazamir Andrei')), '2024-07-05 23:39:58', 'Entertainment', 80),
    ('Transportation', (SELECT monthlyPlan_id FROM SpendWise.MonthlyPlan WHERE user_id = (SELECT user_id FROM SpendWise.Users WHERE name='Cazamir Andrei')), '2024-07-06 04:21:13', 'Transport', 200),
    ('Miscellaneous', (SELECT monthlyPlan_id FROM SpendWise.MonthlyPlan WHERE user_id = (SELECT user_id FROM SpendWise.Users WHERE name='Cazamir Andrei')), '2024-07-06 17:17:17', 'Transport', 200);
END
GO


