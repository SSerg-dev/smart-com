using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Model;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Model.DTO;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers {
    public class PromoStatusChangesController : EFContextController {
        private readonly IAuthorizationManager authorizationManager;

        public PromoStatusChangesController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoStatusChange> GetConstraintedQuery() {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<PromoStatusChange> query = Context.Set<PromoStatusChange>();
            return query;
        }

        protected IQueryable<PromoStatusChange> GetConstraintedByPromoQuery(Guid? promoKey) {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<PromoStatusChange> query = Context.Set<PromoStatusChange>().Where(e => e.PromoId == promoKey);
            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult PromoStatusChangesByPromo(String promoKey) {
            try {
				// Цвета для статусов
				Dictionary<string, string> statusColors = new Dictionary<string, string>();
				var promoStatuses = Context.Set<PromoStatus>().Where(x => !x.Disabled);
				foreach (PromoStatus promoStatus in promoStatuses)
				{
					if (promoStatus.Color == null) promoStatus.Color = "#ffffff";
					statusColors.Add(promoStatus.SystemName, promoStatus.Color);
				}

				Guid promoId = Guid.Empty;
				bool isGuid = Guid.TryParse(promoKey, out promoId);
				if (isGuid)
				{
					Promo promoModel = Context.Set<Promo>().Where(x => x.Id == promoId).First();
					IQueryable<PromoStatusChange> pscs = GetConstraintedByPromoQuery(Guid.Parse(promoKey));
					var pscsList = pscs.OrderByDescending(y => y.Date).ToList();
					foreach (var item in pscsList)
					{
						var user = Context.Set<User>().FirstOrDefault(x => x.Id == item.UserId);
						if (user != null)
						{
							item.UserName = user.Name;
						}
						var role = Context.Set<Role>().FirstOrDefault(x => x.Id == item.RoleId);
						if (role != null)
						{
							item.RoleName = role.DisplayName;
						}
						var status = item.PromoStatus;
						if (status != null)
						{
							item.StatusColor = status.Color;
							item.StatusName = status.Name;
						}
					}
					if (promoModel.PromoStatus.SystemName == "OnApproval")
					{
						bool isNoNegoPassed = CheckNoNego(promoModel);
						return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, isEmpty = false, data = pscsList, statusColors, isNoNegoPassed }));
					}
					else
					{
						return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, isEmpty = false, data = pscsList, statusColors }));
					}
				}
				else
				{
					List<PromoStatusChange> pscs = new List<PromoStatusChange>();
					return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, isEmpty = true, statusColors }));
				}
				
			} catch (DbUpdateException e) {
                return InternalServerError(new Exception(e.Message));
            }
		}

		private bool CheckNoNego(Promo model)
		{
			List<NoneNego> noNegoList = Context.Set<NoneNego>().Where(x => !x.Disabled && x.FromDate <= model.StartDate && x.ToDate >= model.EndDate).ToList();

			ClientTreeHierarchyView clientTreeHierarchy = Context.Set<ClientTreeHierarchyView>().FirstOrDefault(x => x.Id == model.ClientTreeId);

			// может быть выбрано несколько продуктов (subrange) в промо
			int[] productObjectIds = Context.Set<PromoProductTree>().Where(n => n.PromoId == model.Id && !n.Disabled).Select(n => n.ProductTreeObjectId).ToArray();
			ProductTreeHierarchyView[] productTreeHierarchies = Context.Set<ProductTreeHierarchyView>().Where(x => productObjectIds.Contains(x.Id)).ToArray();

			foreach (ProductTreeHierarchyView prodHierarchy in productTreeHierarchies)
			{
				bool resultForProduct = false;
				string productHierarchy = prodHierarchy.Hierarchy + "." + prodHierarchy.Id.ToString();
				int[] productHierarchyArr = Array.ConvertAll(productHierarchy.Split('.'), int.Parse);

				for (int i = (productHierarchyArr.Length - 1); i > 0 && !resultForProduct; i--)
				{
					string clientHierarchy = clientTreeHierarchy.Hierarchy + "." + model.ClientTreeId.ToString();
					int[] clientHierarchyArr = Array.ConvertAll(clientHierarchy.Split('.'), int.Parse);

					for (int j = (clientHierarchyArr.Length - 1); j > 0 && !resultForProduct; j--)
					{
						List<NoneNego> noNegoForClientList = noNegoList.Where(x => x.ClientTree.ObjectId == clientHierarchyArr[j]).ToList();
						foreach (NoneNego noNego in noNegoForClientList)
						{
							if (noNego.ProductTree.ObjectId == productHierarchyArr[i])
							{
								if (noNego.Mechanic == model.MarsMechanic)
								{
									if (noNego.Discount >= model.MarsMechanicDiscount)
									{
										resultForProduct = true;
										break;
									}
								}
							}
						}
					}
				}

				// если хоть один subrange не прошел проверку, то отклоняем
				if (!resultForProduct)
					return false;
			}

			return true;
		}

		private bool EntityExists(System.Guid key) {
            return Context.Set<PromoStatusChange>().Count(e => e.Id == key) > 0;
        }     
    }
}
