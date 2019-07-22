namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_FieldLoadFromTLC_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "LoadFromTLC", c => c.Boolean(nullable: false));
            Sql(@"UPDATE Promo 
                    SET Promo.LoadFromTLC = 'True' 
                    WHERE Promo.Id IN (
                        Select P1.Id From Promo P1, PromoStatus
                            Where (Select Count(*) From PromoProduct Where PromoProduct.PromoId = P1.Id) = 0
                                AND P1.PromoStatusId = PromoStatus.Id And LOWER(PromoStatus.SystemName) IN('started', 'finished', 'closed')
                    );");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "LoadFromTLC");
        }
    }
}
