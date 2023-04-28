using AutoMapper;
using Core.Dependency;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Persist;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using Newtonsoft.Json;
using Utility.FileWorker;

namespace Module.Frontend.TPM.Controllers
{
    public class PlanPostPromoEffectsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        
        public PlanPostPromoEffectsController(IAuthorizationManager authorizationManager) {
            this.authorizationManager = authorizationManager;
        }
        
        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<PlanPostPromoEffect> GetPlanPostPromoEffects() {
            return GetConstraintedQuery();
        }        

        protected IQueryable<PlanPostPromoEffect> GetConstraintedQuery() {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PlanPostPromoEffect> query = Context.Set<PlanPostPromoEffect>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<PlanPostPromoEffect> GetFilteredData(ODataQueryOptions<PlanPostPromoEffect> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<PlanPostPromoEffect>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<PlanPostPromoEffect>;
        }
        
        [ClaimsAuthorize]
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<PlanPostPromoEffect> patch) {
            var model = Context.Set<PlanPostPromoEffect>().Find(key);
            if (model == null) {
                return NotFound();
            }
            patch.Put(model);
            try {
                Context.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Updated(model);
        }
        
        [ClaimsAuthorize]
        public IHttpActionResult Post(PlanPostPromoEffect model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var proxy = Context.Set<PlanPostPromoEffect>().Create<PlanPostPromoEffect>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<PlanPostPromoEffect, PlanPostPromoEffect>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);

            Context.Set<PlanPostPromoEffect>().Add(result);
            Context.Set<ChangesIncident>().Add(new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = nameof(PlanPostPromoEffect),
                ItemId = result.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                Disabled = false
            });

            try {
                Context.SaveChanges();
            } catch (Exception e) {
                return GetErrorRequest(e);
            }

