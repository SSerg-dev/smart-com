namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PlanInStoreShelfPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "PlanInStoreShelfPrice", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "PlanInStoreShelfPrice");
        }
    }
}
