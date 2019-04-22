namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupportPromo_Add_UserTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupportPromo", "UserTimestamp", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupportPromo", "UserTimestamp");
        }
    }
}
