namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoType_To_Mechanic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mechanic", "PromoTypeId", c => c.Guid());
            CreateIndex("dbo.Mechanic", "PromoTypeId");
            AddForeignKey("dbo.Mechanic", "PromoTypeId", "dbo.PromoTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mechanic", "PromoTypeId", "dbo.PromoTypes");
            DropIndex("dbo.Mechanic", new[] { "PromoTypeId" });
            DropColumn("dbo.Mechanic", "PromoTypeId");
        }
    }
}
