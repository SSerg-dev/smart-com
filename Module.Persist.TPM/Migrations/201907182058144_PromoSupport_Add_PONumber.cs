namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_Add_PONumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "PONumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupport", "PONumber");
        }
    }
}
