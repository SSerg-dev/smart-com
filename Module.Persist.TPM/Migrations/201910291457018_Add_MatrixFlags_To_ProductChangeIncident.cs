namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MatrixFlags_To_ProductChangeIncident : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductChangeIncident", "IsCreateInMatrix", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductChangeIncident", "IsDeleteInMatrix", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductChangeIncident", "IsDeleteInMatrix");
            DropColumn("dbo.ProductChangeIncident", "IsCreateInMatrix");
        }
    }
}
