namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoTypes_Glypg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoTypes", "Glyph", c => c.String());
             
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.PromoTypes", "Glyph");
        }
    }
}
