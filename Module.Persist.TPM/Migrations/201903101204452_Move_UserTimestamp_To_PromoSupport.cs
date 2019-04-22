namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Move_UserTimestamp_To_PromoSupport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "UserTimestamp", c => c.String());
            DropColumn("dbo.PromoSupportPromo", "UserTimestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoSupportPromo", "UserTimestamp", c => c.String());
            DropColumn("dbo.PromoSupport", "UserTimestamp");
        }
    }
}
