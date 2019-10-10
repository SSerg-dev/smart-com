namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mod_Baseline_Col : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BaseLine", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.BaseLine", "Unique_BaseLine");
            AlterColumn("dbo.BaseLine", "DemandCode", c => c.String(maxLength: 255));
            CreateIndex("dbo.BaseLine", new[] { "DeletedDate", "ProductId", "StartDate", "DemandCode" }, unique: true, name: "Unique_BaseLine");
            DropColumn("dbo.BaseLine", "ClientTreeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseLine", "ClientTreeId", c => c.Int(nullable: false));
            DropIndex("dbo.BaseLine", "Unique_BaseLine");
            AlterColumn("dbo.BaseLine", "DemandCode", c => c.String());
            CreateIndex("dbo.BaseLine", new[] { "DeletedDate", "ProductId", "ClientTreeId", "StartDate" }, unique: true, name: "Unique_BaseLine");
            AddForeignKey("dbo.BaseLine", "ClientTreeId", "dbo.ClientTree", "Id");
        }
    }
}
