namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNameByPromoProductCorrection : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.PromoProductsCorrection", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProductsCorrection", "UserName"); 
        }
    }
}
