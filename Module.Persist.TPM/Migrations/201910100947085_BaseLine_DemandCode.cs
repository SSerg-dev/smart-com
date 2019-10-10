namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseLine_DemandCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseLine", "DemandCode", c => c.String());
            Sql("UPDATE BaseLine SET DemandCode = (SELECT DemandCode FROM ClientTree WHERE Id = BaseLine.ClientTreeId)");
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseLine", "DemandCode");
        }
    }
}
