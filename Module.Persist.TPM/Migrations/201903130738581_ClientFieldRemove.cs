namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientFieldRemove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Promo", "ClientId", "dbo.Client");
            DropIndex("dbo.Promo", new[] { "ClientId" });
            DropColumn("dbo.Promo", "ClientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ClientId", c => c.Guid());
            CreateIndex("dbo.Promo", "ClientId");
            AddForeignKey("dbo.Promo", "ClientId", "dbo.Client", "Id");
        }
    }
}
