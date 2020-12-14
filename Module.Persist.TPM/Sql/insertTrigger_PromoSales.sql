CREATE TRIGGER trig_Insert ON [PromoSales]
INSTEAD OF INSERT AS
INSERT INTO [Promo] ([Id], [Disabled], [DeletedDate], [Name], [ClientId], [BrandId], [BrandTechId], [PromoStatusId], [MechanicId], [StartDate], [EndDate], [DispatchesStart], [DispatchesEnd])
SELECT [Id], 0, NULL, [Name], [ClientId], [BrandId], [BrandTechId], [PromoStatusId], [MechanicId], [StartDate], [EndDate], [DispatchesStart], [DispatchesEnd]
FROM inserted
INSERT INTO [Sale] ([Id], [Disabled], [DeletedDate], [PromoId], [BudgetItemId], [Plan], [Fact])
SELECT NEWID(), 0, NULL, [Id], [BudgetItemId], [Plan], [Fact]
FROM inserted