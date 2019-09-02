namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_LastChangeDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "LastChangedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Promo", "LastChangedDateDemand", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Promo", "LastChangedDateFinance", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "LastChangedDateFinance");
            DropColumn("dbo.Promo", "LastChangedDateDemand");
            DropColumn("dbo.Promo", "LastChangedDate");
        }
    }
}
