namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Column_Into_Baseline_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseLine", "InputBaselineQTY", c => c.Double());
            AddColumn("dbo.BaseLine", "SellInBaselineQTY", c => c.Double());
            AddColumn("dbo.BaseLine", "SellOutBaselineQTY", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseLine", "SellOutBaselineQTY");
            DropColumn("dbo.BaseLine", "SellInBaselineQTY");
            DropColumn("dbo.BaseLine", "InputBaselineQTY");
        }
    }
}