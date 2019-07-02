namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EAN_To_EANCase_Add_EANPC : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Product", "EAN", "EAN_Case");
            AddColumn("dbo.Product", "EAN_PC", c => c.String(maxLength: 255));
            RenameColumn("dbo.PromoProduct", "EAN", "EAN_Case");
            AddColumn("dbo.PromoProduct", "EAN_PC", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            RenameColumn("dbo.PromoProduct", "EAN_Case", "EAN");
            DropColumn("dbo.PromoProduct", "EAN_PC");
            RenameColumn("dbo.Product", "EAN_Case", "EAN");
            DropColumn("dbo.Product", "EAN_PC");
        }
    }
}
