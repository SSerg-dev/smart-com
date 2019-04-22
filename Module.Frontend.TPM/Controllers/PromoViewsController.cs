using AutoMapper;
using Core.Data;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions.Export;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.DTO;

namespace Module.Frontend.TPM.Controllers {
    public class PromoViewsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PromoViewsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoView> GetConstraintedQuery() {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoView> query = Context.Set<PromoView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator") {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PromoView> GetPromoView([FromODataUri] Guid key) {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoView> GetPromoViews() {
            return GetConstraintedQuery();
        }

        private bool EntityExists(Guid key) {
            return Context.Set<PromoView>().Count(e => e.Id == key) > 0;
        }

        ///// <summary>
        ///// Экспорт календаря в эксель
        ///// </summary>
        ///// <param name="options"></param>
        ///// <param name="data">clients - список id клиентов соответствующих фильтру на клиенте, year - год</param>
        ///// <returns></returns>
        //[ClaimsAuthorize]
        //[HttpPost]
        //public IHttpActionResult ExportSchedule(ODataQueryOptions<Promo> options, ODataActionParameters data) {
        //    try {
        //        IQueryable results = options.ApplyTo(GetConstraintedQuery());
        //        List<Promo> promoes = CastQueryToPromo(results);
        //        DateTime startDate = DateTime.Now;
        //        DateTime endDate = DateTime.Now;
        //        bool yearExport = false;
        //        if (data.Count() > 1) {
        //            int year = (int) data["year"];
        //            startDate = new DateTime(year, 1, 1);
        //            endDate = new DateTime(year, 12, 31);
        //            promoes = promoes.Where(p => (p.EndDate > startDate && p.EndDate < endDate) || (p.StartDate > startDate && p.StartDate < endDate)).ToList();
        //            if (promoes.Count == 0) {
        //                return Content<string>(HttpStatusCode.NotImplemented, "No promo available for export");
        //            }
        //            yearExport = true;
        //        }
        //        IEnumerable<int> filteredClients = (IEnumerable<int>) data["clients"];
        //        UserInfo user = authorizationManager.GetCurrentUser();
        //        string username = user == null ? "" : user.Login;
        //        SchedulerExporter exporter = yearExport ? new SchedulerExporter(startDate, endDate) : new SchedulerExporter();
        //        string filePath = exporter.GetExportFileName(username);
        //        exporter.Export(promoes, filteredClients, filePath, Context);
        //        string filename = System.IO.Path.GetFileName(filePath);
        //        return Content<string>(HttpStatusCode.OK, filename);
        //    } catch (Exception e) {
        //        return Content<string>(HttpStatusCode.InternalServerError, e.Message);
        //    }
        //}

        //private string GetUserName(string userName) {
        //    string[] userParts = userName.Split(new char[] { '/', '\\' });
        //    return userParts[userParts.Length - 1];
        //}

        ///// <summary>
        ///// Преобразование записей в модель Promo
        ///// </summary>
        ///// <param name="records"></param>
        ///// <returns></returns>
        //private List<Promo> CastQueryToPromo(IQueryable records) {
        //    List<Promo> castedPromoes = new List<Promo>();
        //    Promo proxy = Context.Set<Promo>().Create<Promo>();
        //    foreach (var item in records) {
        //        if (item is IEntity<Guid>) {
        //            Promo result = (Promo) Mapper.Map(item, proxy, typeof(Promo), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
        //            castedPromoes.Add(result);
        //        } else if (item is ISelectExpandWrapper) {
        //            var property = item.GetType().GetProperty("Instance");
        //            var instance = property.GetValue(item);
        //            Promo val = null;
        //            if (instance is Promo) {
        //                val = (Promo) instance;
        //                castedPromoes.Add(val);
        //            }
        //        }
        //    }
        //    return castedPromoes;
        //}
    }
}
