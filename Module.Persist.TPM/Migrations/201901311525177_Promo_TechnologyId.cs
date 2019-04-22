namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_TechnologyId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "TechnologyId", c => c.Guid());
            CreateIndex("dbo.Promo", "TechnologyId");
            AddForeignKey("dbo.Promo", "TechnologyId", "dbo.Technology", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "TechnologyId", "dbo.Technology");
            DropIndex("dbo.Promo", new[] { "TechnologyId" });
            DropColumn("dbo.Promo", "TechnologyId");
        }
    }
}
