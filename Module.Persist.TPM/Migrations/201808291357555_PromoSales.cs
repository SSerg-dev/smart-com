namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSales : DbMigration
    {
        public override void Up()
        {
            //Sql("CREATE VIEW [dbo].[PromoSales] AS SELECT s.[Id], (CASE WHEN p.[Disabled] = 1 OR s.[Disabled] = 1 THEN CONVERT(bit, 1) ELSE CONVERT(bit, 0) END) AS 'Disabled', p.[DeletedDate], p.[Number], p.[Name], p.[ClientId], p.[BrandId], p.[BrandTechId],p.[PromoStatusId], p.[MechanicId], p.[MechanicTypeId], p.[StartDate], p.[EndDate], p.[DispatchesStart], p.[DispatchesEnd], p.[MechanicDiscount], s.[BudgetItemId], s.[Plan], s.[Fact]FROM [dbo].[Promo] p JOIN [dbo].[Sale] s ON p.[Id] = s.[PromoId]");
        }
        
        public override void Down()
        {
            //Sql("DROP VIEW [dbo].[PromoSales]");
        }
    }
}
