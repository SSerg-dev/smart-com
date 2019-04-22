CREATE TRIGGER trig_Update ON [dbo].[PromoSales]
INSTEAD OF UPDATE AS
UPDATE [dbo].[Promo]
SET [Number] = i.[Number],
	[Name] = i.[Name],
	[ClientId] = i.[ClientId],
	[BrandId] = i.[BrandId],
	[BrandTechId] = i.[BrandTechId],
	[PromoStatusId] = i.[PromoStatusId],
	[MechanicId] = i.[MechanicId],
	[StartDate] = i.[StartDate],
	[EndDate] = i.[EndDate],
	[DispatchesStart] = i.[DispatchesStart],
	[DispatchesEnd] = i.[DispatchesEnd]
FROM [dbo].[Sale] s JOIN inserted i ON s.[Id] = i.[Id], [dbo].[Promo] p
WHERE p.[Id] = s.[PromoId]

UPDATE [dbo].[Sale]
SET [BudgetItemId] = i.[BudgetItemId],
	[Plan] = i.[Plan],
	[Fact] = i.[Fact]
FROM [dbo].[Sale] s JOIN inserted i ON s.[Id] = i.[Id]