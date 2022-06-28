using Core.Data;
using Interfaces.Implementation.Import.FullImport;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportEventAction : FullXLSXImportAction
    {
        private Guid roleId;
        private Guid userId;

        public FullXLSXUpdateImportEventAction(FullImportSettings settings, Guid roleId, Guid userId) : base(settings)
        {
            this.roleId = roleId;
            this.userId = userId;
        }
        protected override bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors)
        {
            errors = new List<string>();
            bool isSuitable = true;


            Event typedRec = (Event)rec;
            if (String.IsNullOrEmpty(typedRec.Name)) { errors.Add("Event Name must have a value"); isSuitable = false; }
            if (String.IsNullOrEmpty(typedRec.EventType.Name)) { errors.Add("Event Type must have a value"); isSuitable = false; }
            var eventtypes = new List<string> { "Customer", "National" };
            if (!eventtypes.Contains(typedRec.EventType.Name)) { errors.Add("Event Type must have the following values - Customer or National"); isSuitable = false; }
            var segments = new List<string> { "Catcare", "Dogcare", "" };
            if (!segments.Contains(typedRec.MarketSegment)) { errors.Add("Event MarketSegment must have the following values - Catcare, Dogcare, or empty for All"); isSuitable = false; }
            if (typedRec.EventType.Name == "Customer" && !string.IsNullOrEmpty(typedRec.MarketSegment))
            {
                errors.Add("Event MarketSegment must by empty if type Customer"); isSuitable = false;
            }


            return isSuitable;
        }
        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> importedRecords, DatabaseContext databaseContext)
        {

            // все импортируемые Events
            var importedEvents = importedRecords.Cast<Event>();
            List<Event> events = databaseContext.Set<Event>().Where(g => !g.Disabled).ToList();
            //List<EventType> eventTypes = databaseContext.Set<EventType>().Where(g => !g.Disabled).ToList();
            foreach (var newRecord in importedEvents)
            {
                Event oldRecord = events.FirstOrDefault(x => x.Name == newRecord.Name);
                if (oldRecord == null)
                {
                    databaseContext.Set<Event>().Add(newRecord);
                }
                else
                {
                    oldRecord.Description = newRecord.Description;
                    oldRecord.MarketSegment = newRecord.MarketSegment;
                    oldRecord.EventType = newRecord.EventType;
                }
            }

            var saveChangesCount = databaseContext.SaveChanges();
            return saveChangesCount;
        }

    }
}
