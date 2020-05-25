namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProductDifference_Fields_Mod : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PromoProductDifference", "DMDGROUP", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "Type", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "MOE", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "SALES_ORG", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "SALES_DIST_CHANNEL", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "SALES_DIVISON", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "BUS_SEG", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "MKT_SEG", c => c.String());
            AlterColumn("dbo.PromoProductDifference", "Roll_FC_Flag", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoProductDifference", "Roll_FC_Flag", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "MKT_SEG", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "BUS_SEG", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "SALES_DIVISON", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "SALES_DIST_CHANNEL", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "SALES_ORG", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "MOE", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "Type", c => c.Int());
            AlterColumn("dbo.PromoProductDifference", "DMDGROUP", c => c.Int());
        }
    }
}
