using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Module.Persist.TPM.Utils;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.PromoStateControl;
using Module.Frontend.TPM.Util;
using Frontend.Core.Extensions.Export;
using System.Web.Http.OData.Query;
using Persist.ScriptGenerator.Filter;

namespace Module.Frontend.TPM.Controllers {
    public class PromoGridViewsController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PromoGridViewsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoGridView> GetConstraintedQuery(bool canChangeStateOnly = false) {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoGridView> query = Context.Set<PromoGridView>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters, FilterQueryModes.Active, canChangeStateOnly ? role : String.Empty);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator") {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }

        protected IQueryable<Promo> GetFullConstraintedQuery() {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<Promo> query = Context.Set<Promo>().AsNoTracking();
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatus.SystemName != "Draft" || e.CreatorId == user.Id);
            }
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<PromoGridView> GetPromoGridView([FromODataUri] Guid key) {
            return SingleResult.Create(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoGridView> GetPromoGridViews(bool canChangeStateOnly = false) {
            return GetConstraintedQuery(canChangeStateOnly);
        }

        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] Guid key) {
            try {
                var model = Context.Set<Promo>().Find(key);
                if (model == null) {
                    return NotFound();
                }

                Promo promoCopy = new Promo(model);

                model.DeletedDate = DateTime.Now;
                model.Disabled = true;
                model.PromoStatusId = Context.Set<PromoStatus>().FirstOrDefault(e => e.SystemName == "Deleted").Id;

                UserInfo user = authorizationManager.GetCurrentUser();
                string userRole = user.GetCurrentRole().SystemName;

                string message;

                PromoStateContext promoStateContext = new PromoStateContext(Context, promoCopy);
                bool status = promoStateContext.ChangeState(model, userRole, out message);

                if (!status) {
                    return InternalServerError(new Exception(message));
                }
                List<PromoProduct> promoProductToDeleteList = Context.Set<PromoProduct>().Where(x => x.PromoId == model.Id && !x.Disabled).ToList();
                foreach (PromoProduct promoProduct in promoProductToDeleteList)
                {
                    promoProduct.DeletedDate = System.DateTime.Now;
                    promoProduct.Disabled = true;
                }
                model.NeedRecountUplift = true;
                //необходимо удалить все коррекции
                var promoProductToDeleteListIds = promoProductToDeleteList.Select(x => x.Id).ToList();
                List<PromoProductsCorrection> promoProductCorrectionToDeleteList = Context.Set<PromoProductsCorrection>()
                    .Where(x => promoProductToDeleteListIds.Contains(x.PromoProductId) && x.Disabled != true).ToList();
                foreach (PromoProductsCorrection promoProductsCorrection in promoProductCorrectionToDeleteList)
                {
                    promoProductsCorrection.DeletedDate = DateTimeOffset.UtcNow;
                    promoProductsCorrection.Disabled = true;
                    promoProductsCorrection.UserId = (Guid)user.Id;
                    promoProductsCorrection.UserName = user.Login;
                }
                Context.SaveChanges();

                PromoCalculateHelper.RecalculateBudgets(model, user, Context);
                PromoHelper.WritePromoDemandChangeIncident(Context, model, true);

                //если промо инаут, необходимо убрать записи в IncrementalPromo при отмене промо
                if (model.InOut.HasValue && model.InOut.Value)
                {
                    PromoHelper.DisableIncrementalPromo(Context, model);
                }

                return StatusCode(HttpStatusCode.OK);
            } catch (Exception e) {
                return InternalServerError(e);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PromoGridView> options) {
            try {
                // TODO: Применять options сразу к Promo
                IQueryable<Guid> results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled)).Cast<PromoGridView>().Select(p=>p.Id);
                IQueryable<Promo> fullResults = GetFullConstraintedQuery().Where(x => !x.Disabled).Where(x => results.Contains(x.Id));
                IEnumerable<Column> columns = PromoHelper.GetExportSettings();
                XLSXExporter exporter = new XLSXExporter(columns);
                UserInfo user = authorizationManager.GetCurrentUser();
                string username = user == null ? "" : user.Login;
                string filePath = exporter.GetExportFileName("Promo", username);
                exporter.Export(fullResults, filePath);
                string filename = System.IO.Path.GetFileName(filePath);
                return Content<string>(HttpStatusCode.OK, filename);
            } catch (Exception e) {
                return Content<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
