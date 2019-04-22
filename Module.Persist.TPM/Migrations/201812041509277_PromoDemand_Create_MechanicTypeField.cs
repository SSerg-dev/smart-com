namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemand_Create_MechanicTypeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoDemand", "MechanicTypeId", c => c.Guid());
            CreateIndex("dbo.PromoDemand", "MechanicTypeId");
            AddForeignKey("dbo.PromoDemand", "MechanicTypeId", "dbo.MechanicType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoDemand", "MechanicTypeId", "dbo.MechanicType");
            DropIndex("dbo.PromoDemand", new[] { "MechanicTypeId" });
            DropColumn("dbo.PromoDemand", "MechanicTypeId");
        }
    }
}
