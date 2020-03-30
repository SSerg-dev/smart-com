using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;

namespace Module.Frontend.TPM.Controllers
{

	public class DeletedPromoSupportsController : EFContextController
	{
		private readonly IAuthorizationManager authorizationManager;

		public DeletedPromoSupportsController(IAuthorizationManager authorizationManager)
		{
			this.authorizationManager = authorizationManager;
		}

		protected IQueryable<PromoSupport> GetConstraintedQuery()
		{
			UserInfo user = authorizationManager.GetCurrentUser();
			string role = authorizationManager.GetCurrentRoleName();
			IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
				.Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
				.ToList() : new List<Constraint>();

			IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
			IQueryable<PromoSupport> query = Context.Set<PromoSupport>();
			IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

			query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

			return query;
		}

		[ClaimsAuthorize]
		[EnableQuery(MaxNodeCount = int.MaxValue)]
		public IQueryable<PromoSupport> GetDeletedPromoSupports()
		{
			return GetConstraintedQuery().Where(e => e.Disabled);
		}
		[ClaimsAuthorize]
		[EnableQuery(MaxNodeCount = int.MaxValue)]
		public SingleResult<PromoSupport> GetDeletedPromoStatus([FromODataUri] System.Guid key)
		{
			return SingleResult.Create(GetConstraintedQuery()
				.Where(e => e.Id == key)
				.Where(e => e.Disabled));
		}

		[ClaimsAuthorize]
		[HttpPost]
		public IQueryable<PromoSupport> GetFilteredData(ODataQueryOptions<PromoSupport> options)
		{
			var query = GetConstraintedQuery().Where(e => e.Disabled);

			var querySettings = new ODataQuerySettings
			{
				EnsureStableOrdering = false,
				HandleNullPropagation = HandleNullPropagationOption.False
			};

			var optionsPost = new ODataQueryOptionsPost<PromoSupport>(options.Context, Request, HttpContext.Current.Request);
			return optionsPost.ApplyTo(query, querySettings) as IQueryable<PromoSupport>;
		}
	}

}
