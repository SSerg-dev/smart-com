namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Drop_Finance : DbMigration
    {
        public override void Up()
        {
            Sql("DROP VIEW IF EXISTS [dbo].[Finance];");
        }

        public override void Down()
        {
            Sql("CREATE VIEW [dbo].[Finance] AS SELECT [Id], [Disabled], [DeletedDate], [ClientId], [BrandId], [BrandTechId], [PromoStatusId], [MarsMechanicId], [MarsMechanicTypeId], [InstoreMechanicId], [InstoreMechanicTypeId], [EventId], [Number], [Name], [StartDate], [EndDate], [DispatchesStart], [DispatchesEnd], [MarsMechanicDiscount], [InstoreMechanicDiscount], [RoiPlan], [RoiFact] FROM[dbo].[Promo]");
        }
    }
}
