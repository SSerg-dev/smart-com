namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Finance : DbMigration
    {
        public override void Up()
        {
            //Sql("CREATE VIEW [dbo].[Finance] AS SELECT [Id], [Disabled], [DeletedDate], [ClientId], [BrandId], [BrandTechId], [PromoStatusId], [MarsMechanicId], [MarsMechanicTypeId], [InstoreMechanicId], [InstoreMechanicTypeId], [EventId], [Number], [Name], [StartDate], [EndDate], [DispatchesStart], [DispatchesEnd], [MarsMechanicDiscount], [InstoreMechanicDiscount], [RoiPlan], [RoiFact] FROM[dbo].[Promo]");
        }
        
        public override void Down()
        {
            //Sql("DROP VIEW [dbo].[Finance]");
        }
    }
}
