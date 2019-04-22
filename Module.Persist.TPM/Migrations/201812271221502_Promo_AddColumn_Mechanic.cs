namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_AddColumn_Mechanic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "Mechanic", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "Mechanic");
        }
    }
}
