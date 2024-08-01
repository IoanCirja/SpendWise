export type BudgetPlan = {
    plan_id: string;
    name: string;
    description: string;
    noCategory: number;
    category: string;
    image: string;
    creationDate: string; 
    created_by: string;
  };
  
  export type BudgetPlans = BudgetPlan[];
  