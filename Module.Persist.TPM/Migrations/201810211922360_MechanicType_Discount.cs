namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicType_Discount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MechanicType", "Discount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MechanicType", "Discount");
        }
    }
}
