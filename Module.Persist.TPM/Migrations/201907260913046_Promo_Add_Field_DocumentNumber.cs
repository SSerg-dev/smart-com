namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Add_Field_DocumentNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "DocumentNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "DocumentNumber");
        }
    }
}
