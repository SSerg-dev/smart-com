namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePromoMechanicLength1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "Mechanic", c => c.String(maxLength: 255));
            AlterColumn("dbo.Promo", "MechanicIA", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "MechanicIA", c => c.String(maxLength: 100));
            AlterColumn("dbo.Promo", "Mechanic", c => c.String(maxLength: 100));
        }
    }
}
