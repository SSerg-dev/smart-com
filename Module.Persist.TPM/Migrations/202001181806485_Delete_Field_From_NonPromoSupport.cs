namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_Field_From_NonPromoSupport : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NonPromoSupport", "UserTimestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NonPromoSupport", "UserTimestamp", c => c.String());
        }
    }
}
