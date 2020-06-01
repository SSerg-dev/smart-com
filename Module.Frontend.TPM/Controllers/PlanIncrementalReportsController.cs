﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

using Core.MarsCalendar;
using Core.Security;
using Core.Security.Models;

using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;

using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;

using Persist.Model;

using Thinktecture.IdentityModel.Authorization.WebApi;

using Utility;

namespace Module.Frontend.TPM.Controllers
{

    public class PlanIncrementalReportsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PlanIncrementalReportsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        //[ClaimsAuthorize]
        //[EnableQuery(MaxNodeCount = int.MaxValue)]
        //public SingleResult<PlanIncrementalReport> GetPlanIncrementalReport([FromODataUri] System.Guid key) {
        //    return SingleResult.Create(GetConstraintedQuery());
        //}

        public IQueryable<PlanIncrementalReport> GetConstraintedQuery(bool forExport = false)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            IQueryable<PlanIncrementalReport> query = Context.Set<PlanIncrementalReport>();

            //query = ModuleApplyFilterHelper.ApplyFilter(query, Context, hierarchy, filters);

            query = SetWeekByMarsDates(query);

            if (!forExport)
            {
                query = JoinWeeklyDivision(query);
            }

            query = ModuleApplyFilterHelper.ApplyFilter(query, Context, hierarchy, filters);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanIncrementalReport> GetPlanIncrementalReports(ODataQueryOptions<PlanIncrementalReport> queryOptions = null)
        {
            var query = GetConstraintedQuery();
            if (queryOptions != null && queryOptions.Filter != null)
            {
                query = RoundingHelper.ModifyQuery(query);
            }
            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PlanIncrementalReport> GetFilteredData(ODataQueryOptions<PlanIncrementalReport> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PlanIncrementalReport>(options.Context, Request, HttpContext.Current.Request);
            return RoundingHelper.ModifyQuery(optionsPost.ApplyTo(query, querySettings) as IQueryable<PlanIncrementalReport>);
        }

        private IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 1, Field = "ZREP", Header = "ZREP", Quoting = false },
                new Column() { Order = 2, Field = "DemandCode", Header = "Demand Code", Quoting = false },
                new Column() { Order = 3, Field = "PromoNameId", Header = "Promo Name  #Promo Id", Quoting = false },
                new Column() { Order = 4, Field = "LocApollo", Header = "Loc", Quoting = false },
                new Column() { Order = 5, Field = "TypeApollo", Header = "Type", Quoting = false },
                new Column() { Order = 6, Field = "ModelApollo", Header = "Model", Quoting = false },
                new Column() { Order = 7, Field = "WeekStartDate", Header = "Week Start Date", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 8, Field = "PlanProductIncrementalCaseQty", Header = "Plan Product Incremental Qty", Quoting = false, Format = "0.00"},
                new Column() { Order = 9, Field = "PlanUplift", Header = "Uplift Plan", Quoting = false, Format = "0.00" },
                new Column() { Order = 10, Field = "DispatchesStart", Header = "Dispatches Start", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 11, Field = "DispatchesEnd", Header = "Dispatches End", Quoting = false, Format = "dd.MM.yyyy" },
                new Column() { Order = 12, Field = "Week", Header = "Week", Quoting = false },
                new Column() { Order = 13, Field = "Status", Header = "Status", Quoting = false },
                new Column() { Order = 14, Field = "PlanProductBaselineCaseQty", Header = "Plan Product Baseline Case Qty", Quoting = false, Format = "0.00" },
                new Column() { Order = 15, Field = "PlanProductIncrementalLSV", Header = "Plan Product Incremental LSV", Quoting = false, Format = "0.00" },
                new Column() { Order = 16, Field = "PlanProductBaselineLSV", Header = "Plan Product Baseline LSV", Quoting = false, Format = "0.00" },
                new Column() { Order = 17, Field = "InOut", Header = "InOut", Quoting = false },
                new Column() { Order = 18, Field = "IsGrowthAcceleration", Header = "Growth Acceleration", Quoting = false },
            };
            return columns;
        }
        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PlanIncrementalReport> options)
        {
            try
            {
                IQueryable results = options.ApplyTo(GetConstraintedQuery(true));
                IEnumerable<Column> columns = GetExportSettings();
                NonGuidIdExporter exporter = new NonGuidIdExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("PlanIncrementalReport", username);
                exporter.Export(results, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            }
            catch (Exception e)
            {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private bool EntityExists(Guid key)
        {
            return Context.Set<PromoProduct>().Count(e => e.Id == key) > 0;
        }

        //Простановка дат в формате Mars в поле Week
        public IQueryable<PlanIncrementalReport> SetWeekByMarsDates(IQueryable<PlanIncrementalReport> query)
        {
            List<PlanIncrementalReport> result = new List<PlanIncrementalReport>(query);
            string stringFormatYP2W = "{0}P{1:D2}W{2}";
            foreach (PlanIncrementalReport item in result)
            {
                if (item.WeekStartDate != null)
                {
                    item.Week = (new MarsDate((DateTimeOffset)item.WeekStartDate)).ToString(stringFormatYP2W);
                }
            }
            return result.AsQueryable();
        }

        public IQueryable<PlanIncrementalReport> JoinWeeklyDivision(IQueryable<PlanIncrementalReport> query)
        {
            List<PlanIncrementalReport> result = new List<PlanIncrementalReport>();

            var temp = query.GroupBy(x => new { x.PromoNameId, x.ZREP });

            foreach (var group in temp)
            {
                PlanIncrementalReport toAdd = null;
                foreach (var item in group)
                {
                    if (toAdd == null)
                    {
                        toAdd = (PlanIncrementalReport)item.Clone();
                        toAdd.PlanProductBaselineCaseQty = 0;
                        toAdd.PlanProductBaselineLSV = 0;
                        toAdd.PlanProductIncrementalCaseQty = 0;
                        toAdd.PlanProductIncrementalLSV = 0;
                    }
                    toAdd.PlanProductBaselineCaseQty += item.PlanProductBaselineCaseQty;
                    toAdd.PlanProductBaselineLSV += item.PlanProductBaselineLSV;
                    toAdd.PlanProductIncrementalCaseQty += item.PlanProductIncrementalCaseQty;
                    toAdd.PlanProductIncrementalLSV += item.PlanProductIncrementalLSV;

                    if (DateTimeOffset.Compare((DateTimeOffset)toAdd.WeekStartDate, (DateTimeOffset)item.WeekStartDate) > 0)
                    {
                        toAdd.WeekStartDate = item.WeekStartDate;
                        toAdd.Week = item.Week;

                    }
                }
                result.Add(toAdd);
            }
            return result.AsQueryable();
        }
    }
}
