namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoTypes_To_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "PromoTypesId", c => c.Guid());
            CreateIndex("dbo.Promo", "PromoTypesId");
            AddForeignKey("dbo.Promo", "PromoTypesId", "dbo.PromoTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "PromoTypesId", "dbo.PromoTypes");
            DropIndex("dbo.Promo", new[] { "PromoTypesId" });
            DropColumn("dbo.Promo", "PromoTypesId");
        }
    }
}
