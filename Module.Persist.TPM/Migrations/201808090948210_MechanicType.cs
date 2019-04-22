namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mechanic", "MechanicType", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mechanic", "MechanicType");
        }
    }
}
