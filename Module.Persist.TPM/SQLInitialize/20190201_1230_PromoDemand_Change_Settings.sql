INSERT INTO [Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_CHANGE_PROPERTIES'
           ,N'String'
           ,N'ClientTreeId, ProductTreeId, StartDate, EndDate, DispatchesStart, DispatchesEnd, PlanUplift, PlanIncrementalLsv, MarsMechanicDiscount, InstoreMechanicDiscount, Mechanic, MarsMechanic, InstoreMechanic'
           ,N'Атрибуты промо при изменении которых необходимо создавать запись об изменении'),
		   (N'PROMO_CHANGE_PERIOD_DAYS'
		   ,N'int'
		   ,N'56'
		   ,N'При изменении промо в течении этого количества дней необходимо создавать запись об изменении'),
		   (N'PROMO_CHANGE_MARS_DISCOUNT'
		   ,N'int'
		   ,N'3'
		   ,N'При изменении МАРС-скидки на заданное или большее значени необходимо создавать запись об изменении'),
		   (N'PROMO_CHANGE_INSTORE_DISCOUNT'
		   ,N'int'
		   ,N'5'
		   ,N'При изменении Instore-скидки на заданное или большее значени необходимо создавать запись об изменении'),
		   (N'PROMO_CHANGE_DURATION_DAYS'
		   ,N'int'
		   ,N'3'
		   ,N'При изменении длительности на заданное или большее количество дней необходимо создавать запись об изменении'),
		   (N'PROMO_CHANGE_DISPATCH_DAYS'
		   ,N'int'
		   ,N'5'
		   ,N'При изменении дат отгрузки на заданное или большее количество дней необходимо создавать запись об изменении')