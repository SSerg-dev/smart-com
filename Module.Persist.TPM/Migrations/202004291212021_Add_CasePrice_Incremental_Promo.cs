namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CasePrice_Incremental_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncrementalPromo", "CasePrice", c => c.Double());
        }

        public override void Down()
        {
            DropColumn("dbo.IncrementalPromo", "CasePrice");
        }
    }
}
