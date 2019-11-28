namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProductsCorrection_DatabaseGeneratedOption_Identity : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PromoProductsCorrection");
            AlterColumn("dbo.PromoProductsCorrection", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.PromoProductsCorrection", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PromoProductsCorrection");
            AlterColumn("dbo.PromoProductsCorrection", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.PromoProductsCorrection", "Id");
        }
    }
}
