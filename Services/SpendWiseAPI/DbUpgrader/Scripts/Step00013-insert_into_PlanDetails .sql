﻿USE SpendWiseDB;
GO

IF  EXISTS (SELECT * FROM sys.tables WHERE name = 'PlanDetails' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
    INSERT INTO SpendWise.PlanDetails (name, description, noCategory, category, image, created_by)
    VALUES 
        ('Student plan', 'The Student Budget Plan is designed to help students manage their finances effectively while pursuing their education. This plan aims to provide a structured approach to budgeting, ensuring that students can cover essential expenses, save for the future, and enjoy their college life without financial stress.', 8, 'Food, Rent, Utilities, Entertainment, Clothes, Transportation, Education, Misicellaneous','./assets/student.svg',(SELECT user_id from SpendWise.Users where name='Sabina Barila')),
        ('Family Plan',  'The Family Budget Plan is crafted to assist families in managing their household finances efficiently. This plan offers a comprehensive approach to budgeting, ensuring that families can cover essential living expenses, save for future needs, and enjoy quality time together without financial worries. It is designed to help families balance their income with expenditures, prioritize important costs, and create a stable financial environment for all members.', 9, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Education, Health','./assets/family.svg',(SELECT user_id from SpendWise.Users where name='Cirja Ioan')),
        ('Travel Plan', ' The Travel Budget Plan is designed to help individuals and families manage their travel expenses effectively. This plan provides a structured approach to budgeting for vacations and trips, ensuring that travelers can cover all necessary costs, save money, and fully enjoy their adventures without financial stress. It includes strategies for planning expenses such as accommodation, transportation, food, activities, and souvenirs, making it easier to have memorable and well-budgeted travel experiences.', 14, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Accommodation, Local transport, Local food, Activities, Souvenirs, Flights, Insurance','./assets/travel.svg',(SELECT user_id from SpendWise.Users where name='Cazamir Andrei')),
        ('Wedding Plan', 'The Wedding Budget Plan is tailored to help couples manage their wedding expenses efficiently. This plan offers a detailed and structured approach to budgeting for all aspects of the wedding, ensuring that couples can cover essential costs, avoid overspending, and create their dream wedding without financial stress. It includes guidelines for planning expenses such as venue, catering, attire, decorations, and entertainment, allowing couples to focus on celebrating their special day while keeping their finances in check.', 11, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Venue, Catering, Attire, Decorations','./assets/wedding.svg',(SELECT user_id from SpendWise.Users where name='Sabina Barila')),
        ('Retirement Plan', 'The Retirement Budget Plan is designed to help individuals plan and manage their finances effectively as they approach and enter retirement. This plan provides a comprehensive approach to budgeting, ensuring that retirees can cover essential living expenses, maintain their desired lifestyle, and save for unforeseen costs. It focuses on strategies for managing income from pensions, savings, and investments, as well as planning for healthcare, travel, and leisure activities. The goal is to provide financial stability and peace of mind during the retirement years.', 10, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Health care, Travel, Savings','./assets/travel.svg',(SELECT user_id from SpendWise.Users where name='Cazamir Andrei')),
        ('StartUp Plan', 'The Startup Budget Plan is designed to help entrepreneurs manage the financial aspects of launching and growing their new business. This plan provides a structured approach to budgeting, ensuring that startup owners can cover essential expenses, allocate resources efficiently, and plan for future growth. It includes guidelines for managing costs such as office rent, equipment, marketing, salaries, and other operational expenses. By following this plan, entrepreneurs can focus on building their business while maintaining financial stability and preparing for unforeseen challenges.', 11, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Office rent, Equipment, Marketing, Salaries','./assets/startup.svg',(SELECT user_id from SpendWise.Users where name='Sabina Barila')),
        ('Emergency Fund Plan', 'The Emergency Fund Plan is designed to help individuals and families prepare financially for unexpected situations and emergencies. This plan provides a structured approach to saving and managing funds, ensuring that there are sufficient resources to cover unforeseen expenses such as medical emergencies, home repairs, job loss, or legal issues. By establishing an emergency fund, individuals can achieve financial security and peace of mind, knowing they are prepared for life''s unexpected challenges without compromising their financial stability.', 10, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Medical, Repair and maintenance, Legal','./assets/startup.svg',(SELECT user_id from SpendWise.Users where name='Sabina Barila')),
        ('Holiday shopping Plan', 'The Holiday Shopping Budget Plan is crafted to help individuals and families manage their expenses during the holiday season. This plan provides a structured approach to budgeting for holiday-related costs, ensuring that you can cover all necessary expenses without overspending. It includes strategies for planning and allocating funds for gifts, decorations, travel, food, and entertainment. By following this plan, you can enjoy a festive and joyful holiday season while keeping your finances in check and avoiding post-holiday financial stress.', 10, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Gifts, Decorations, Travel','./assets/startup.svg',(SELECT user_id from SpendWise.Users where name='Sabina Barila')),
        ('Fitness & Wellness Plan', 'The Fitness & Wellness Plan is designed to help individuals maintain a healthy lifestyle while managing their fitness and wellness expenses effectively. This plan provides a structured approach to budgeting, ensuring that you can prioritize your physical and mental well-being without exceeding your financial limits. It includes strategies for budgeting expenses such as gym memberships, fitness equipment, wellness programs, supplements, and health foods. By following this plan, you can achieve your fitness goals, improve your overall well-being, and maintain a balanced budget for a healthier lifestyle.', 11, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Gym membership, Equipment, Supplements, Health food','./assets/startup.svg',(SELECT user_id from SpendWise.Users where name='Sabina Barila')),
        ('Pet care Plan', 'The Pet Care Plan is designed to help pet owners manage their pet-related expenses efficiently. This plan provides a structured approach to budgeting and managing costs associated with pet care, ensuring that owners can cover essential expenses without exceeding their financial limits. It includes strategies for budgeting expenses such as pet food, monthly maintenance, utilities, entertainment, transportation, and other specific pet needs. By following this plan, pet owners can ensure good care for their pets while maintaining a balanced financial approach.', 12, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Pet food, Veterinary, Grooming, Toys and accesories, Pet insurance','./assets/startup.svg',(SELECT user_id from SpendWise.Users where name='Cazamir Andrei')),
        ('Medical expense plan', 'The Medical Expense Plan is designed to help individuals and families effectively manage their healthcare expenses. This plan provides a structured approach to budgeting and preparing for medical costs, ensuring that necessary healthcare needs can be met without financial strain. It includes strategies for budgeting expenses such as doctor visits, medications, procedures, and health insurance premiums. By following this plan, individuals and families can maintain their health and well-being while managing healthcare expenses responsibly and effectively.', 11, 'Food, Rent/Monthly rate, Utilities, Entertainment, Clothes, Transportation, Misicellaneous, Doctor visits, Medications, Procedures, Insurance','./assets/startup.svg',(SELECT user_id from SpendWise.Users where name='Cirja Ioan')),
        ('Default plan', '50-30-20 rule', 3, 'Needs, Wants, Savings','./assets/travel.svg',(SELECT user_id from SpendWise.Users where name='Cirja Ioan'));
END
GO