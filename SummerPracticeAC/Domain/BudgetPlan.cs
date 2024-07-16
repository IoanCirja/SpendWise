﻿namespace Domain
{
    public class BudgetPlan
    {
        public Guid PlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NoCategory { get; set; }
        public string Category { get; set; }
        public Guid CreatedBy { get; set; } 
        public string Imagine { get; set; }

        public User User { get; set; }
    }
}
