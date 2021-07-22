namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anaplan_PostPromoEffectReport_Modification : DbMigration
    {
        public override void Up()
        {
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }

        private string SqlString =
		@"
		CREATE OR ALTER VIEW [Jupiter].[PlanPostPromoEffectReportWeekView] AS
            WITH
				BaseDataView
			AS
			(
				SELECT  
					p.[Id],
					p.[ClientTreeId], 
					p.[ClientTreeKeyId],
					p.[Name],
					p.[Number],
					p.[StartDate],
					p.[EndDate],
					p.[DispatchesStart],
					p.[DispatchesEnd],
					p.[InOut],
					p.[PlanPromoUpliftPercent],
					p.[BrandTechId],
					ISNULL(p.[IsOnInvoice], 0) AS IsOnInvoice,
					ps.[Name] AS PromoStatusName,

					pp.[ProductId],
					pp.[ZREP], 
					pp.[PlanProductIncrementalCaseQty], 
					pp.[PlanProductBaselineCaseQty], 
					pp.[PlanProductPostPromoEffectLSVW1], 
					pp.[PlanProductPostPromoEffectLSVW2], 
					pp.[PlanProductBaselineLSV],
		
					Jupiter.GetDemandCode(ct.[parentId], ct.[DemandCode]) AS DemandCode,
					ISNULL(ct.[PostPromoEffectW1], 0) AS PostPromoEffectW1,
					ISNULL(ct.[PostPromoEffectW2], 0) AS PostPromoEffectW2,

					CASE WHEN (1 - dispEndD.[MarsDay]) < 1 
						THEN CAST(DATEADD(DAY, 1 - dispEndD.[MarsDay] + 7, dispEndD.[OriginalDate]) AS DATE)
						ELSE CAST(DATEADD(DAY, 1 - dispEndD.[MarsDay], dispEndD.[OriginalDate]) AS DATE)
					END AS WeekStart,

					ISNULL(pl.[Price], 0) AS [Price]
				FROM [Promo] p
				INNER JOIN [PromoProduct] pp 
					ON pp.[PromoId] = p.[Id]
				INNER JOIN [PromoStatus] ps 
					ON ps.[Id] = p.[PromoStatusId]
				INNER JOIN [ClientTree] ct
					ON ct.[Id] = p.[ClientTreeKeyId]
				INNER JOIN [Dates] dispEndD
					ON dispEndD.[OriginalDate] = CAST(p.[DispatchesEnd] AS DATE) 
				LEFT JOIN [PriceList] pl
					ON pl.Id = (
						SELECT TOP(1)
							Id
						FROM [PriceList]
						WHERE [ClientTreeId] = p.[ClientTreeKeyId]
							AND [ProductId] = pp.[ProductId]
							AND [StartDate] <= p.[DispatchesStart]
							AND [EndDate] >= p.[DispatchesEnd]
							AND [Disabled] = 0
						ORDER BY [StartDate] DESC
					)

				WHERE pp.[Disabled] = 0 AND p.[Disabled] = 0
			),
				BaseLineView
			AS
			(
				SELECT 
					p.[ZREP],
					bl.[SellInBaselineQTY],
					bl.[SellOutBaselineQTY],
					CAST(bl.[StartDate] AS DATE) AS [StartDate],
					bl.[DemandCode]
 				FROM [BaseLine] bl
				INNER JOIN [Product] p 
					ON p.[Id] = bl.[ProductId]
				WHERE bl.[Disabled] = 0
			),
				ForCalculatedView
			AS 
			(
				SELECT
					p.*,
					ISNULL(ctbt.Share, 1) AS Share,
					CASE WHEN p.[IsOnInvoice] = 1
						THEN ISNULL(blW1.[SellInBaselineQTY], 0) 
						ELSE ISNULL(blW1.[SellOutBaselineQTY], 0)
					END AS BaseLinePostPromoEffectW1QTY,
					CASE WHEN p.[IsOnInvoice] = 1
						THEN ISNULL(blW2.[SellInBaselineQTY], 0) 
						ELSE ISNULL(blW2.[SellOutBaselineQTY], 0)
					END AS BaseLinePostPromoEffectW2QTY,
		
					CAST(d.[MarsYear] AS NVARCHAR(4)) + 'P' + REPLACE(STR(d.[MarsPeriod], 2), SPACE(1), '0') + d.[MarsWeekName] AS [Week]
				FROM BaseDataView p
				LEFT JOIN BaseLineView blW1
					ON blW1.[DemandCode] = p.[DemandCode]
						AND blW1.[ZREP] = p.[ZREP]
						AND blW1.[StartDate] = p.[WeekStart]
				LEFT JOIN BaseLineView blW2
					ON blW2.[DemandCode] = p.[DemandCode]
						AND blW2.[ZREP] = p.[ZREP]
						AND blW2.[StartDate] = DATEADD(DAY, 7, p.[WeekStart])
				LEFT JOIN [ClientTreeBrandTech] ctbt
					ON ctbt.Id = (
						SELECT TOP(1)
							Id
						FROM [ClientTreeBrandTech]
						WHERE
							[ParentClientTreeDemandCode] = p.[DemandCode]
							AND [BrandTechId] = p.[BrandTechId]
							AND [Disabled] = 0
					)
				INNER JOIN [Dates] d
					ON d.[OriginalDate] = CAST(p.[WeekStart] AS DATE) 
			)

			SELECT 
				NEWID()														AS Id,
				CONCAT(c.[ZREP], '_0125') 									AS [ZREP],
				c.[PromoStatusName]											AS [Status],
				FORMATMESSAGE('%s#%i', c.[Name], c.[Number])				AS [PromoNameId],
				c.[Number]													AS [PromoNumber],
				CAST(c.[WeekStart] AS DATETIMEOFFSET(7))					AS [WeekStartDate],
				CAST(c.[WeekStart] AS DATETIMEOFFSET(7))					AS [StartDate],
				CAST(DATEADD(DAY, 14, c.[WeekStart]) AS DATETIMEOFFSET(7))	AS [EndDate],
				CASE WHEN c.[DemandCode] IS NULL OR c.[DemandCode] = ''
					THEN 'Demand code was not found'
					ELSE c.[DemandCode]
				END															AS [DemandCode],
				c.[InOut]													AS [InOut],
				'RU_0125'													AS [LocApollo],
				'7'															AS [TypeApollo],
				'SHIP_LEWAND_CS'											AS [ModelApollo],
				c.[Week]													AS [Week],
				c.[IsOnInvoice]												AS [IsOnInvoice],
				c.[PlanPromoUpliftPercent]									AS [PlanUplift],	

				ROUND(c.[BaseLinePostPromoEffectW1QTY] * c.[Share] / 100 * c.[PostPromoEffectW1] / 100, 2)					AS PlanPostPromoEffectQtyW1,
				ROUND(c.[BaseLinePostPromoEffectW1QTY] * c.[Share] / 100, 2)												AS PlanProductBaselineCaseQtyW1,
				ROUND(c.[BaseLinePostPromoEffectW1QTY] * c.[Price] * c.[Share] / 100 * ABS(c.[PostPromoEffectW1]) / 100, 2) AS PlanProductPostPromoEffectLSVW1,
				ROUND(c.[BaseLinePostPromoEffectW1QTY] * c.[Price] * c.[Share] / 100, 2)									AS PlanProductBaselineLSVW1,

				ROUND(c.[BaseLinePostPromoEffectW2QTY] * c.[Share] / 100 * c.[PostPromoEffectW2] / 100, 2)					AS PlanPostPromoEffectQtyW2,
				ROUND(c.[BaseLinePostPromoEffectW2QTY] * c.[Share] / 100, 2)												AS PlanProductBaselineCaseQtyW2,
				ROUND(c.[BaseLinePostPromoEffectW2QTY] * c.[Price] * c.[Share] / 100 * ABS(c.[PostPromoEffectW2]) / 100, 2) AS PlanProductPostPromoEffectLSVW2,
				ROUND(c.[BaseLinePostPromoEffectW2QTY] * c.[Price] * c.[Share] / 100, 2)									AS PlanProductBaselineLSVW2

			FROM ForCalculatedView c
			GO
        ";
    }
}
