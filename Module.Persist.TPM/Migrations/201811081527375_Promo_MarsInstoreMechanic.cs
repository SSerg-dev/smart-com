namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_MarsInstoreMechanic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "MarsMechanicId", c => c.Guid());
            AddColumn("dbo.Promo", "MarsMechanicTypeId", c => c.Guid());
            AddColumn("dbo.Promo", "InstoreMechanicId", c => c.Guid());
            AddColumn("dbo.Promo", "InstoreMechanicTypeId", c => c.Guid());
            AddColumn("dbo.Promo", "MarsMechanicDiscount", c => c.Int());
            AddColumn("dbo.Promo", "InstoreMechanicDiscount", c => c.Int());
            CreateIndex("dbo.Promo", "MarsMechanicId");
            CreateIndex("dbo.Promo", "MarsMechanicTypeId");
            CreateIndex("dbo.Promo", "InstoreMechanicId");
            CreateIndex("dbo.Promo", "InstoreMechanicTypeId");
            AddForeignKey("dbo.Promo", "InstoreMechanicId", "dbo.Mechanic", "Id");
            AddForeignKey("dbo.Promo", "InstoreMechanicTypeId", "dbo.MechanicType", "Id");
            AddForeignKey("dbo.Promo", "MarsMechanicId", "dbo.Mechanic", "Id");
            AddForeignKey("dbo.Promo", "MarsMechanicTypeId", "dbo.MechanicType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promo", "MarsMechanicTypeId", "dbo.MechanicType");
            DropForeignKey("dbo.Promo", "MarsMechanicId", "dbo.Mechanic");
            DropForeignKey("dbo.Promo", "InstoreMechanicTypeId", "dbo.MechanicType");
            DropForeignKey("dbo.Promo", "InstoreMechanicId", "dbo.Mechanic");
            DropIndex("dbo.Promo", new[] { "InstoreMechanicTypeId" });
            DropIndex("dbo.Promo", new[] { "InstoreMechanicId" });
            DropIndex("dbo.Promo", new[] { "MarsMechanicTypeId" });
            DropIndex("dbo.Promo", new[] { "MarsMechanicId" });
            DropColumn("dbo.Promo", "InstoreMechanicDiscount");
            DropColumn("dbo.Promo", "MarsMechanicDiscount");
            DropColumn("dbo.Promo", "InstoreMechanicTypeId");
            DropColumn("dbo.Promo", "InstoreMechanicId");
            DropColumn("dbo.Promo", "MarsMechanicTypeId");
            DropColumn("dbo.Promo", "MarsMechanicId");
        }
    }
}
