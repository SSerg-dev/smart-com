namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_PromoProductsCorrection : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PromoProductsCorrection", "CreateDate", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.PromoProductsCorrection", "ChangeDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoProductsCorrection", "ChangeDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.PromoProductsCorrection", "CreateDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
