namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseLine_UpdateFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseLine", "ClientTreeId", c => c.Int(nullable: false));
            AddColumn("dbo.BaseLine", "QTY", c => c.Double(nullable: false));
            AddColumn("dbo.BaseLine", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.BaseLine", "BaselineLSV", c => c.Double(nullable: false));
            AddColumn("dbo.BaseLine", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.BaseLine", "StartDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            CreateIndex("dbo.BaseLine", "ClientTreeId");
            AddForeignKey("dbo.BaseLine", "ClientTreeId", "dbo.ClientTree", "Id");
            DropColumn("dbo.BaseLine", "DemandCode");
            DropColumn("dbo.BaseLine", "Baseline");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseLine", "Baseline", c => c.Double(nullable: false));
            AddColumn("dbo.BaseLine", "DemandCode", c => c.String(nullable: false, maxLength: 255));
            DropForeignKey("dbo.BaseLine", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.BaseLine", new[] { "ClientTreeId" });
            AlterColumn("dbo.BaseLine", "StartDate", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.BaseLine", "Type");
            DropColumn("dbo.BaseLine", "BaselineLSV");
            DropColumn("dbo.BaseLine", "Price");
            DropColumn("dbo.BaseLine", "QTY");
            DropColumn("dbo.BaseLine", "ClientTreeId");
        }
    }
}
