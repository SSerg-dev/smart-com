namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Qty_Price_BaselineLSV_from_BaseLine : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BaseLine", "QTY");
            DropColumn("dbo.BaseLine", "Price");
            DropColumn("dbo.BaseLine", "BaselineLSV");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseLine", "BaselineLSV", c => c.Double(nullable: false));
            AddColumn("dbo.BaseLine", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.BaseLine", "QTY", c => c.Double(nullable: false));
        }
    }
}
