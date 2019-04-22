namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_AddColumn_MechanicIA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "MechanicIA", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "MechanicIA");
        }
    }
}
