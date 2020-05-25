namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_PromoProductDifference : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PromoProductDifference", "QTY", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoProductDifference", "QTY", c => c.Int());
        }
    }
}
