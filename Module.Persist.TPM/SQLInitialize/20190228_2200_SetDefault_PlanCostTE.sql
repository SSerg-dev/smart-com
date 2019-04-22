-- для исключения деления на ноль
Update PromoSupport Set PlanCostTE=10 Where (PlanCostTE is NULL) OR (PlanCostTE < 1)