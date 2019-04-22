namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColorName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Color", "DisplayName", c => c.String(maxLength: 255));
            CreateIndex("dbo.Color", "DisplayName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Color", new[] { "DisplayName" });
            DropColumn("dbo.Color", "DisplayName");
        }
    }
}
