namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMechanicLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PromoDemandChangeIncident", "OldMarsMechanic", c => c.String(maxLength: 255));
            AlterColumn("dbo.PromoDemandChangeIncident", "NewMarsMechanic", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoDemandChangeIncident", "NewMarsMechanic", c => c.String(maxLength: 20));
            AlterColumn("dbo.PromoDemandChangeIncident", "OldMarsMechanic", c => c.String(maxLength: 20));
        }
    }
}
