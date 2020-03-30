﻿using Core.History;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class HistoricalClientDashboardsController : ODataController
    {
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
        public IQueryable<HistoricalClientDashboardView> GetHistoricalClientDashboards()
        {
            return HistoryReader.GetAll<HistoricalClientDashboardView>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalClientDashboardView> GetHistoricalClientDashboards(Guid? Id)
        {
            if (Id == null)
            {
                return new List<HistoricalClientDashboardView>().AsQueryable();
            }
            else
            {
                return HistoryReader.GetAllById<HistoricalClientDashboardView>(Id.ToString());
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalClientDashboardView> GetFilteredData(ODataQueryOptions<HistoricalClientDashboardView> options)
        {
            var query = Enumerable.Empty<HistoricalClientDashboardView>().AsQueryable();
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            if (isArgumentExists)
            {
                Guid? id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
                query = id == null
                    ? new List<HistoricalClientDashboardView>().AsQueryable()
                    : HistoryReader.GetAllById<HistoricalClientDashboardView>(id.ToString());
            }
            else
            {
                query = HistoryReader.GetAll<HistoricalClientDashboardView>();
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalClientDashboardView>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalClientDashboardView>;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
