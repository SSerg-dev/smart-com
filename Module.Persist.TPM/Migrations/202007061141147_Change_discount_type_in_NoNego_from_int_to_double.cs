namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_discount_type_in_NoNego_from_int_to_double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NoneNego", "Discount", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NoneNego", "Discount", c => c.Int());
        }
    }
}
