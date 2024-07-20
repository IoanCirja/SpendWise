export interface MonthlyPlan {
    monthlyPlan_id: string;
    user_id: string;
    plan_id: string;
    plan_name: string;
    description: string;
    noCategory: number;
    category: string;
    image: string;
    date: string; 
    totalAmount: number;
    amountSpent: number;
    status: string;
    priceByCategory: string;
    spentOfCategory: string;
  }
  