using System;
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

			query = ModuleApplyFilterHelper.ApplyFilter(query, Context, hierarchy, filters);
			
			if (!forExport)
				query = JoinWeeklyDivision(query);
						
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
		
		public IQueryable<PlanIncrementalReport> JoinWeeklyDivision(IQueryable<PlanIncrementalReport> query)
		{
			List<PlanIncrementalReport> result = new List<PlanIncrementalReport>();

			List<GroupedPlanValues> grouped = query
							.GroupBy(q => new { q.PromoNameId, q.ZREP })
							.Select(q => new GroupedPlanValues()
							{
								PlanProductBaselineCaseQty = q.Sum(s => s.PlanProductBaselineCaseQty),
								PlanProductBaselineLSV = q.Sum(s => s.PlanProductBaselineLSV),
								PlanProductIncrementalCaseQty = q.Sum(s => s.PlanProductIncrementalCaseQty),
								PlanProductIncrementalLSV = q.Sum(s => s.PlanProductIncrementalLSV),
								WeekStartDate = q.Min(m => m.WeekStartDate),
								Week = q.Min(m => m.Week),
								Pir = q.Select(i => i).FirstOrDefault()
							}).ToList();
			grouped.ForEach(item => 
			{
				PlanIncrementalReport toAdd = (PlanIncrementalReport)item.Pir.Clone();

				toAdd.PlanProductBaselineCaseQty += item.PlanProductBaselineCaseQty;
				toAdd.PlanProductBaselineLSV += item.PlanProductBaselineLSV;
				toAdd.PlanProductIncrementalCaseQty += item.PlanProductIncrementalCaseQty;
				toAdd.PlanProductIncrementalLSV += item.PlanProductIncrementalLSV;
				toAdd.WeekStartDate = item.WeekStartDate;
				toAdd.Week = item.Week;

				result.Add(toAdd);
			});

			return result.AsQueryable();
		}

		/// <summary>
		/// Класс для суммирования плановых показателей сгруппированных по PromoNameId, ZREP объектов PlanIncrementalReport 
		/// </summary>
		private class GroupedPlanValues
		{
			public double? PlanProductBaselineCaseQty { get; set; }
			public double? PlanProductBaselineLSV { get; set; }
			public double? PlanProductIncrementalCaseQty { get; set; }
			public double? PlanProductIncrementalLSV { get; set; }
			public DateTimeOffset? WeekStartDate { get; set; }
			public string Week { get; set; }
			public PlanIncrementalReport Pir { get; set; }
		}
	}
}