            return Created(model);
        }
        
        [ClaimsAuthorize]
        public IHttpActionResult Delete([FromODataUri] System.Guid key) {
            try {
                var model = Context.Set<PlanPostPromoEffect>().Find(key);
                if (model == null) {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;
                
                Context.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e) {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<PlanPostPromoEffect> patch) {
            try
            {
                var model = Context.Set<PlanPostPromoEffect>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);

                Context.Set<ChangesIncident>().Add(new ChangesIncident
                {
                    Id = Guid.NewGuid(),
                    DirectoryName = nameof(PlanPostPromoEffect),
                    ItemId = model.Id.ToString(),
                    CreateDate = DateTimeOffset.Now,
                    Disabled = false
                });

                Context.SaveChanges();

                return Updated(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception e)
            {
                return GetErrorRequest(e);
            }
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult GetBrandTechSizes(string brandTechCode)
        {
            try
            {
                IQueryable<string> brandTechSizes = Context.Set<Product>()
                    .Where(x => x.BrandsegTech_code == brandTechCode)
                    .Select(x => x.Size).Distinct();

                return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, data = brandTechSizes }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            }
            catch (Exception e)
            {
                return GetErrorRequest(e);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult ExportXLSX(ODataQueryOptions<PlanPostPromoEffect> options) 
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                string handlerName = "ExportHandler";

                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TModel", typeof(PlanPostPromoEffect), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(PlanPostPromoEffectsController), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(PlanPostPromoEffectsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = $"Export {nameof(PlanPostPromoEffect)} dictionary",
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
        
        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX() {
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                NameValueCollection form = System.Web.HttpContext.Current.Request.Form;
                CreateImportTask(fileName, "FullXLSXPPEUpdateImportHandler", form);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            } catch (Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private void CreateImportTask(string fileName, string importHandler, NameValueCollection paramForm)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            using (DatabaseContext context = new DatabaseContext())
            {
                ImportResultFilesModel resiltfile = new ImportResultFilesModel();
                ImportResultModel resultmodel = new ImportResultModel();

                HandlerData data = new HandlerData();
                FileModel file = new FileModel()
                {
                    LogicType = "Import",
                    Name = Path.GetFileName(fileName),
                    DisplayName = Path.GetFileName(fileName)
                };

                // параметры импорта
                HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportPPE), data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportPPE).Name, data, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportPPE), data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Загрузка импорта из файла " + typeof(ImportCOGSTn).Name,
                    Name = "Module.Host.TPM.Handlers." + importHandler,
                    ExecutionPeriod = null,
                    RunGroup = typeof(ImportCOGSTn).Name,
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
        }

        private bool EntityExists(System.Guid key) {
            return Context.Set<PlanPostPromoEffect>().Count(e => e.Id == key) > 0;
        }

        public static IEnumerable<Column> GetExportSettings() {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "ClientTree.ObjectId", Header = "Client Hierarchy Code", Quoting = false },
                new Column() { Order = 1, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 2, Field = "BrandTech.BrandsegTechsub", Header = "Brand Tech", Quoting = false },
                new Column() { Order = 3, Field = "Size", Header = "Size", Quoting = false },
                new Column() { Order = 4, Field = "DiscountRange.Name", Header = "Discount", Quoting = false },
                new Column() { Order = 5, Field = "DurationRange.Name", Header = "Promo Duration", Quoting = false },
                new Column() { Order = 6, Field = "PlanPostPromoEffectW1", Header = "Plan Post Promo Effect W1", Quoting = false },
                new Column() { Order = 7, Field = "PlanPostPromoEffectW2", Header = "Plan PostPromo Effect W2", Quoting = false }
            };
            return columns;
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                string templateDir = AppSettingsManager.GetSetting("TEMPLATE_DIRECTORY", "Templates");
                string templateFilePath = Path.Combine(templateDir, "Plan PPE template.xlsx");
                using (FileStream templateStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook twb = new XSSFWorkbook(templateStream);

                    string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                    string filename = string.Format("{0}Template.xlsx", "PlanPostPromoEffect");
                    if (!Directory.Exists(exportDir))
                    {
                        Directory.CreateDirectory(exportDir);
                    }
                    string filePath = Path.Combine(exportDir, filename);
                    string file = Path.GetFileName(filePath);

                    DateTime dt = DateTime.Now;
                    List<ClientTree> clientsList = Context.Set<ClientTree>().Where(x => x.IsBaseClient
                    && (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0))).ToList();

                    List<BrandTech> brandtechs = Context.Set<BrandTech>().Where(x => !x.Disabled).ToList();
                    List<Product> products = Context.Set<Product>().Where(x => !x.Disabled).ToList();

                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        ISheet clientsSheet = twb.GetSheet("Clients");
                        ICreationHelper cH = twb.GetCreationHelper();

                        int i = 0;
                        foreach (var ct in clientsList)
                        {
                            IRow clientRow = clientsSheet.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(ct.FullPathName);

                            ICell idCell = clientRow.CreateCell(1);
                            idCell.SetCellValue(ct.ObjectId);
                            i++;
                        }

                        ISheet brandtechSheet = twb.GetSheet("Brandtech");
                        i = 1;
                        foreach (var bt in brandtechs)
                        {
                            IRow clientRow = brandtechSheet.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(bt.BrandsegTechsub);
                            i++;
                        }

                        //ISheet productsSheet = twb.GetSheet("Products");
                        //i = 1;
                        //foreach (var pr in products)
                        //{
                        //    IRow clientRow = productsSheet.CreateRow(i);
                        //    ICell hcell = clientRow.CreateCell(0);
                        //    hcell.SetCellValue(pr.ZREP);
                        //    hcell.SetCellValue(pr.EAN_Case);
                        //    hcell.SetCellValue(pr.EAN_PC);
                        //    hcell.SetCellValue(pr.ProductEN);
                        //    hcell.SetCellValue(pr.Brand);
                        //    hcell.SetCellValue(pr.Brand_code);
                        //    hcell.SetCellValue(pr.Technology);
                        //    hcell.SetCellValue(pr.Tech_code);
                        //    hcell.SetCellValue(pr.BrandTech);
                        //    hcell.SetCellValue(pr.Segmen_code);
                        //    hcell.SetCellValue(pr.BrandsegTech_code);
                        //    hcell.SetCellValue(pr.Brandsegtech);
                        //    hcell.SetCellValue(pr.BrandsegTechsub_code);
                        //    hcell.SetCellValue(pr.BrandsegTechsub);
                        //    hcell.SetCellValue(pr.SubBrand_code);
                        //    hcell.SetCellValue(pr.SubBrand);
                        //    hcell.SetCellValue(pr.BrandFlagAbbr);
                        //    hcell.SetCellValue(pr.BrandFlag);
                        //    hcell.SetCellValue(pr.SubmarkFlag);
                        //    hcell.SetCellValue(pr.IngredientVariety);
                        //    hcell.SetCellValue(pr.ProductCategory);
                        //    hcell.SetCellValue(pr.ProductType);
                        //    hcell.SetCellValue(pr.MarketSegment);
                        //    hcell.SetCellValue(pr.SupplySegment);
                        //    hcell.SetCellValue(pr.FunctionalVariety);
                        //    hcell.SetCellValue(pr.Size);
                        //    hcell.SetCellValue(pr.BrandEssence);
                        //    hcell.SetCellValue(pr.PackType);
                        //    hcell.SetCellValue(pr.GroupSize);
                        //    hcell.SetCellValue(pr.TradedUnitFormat);
                        //    hcell.SetCellValue(pr.ConsumerPackFormat);
                        //    hcell.SetCellValue((double)pr.UOM_PC2Case.GetValueOrDefault());
                        //    hcell.SetCellValue((double)pr.Division.GetValueOrDefault());
                        //    hcell.SetCellValue(pr.UOM);
                        //    hcell.SetCellValue((double)pr.NetWeight.GetValueOrDefault());
                        //    hcell.SetCellValue((double)pr.CaseVolume.GetValueOrDefault());
                        //    hcell.SetCellValue((double)pr.PCVolume.GetValueOrDefault());

                        //    i++;
                        //}

                        clientsSheet.AutoSizeColumn(0);
                        clientsSheet.AutoSizeColumn(1);
                        brandtechSheet.AutoSizeColumn(0);

                        twb.Write(stream);
                        stream.Close();
                    }
                    FileDispatcher fileDispatcher = new FileDispatcher();
                    fileDispatcher.UploadToBlob(Path.GetFileName(filePath), Path.GetFullPath(filePath), exportDir.Split('\\').Last());
                    return Content(HttpStatusCode.OK, file);
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        private ExceptionResult GetErrorRequest(Exception e) {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601)) {
                return InternalServerError(new Exception("This PlanPostPromoEffect has already existed"));
            } else {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }
}