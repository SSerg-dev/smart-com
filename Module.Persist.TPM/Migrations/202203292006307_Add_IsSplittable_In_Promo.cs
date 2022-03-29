namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsSplittable_In_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("jupiter_test.Promo", "IsSplittable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("jupiter_test.Promo", "IsSplittable");
        }
    }
}
