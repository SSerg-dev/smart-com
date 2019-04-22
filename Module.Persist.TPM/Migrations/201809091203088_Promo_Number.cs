namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Number : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "Number", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "Number");
        }
    }
}
