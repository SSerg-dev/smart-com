namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Demand : DbMigration
    {
        public override void Up()
        {
            //Sql("CREATE VIEW [dbo].[Demand] AS SELECT [Id], [Disabled], [DeletedDate], [ClientId], [BrandId], [BrandTechId], [Number], [Name], [StartDate], [EndDate], [DispatchesStart], [DispatchesEnd], [PlanBaseline], [PlanDuration], [PlanUplift], [PlanIncremental], [PlanActivity], [PlanSteal], [FactBaseline], [FactDuration], [FactUplift], [FactIncremental], [FactActivity], [FactSteal] FROM [dbo].[Promo]");
        }
        
        public override void Down()
        {
            //Sql("DROP VIEW [dbo].[Demand]");
        }
    }
}
