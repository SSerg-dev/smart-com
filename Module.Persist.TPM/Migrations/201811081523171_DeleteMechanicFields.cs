namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteMechanicFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Promo", "MechanicId", "dbo.Mechanic");
            DropForeignKey("dbo.Promo", "MechanicTypeId", "dbo.MechanicType");
            DropIndex("dbo.Promo", new[] { "MechanicId" });
            DropIndex("dbo.Promo", new[] { "MechanicTypeId" });
            DropColumn("dbo.Promo", "MechanicId");
            DropColumn("dbo.Promo", "MechanicTypeId");
            DropColumn("dbo.Promo", "MechanicDiscount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "MechanicDiscount", c => c.Int());
            AddColumn("dbo.Promo", "MechanicTypeId", c => c.Guid());
            AddColumn("dbo.Promo", "MechanicId", c => c.Guid());
            CreateIndex("dbo.Promo", "MechanicTypeId");
            CreateIndex("dbo.Promo", "MechanicId");
            AddForeignKey("dbo.Promo", "MechanicTypeId", "dbo.MechanicType", "Id");
            AddForeignKey("dbo.Promo", "MechanicId", "dbo.Mechanic", "Id");
        }
    }
}
