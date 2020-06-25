namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DeviationCoefficient_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "DeviationCoefficient", c => c.Double(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "DeviationCoefficient");
        }
    }
}
