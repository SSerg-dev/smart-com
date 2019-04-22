namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastApproved_reserved_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "LastApprovedDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "LastApprovedDate");
        }
    }
}
