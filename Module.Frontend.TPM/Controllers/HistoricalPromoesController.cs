﻿using Core.History;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers {

    public class HistoricalPromoesController : ODataController {
        [Inject]
        public IHistoryReader HistoryReader { get; set; }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalPromo> GetHistoricalPromoes(Guid? promoIdHistory)
        {
            return HistoryReader.GetById<HistoricalPromo>(promoIdHistory.ToString());// HistoryReader.GetAll<HistoricalPromo>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalPromo> GetHistoricalPromoes(Guid? promoIdHistory, bool onlyCreator)
        {
            var result = HistoryReader.GetById<HistoricalPromo>(promoIdHistory.ToString());// HistoryReader.GetAll<HistoricalPromo>();
            result = result.Where(x => x.CreatorLogin != null);
            return result;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
