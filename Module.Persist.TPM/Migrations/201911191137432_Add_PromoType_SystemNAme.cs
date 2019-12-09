namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoType_SystemNAme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoTypes", "SystemName", c => c.String(maxLength: 255));
          
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoTypes", "SystemName");
        }
    }
}
