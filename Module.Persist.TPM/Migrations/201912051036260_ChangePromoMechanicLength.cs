namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePromoMechanicLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promo", "Mechanic", c => c.String(maxLength: 100));
            AlterColumn("dbo.Promo", "MechanicIA", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promo", "MechanicIA", c => c.String(maxLength: 20));
            AlterColumn("dbo.Promo", "Mechanic", c => c.String(maxLength: 20));
        }
    }
}
