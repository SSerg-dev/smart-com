using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{
	public class PlanIncrementalReportsController : EFContextController
	{
		private readonly UserInfo user;
		private readonly string role;
		private readonly Guid? roleId;

		public PlanIncrementalReportsController(IAuthorizationManager authorizationManager)
		{
			user = authorizationManager.GetCurrentUser();
			var roleInfo = authorizationManager.GetCurrentRole();
			role = roleInfo.SystemName;
			roleId = roleInfo.Id;
		}

		public PlanIncrementalReportsController(UserInfo User, string Role, Guid RoleId)
		{
			user = User;
			role = Role;
			roleId = RoleId;
		}

		public IQueryable<PlanIncrementalReport> GetConstraintedQuery(bool forExport = false, DatabaseContext context = null)
		{
			PerformanceLogger performanceLogger = new PerformanceLogger();
			performanceLogger.Start();
			if (context == null)
            {
				context = Context;
			}
			IList<Constraint> constraints = user.Id.HasValue ? context.Constraints
				.Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
				.ToList() : new List<Constraint>();

			IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
			PerformanceLogger logger = new PerformanceLogger();
			logger.Start();
			IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();

			IQueryable<PlanIncrementalReport> query = context.Set<PlanIncrementalReport>();

			query = ModuleApplyFilterHelper.ApplyFilter(query, context, hierarchy, filters, forExport);
			logger.Stop("Context");

			if (!forExport)
				query = JoinWeeklyDivision(query);

			performanceLogger.Stop("Full");
			return query;
		}


		[ClaimsAuthorize]
		[EnableQuery(MaxNodeCount = int.MaxValue)]
		public IQueryable<PlanIncrementalReport> GetPlanIncrementalReports(ODataQueryOptions<PlanIncrementalReport> queryOptions = null)
		{
			var query = GetConstraintedQuery();
			 
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

		public static IEnumerable<Column> GetExportSettings()
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
			Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
			var url = HttpContext.Current.Request.Url.AbsoluteUri;
			using (DatabaseContext context = new DatabaseContext())
			{
				HandlerData data = new HandlerData();
				string handlerName = "ExportHandler";

				HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PlanIncrementalReport), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PlanIncrementalReportsController), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PlanIncrementalReportsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
				HandlerDataHelper.SaveIncomingArgument("URL", url, data, visible: false, throwIfNotExists: false);
				
				LoopHandler handler = new LoopHandler()
				{
					Id = Guid.NewGuid(),
					ConfigurationName = "PROCESSING",
					Description = $"Export {nameof(PlanIncrementalReport)} dictionary",
					Name = "Module.Host.TPM.Handlers." + handlerName,
					ExecutionPeriod = null,
					CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
					LastExecutionDate = null,
					NextExecutionDate = null,
					ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
					UserId = userId,
					RoleId = roleId
				};
				handler.SetParameterData(data);
				context.LoopHandlers.Add(handler);
				context.SaveChanges();
			}

			return Content(HttpStatusCode.OK, "success");
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

				toAdd.PlanProductBaselineCaseQty = item.PlanProductBaselineCaseQty;
				toAdd.PlanProductBaselineLSV = item.PlanProductBaselineLSV;
				toAdd.PlanProductIncrementalCaseQty = item.PlanProductIncrementalCaseQty;
				toAdd.PlanProductIncrementalLSV = item.PlanProductIncrementalLSV;
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
