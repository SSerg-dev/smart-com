namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoViewMode : DbMigration
    {
        public override void Up()
        {
            Sql("DROP VIEW [PromoView]");
            Sql(@"CREATE VIEW [PromoView] 
                AS SELECT pr.[Id], pr.[Name], mmc.[Name] as MarsMechanicName, mmt.[Name] as MarsMechanicTypeName, pr.[MarsMechanicDiscount], 
                cl.[SystemName] as ColorSystemName, ps.[Color] as PromoStatusColor, ps.[SystemName] as PromoStatusSystemName, pr.[CreatorId], 
                pr.[ClientTreeId], pr.[BaseClientTreeIds], pr.[StartDate], pr.[EndDate], pr.[DispatchesStart], pr.[CalendarPriority], pr.[Number], 
                bt.[Name] as BrandTechName, ev.[Name] as EventName, pr.[InOut]

                FROM [Promo] pr 
                LEFT JOIN PromoStatus ps ON pr.PromoStatusId = ps.Id
                LEFT JOIN Color cl ON pr.ColorId = cl.Id 
                LEFT JOIN Mechanic mmc ON pr.MarsMechanicId = mmc.Id 
                LEFT JOIN MechanicType mmt ON pr.MarsMechanicTypeId = mmt.Id 
                LEFT JOIN [Event] ev ON pr.EventId = ev.Id 
                LEFT JOIN BrandTech bt ON pr.BrandTechId = bt.Id");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW [PromoView]");
            Sql(@"CREATE VIEW [PromoView] 
                AS SELECT pr.[Id], pr.[Name], mmc.[Name] as MarsMechanicName, mmt.[Name] as MarsMechanicTypeName, pr.[MarsMechanicDiscount], 
                cl.[SystemName] as ColorSystemName, ps.[Color] as PromoStatusColor, ps.[SystemName] as PromoStatusSystemName, pr.[CreatorId], 
                pr.[ClientTreeId], pr.[BaseClientTreeIds], pr.[StartDate], pr.[EndDate], pr.[DispatchesStart], pr.[CalendarPriority], pr.[Number], 
                bt.[Name] as BrandTechName, ev.[Name] as EventName

                FROM [TPM_Dev].[Promo] pr 
                LEFT JOIN PromoStatus ps ON pr.PromoStatusId = ps.Id
                LEFT JOIN Color cl ON pr.ColorId = cl.Id 
                LEFT JOIN Mechanic mmc ON pr.MarsMechanicId = mmc.Id 
                LEFT JOIN MechanicType mmt ON pr.MarsMechanicTypeId = mmt.Id 
                LEFT JOIN [Event] ev ON pr.EventId = ev.Id 
                LEFT JOIN BrandTech bt ON pr.BrandTechId = bt.Id");
        }
    }
}
