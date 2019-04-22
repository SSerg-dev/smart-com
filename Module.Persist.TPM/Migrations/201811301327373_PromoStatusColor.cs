namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoStatusColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoStatus", "Color", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoStatus", "Color");
        }
    }
}
