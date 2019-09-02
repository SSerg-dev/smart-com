namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_InOutProductIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "InOutProductIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "InOutProductIds");
        }
    }
}
