namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoNumberIdentity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "Number", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "Number", c => c.Int());
        }
    }
}
