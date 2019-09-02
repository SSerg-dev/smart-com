namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentToPromoRejectIncident : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoRejectIncident", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoRejectIncident", "Comment");
        }
    }
}
