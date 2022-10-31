using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.TPM;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoProductCorrectionPriceIncreasesController : EFContextController
    {
        private readonly UserInfo user;
        private readonly Role role;
        private readonly Guid? roleId;

        public PromoProductCorrectionPriceIncreasesController(IAuthorizationManager authorizationManager)
        {
            user = authorizationManager.GetCurrentUser();
            var roleInfo = authorizationManager.GetCurrentRole();
            role = new Role { Id = roleInfo.Id.Value, SystemName = roleInfo.SystemName };
            roleId = role.Id;
        }
        [ClaimsAuthorize]
        public IHttpActionResult Put(PromoProductCorrectionPriceIncrease model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Created(model);
            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            //var item = Context.Set<PromoProductCorrectionPriceIncrease>()
            //    .FirstOrDefault(x => x.Id == model.Id && !x.Disabled);

            //if (item != null)
            //{
            //    if (item.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
            //    item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    item.UserId = user.Id;
            //    item.UserName = user.Login;

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), model);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(model);
            //}
            //else
            //{
            //    var proxy = Context.Set<PromoProductsCorrection>().Create<PromoProductsCorrection>();
            //    var configuration = new MapperConfiguration(cfg =>
            //        cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>().ReverseMap());
            //    var mapper = configuration.CreateMapper();
            //    var result = mapper.Map(model, proxy);
            //    var promoProduct = Context.Set<PromoProduct>()
            //        .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled);

            //    if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.UserId = user.Id;
            //    result.UserName = user.Login;

            //    Context.Set<PromoProductsCorrection>().Add(result);

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), result);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(result);
            //}
        }
        [ClaimsAuthorize]
        public IHttpActionResult Post(PromoProductCorrectionPriceIncrease model)
        {//TODO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Created(model);
            // если существует коррекция на данный PromoProduct, то не создаем новый объект
            //var item = Context.Set<PromoProductCorrectionPriceIncrease>()
            //    .FirstOrDefault(x => x.Id == model.Id && !x.Disabled);

            //if (item != null)
            //{
            //    if (item.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(item.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
            //    item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    item.UserId = user.Id;
            //    item.UserName = user.Login;

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), model);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(model);
            //}
            //else
            //{
            //    var proxy = Context.Set<PromoProductsCorrection>().Create<PromoProductsCorrection>();
            //    var configuration = new MapperConfiguration(cfg =>
            //        cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>().ReverseMap());
            //    var mapper = configuration.CreateMapper();
            //    var result = mapper.Map(model, proxy);
            //    var promoProduct = Context.Set<PromoProduct>()
            //        .FirstOrDefault(x => x.Id == result.PromoProductId && !x.Disabled);

            //    if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(result.TempId))
            //    {
            //        return InternalServerError(new Exception("Promo Locked Update"));
            //    }
            //    result.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
            //    result.UserId = user.Id;
            //    result.UserName = user.Login;

            //    Context.Set<PromoProductsCorrection>().Add(result);

            //    try
            //    {
            //        var saveChangesResult = Context.SaveChanges();
            //        if (saveChangesResult > 0)
            //        {
            //            CreateChangesIncident(Context.Set<ChangesIncident>(), result);
            //            Context.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return GetErorrRequest(e);
            //    }

            //    return Created(result);
            //}
        }
        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PromoProductCorrectionPriceIncrease> patch)
        {
            try
            {
                var model = Context.Set<PromoProductCorrectionPriceIncrease>()
                    .FirstOrDefault(x => x.Id == key);

                if (model == null)
                {
                    return NotFound();
                }
                //var promoStatus = model.PromoProduct.Promo.PromoStatus.SystemName;

                //patch.TryGetPropertyValue("TPMmode", out object mode);

                //if ((int)model.TPMmode != (int)mode)
                //{
                //    List<PromoProductsCorrection> promoProductsCorrections = Context.Set<PromoProductsCorrection>()
                //        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                //        .Include(g => g.PromoProduct.Promo.BTLPromoes)
                //        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                //        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                //        .Where(x => x.PromoProduct.PromoId == model.PromoProduct.PromoId && !x.Disabled)
                //        .ToList();
                //    promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(Context, promoProductsCorrections);
                //    model = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.Promo.Number == model.PromoProduct.Promo.Number);
                //}

                //patch.Patch(model);
                //model.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                //model.UserId = user.Id;
                //model.UserName = user.Login;

                //if (model.TempId == "")
                //{
                //    model.TempId = null;
                //}


                //ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                //string promoStatuses = settingsManager.GetSetting<string>("PROMO_PRODUCT_CORRECTION_PROMO_STATUS_LIST", "Draft,Deleted,Cancelled,Started,Finished,Closed");
                //string[] status = promoStatuses.Split(',');
                //if (status.Any(x => x == promoStatus) && !role.Equals("SupportAdministrator"))
                //    return InternalServerError(new Exception("Cannot be update correction where status promo = " + promoStatus));
                //if (model.PromoProduct.Promo.NeedRecountUplift == false)
                //{
                //    return InternalServerError(new Exception("Promo Locked Update"));
                //}

                //var saveChangesResult = Context.SaveChanges();
                //if (saveChangesResult > 0)
                //{
                //    CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                //    Context.SaveChanges();
                //}

                return Updated(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }
        }
        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoProductsCorrection>().Count(e => e.Id == key) > 0;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoProductCorrection has already existed"));
            }
            else
            {
                return InternalServerError(e);
            }
        }
    }
}
