namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DeviationCoefficient_ClientTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "DeviationCoefficient", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "DeviationCoefficient");
        }
    }
}
