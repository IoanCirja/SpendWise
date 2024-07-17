﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMonthlyPlanRepository
    {
        Task<bool> AddMonthlyPlans(MonthlyPlan monthlyPlan );
        Task<bool> CancelMonthlyPlan(Guid id);
    }
}
