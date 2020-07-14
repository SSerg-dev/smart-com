namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicType_ClientTreeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MechanicType", "ClientTreeId", c => c.Int());
            CreateIndex("dbo.MechanicType", "ClientTreeId");
            AddForeignKey("dbo.MechanicType", "ClientTreeId", "dbo.ClientTree", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MechanicType", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.MechanicType", new[] { "ClientTreeId" });
            DropColumn("dbo.MechanicType", "ClientTreeId");
        }
    }
}
