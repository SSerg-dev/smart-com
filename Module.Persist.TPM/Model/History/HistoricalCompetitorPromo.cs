﻿using System;
using Core.History;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CompetitorPromo))]
    public class HistoricalCompetitorPromo : BaseHistoricalEntity<Guid>
    {
        public string CompetitorName { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public int ClientTreeObjectId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string MechanicType { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public string CompetitorBrandTechBrandTech { get; set; }
    }
}
