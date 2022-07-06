namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Mode : DbMigration
    {
        public override void Up()
        {
            AddColumn("Jupiter.Promo", "TPMmode", c => c.Int(nullable: false));
            AddColumn("Jupiter.PromoProduct", "TPMmode", c => c.Int(nullable: false));
            AddColumn("Jupiter.PromoSupportPromo", "TPMmode", c => c.Int(nullable: false));
            AddColumn("Jupiter.PromoProductTree", "TPMmode", c => c.Int(nullable: false));
            AddColumn("Jupiter.PromoProductsCorrection", "TPMmode", c => c.Int(nullable: false));
            AddColumn("Jupiter.BTLPromo", "TPMmode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Jupiter.BTLPromo", "TPMmode");
            DropColumn("Jupiter.PromoProductsCorrection", "TPMmode");
            DropColumn("Jupiter.PromoProductTree", "TPMmode");
            DropColumn("Jupiter.PromoSupportPromo", "TPMmode");
            DropColumn("Jupiter.PromoProduct", "TPMmode");
            DropColumn("Jupiter.Promo", "TPMmode");
        }
    }
}
