namespace Domain
{
    public class BudgetPlan
    {
        public Guid plan_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public int noCategory { get; set; }

        public string category { get; set; }
        public string image { get; set; }
        public bool isActive { get; set; }
        public Guid created_by { get; set; }
    }
}
