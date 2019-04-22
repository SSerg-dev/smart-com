namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mechanic_MechanicName : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Mechanic", "MechanicName", c => c.String(maxLength: 255));
            //DropColumn("dbo.Mechanic", "MechanicType");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Mechanic", "MechanicType", c => c.String(maxLength: 255));
            //DropColumn("dbo.Mechanic", "MechanicName");
        }
    }
}
