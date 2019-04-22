namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_AttachFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "AttachFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupport", "AttachFileName");
        }
    }
}
