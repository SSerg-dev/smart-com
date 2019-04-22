namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoStatusSystemName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoStatus", "SystemName", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoStatus", "SystemName");
        }
    }
}
