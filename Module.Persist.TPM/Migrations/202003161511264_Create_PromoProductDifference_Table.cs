namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_PromoProductDifference_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoProductDifference",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        DemandUnit = c.String(),
                        DMDGROUP = c.Int(),
                        LOC = c.String(),
                        StartDate = c.String(),
                        DURInMinutes = c.Int(),
                        Type = c.Int(),
                        ForecastID = c.String(),
                        QTY = c.Int(),
                        MOE = c.Int(),
                        Source = c.String(),
                        SALES_ORG = c.Int(),
                        SALES_DIST_CHANNEL = c.Int(),
                        SALES_DIVISON = c.Int(),
                        BUS_SEG = c.Int(),
                        MKT_SEG = c.Int(),
                        DELETION_FLAG = c.String(),
                        DELETION_DATE = c.String(),
                        INTEGRATION_STAMP = c.String(),
                        Roll_FC_Flag = c.Int(),
                        Promotion_Start_Date = c.String(),
                        Promotion_Duration = c.Int(),
                        Promotion_Status = c.String(),
                        Promotion_Campaign = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PromoProductDifference");
        }
    }
}
