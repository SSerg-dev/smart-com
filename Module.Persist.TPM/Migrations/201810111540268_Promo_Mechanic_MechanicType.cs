namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Mechanic_MechanicType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "MechanicTypeId", c => c.Guid());
            AddColumn("dbo.Promo", "MechanicDiscount", c => c.Int());
            CreateIndex("dbo.Promo", "MechanicTypeId");
            AddForeignKey("dbo.Promo", "MechanicTypeId", "dbo.MechanicType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "MechanicTypeId", "dbo.MechanicType");
            DropIndex("dbo.Promo", new[] { "MechanicTypeId" });
            DropColumn("dbo.Promo", "MechanicDiscount");
            DropColumn("dbo.Promo", "MechanicTypeId");
        }
    }
}
