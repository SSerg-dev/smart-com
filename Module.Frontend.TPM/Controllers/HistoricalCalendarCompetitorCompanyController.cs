﻿using Core.History;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class HistoricalCalendarCompetitorCompanyController : ODataController
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
        public IQueryable<HistoricalCalendarCompetitorCompany> GetHistoricalCalendarCompetitorCompanies(Guid? Id) =>
            HistoryReader.GetAllById<HistoricalCalendarCompetitorCompany>(Id.ToString());

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalCalendarCompetitorCompany> GetFilteredData(ODataQueryOptions<HistoricalCalendarCompetitorCompany> options)
        {
            IQueryable<HistoricalCalendarCompetitorCompany> query;
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            if (isArgumentExists)
            {
                Guid? id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
                query = HistoryReader.GetAllById<HistoricalCalendarCompetitorCompany>(id.ToString());
            }
            else
            {
                query = HistoryReader.GetAll<HistoricalCalendarCompetitorCompany>();
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalCalendarCompetitorCompany>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalCalendarCompetitorCompany>;
        }
    }
}
