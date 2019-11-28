namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AverageMarker_PromoProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "AverageMarker", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "AverageMarker");
        }
    }
}
