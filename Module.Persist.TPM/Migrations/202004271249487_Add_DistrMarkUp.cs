namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DistrMarkUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "DistrMarkUp", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "DistrMarkUp");
        }
    }
}
