namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CreatorLogin_To_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "CreatorLogin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "CreatorLogin");
        }
    }
}
