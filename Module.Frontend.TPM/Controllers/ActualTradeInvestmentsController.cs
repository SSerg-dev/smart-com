﻿using AutoMapper;
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
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
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
using Utility.FileWorker;

namespace Module.Frontend.TPM.Controllers
{

    public class ActualTradeInvestmentsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public ActualTradeInvestmentsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<ActualTradeInvestment> GetConstraintedQuery()
        {

            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<ActualTradeInvestment> query = Context.Set<ActualTradeInvestment>().Where(e => !e.Disabled);
            IQueryable<ClientTreeHierarchyView> hierarchy = Context.Set<ClientTreeHierarchyView>().AsNoTracking();

            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, filters);

            return query;
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<ActualTradeInvestment> GetActualTradeInvestment([FromODataUri] System.Guid key)
        {
            return SingleResult.Create(GetConstraintedQuery());
        }


        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<ActualTradeInvestment> GetActualTradeInvestments()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<ActualTradeInvestment> GetFilteredData(ODataQueryOptions<ActualTradeInvestment> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<ActualTradeInvestment>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<ActualTradeInvestment>;
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<ActualTradeInvestment> patch)
        {
            var model = Context.Set<ActualTradeInvestment>().Find(key);
            if (model == null)
            {
                return NotFound();
            }
            patch.Put(model);
            try
            {
                await Context.SaveChangesAsync();
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
            return Updated(model);
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Post(ActualTradeInvestment model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // делаем UTC +3
            model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
            model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
            model.Year = model.StartDate.Value.Year;

            var proxy = Context.Set<ActualTradeInvestment>().Create<ActualTradeInvestment>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ActualTradeInvestment, ActualTradeInvestment>().ReverseMap());
            var mapper = configuration.CreateMapper();
            var result = mapper.Map(model, proxy);

            //Проверка пересечения по времени на клиенте
            if (!DateCheck(result))
            {
                string msg = "There can not be two ActualTradeInvestment of such client, brandTech, Type and SubType in some Time";
                return InternalServerError(new Exception(msg)); //Json(new { success = false, message = msg });
            }
            if (result.StartDate.Value.Year != result.EndDate.Value.Year)
            {
                string msg = "Start and End date must be in same year";
                return InternalServerError(new Exception(msg));
            }
            Context.Set<ActualTradeInvestment>().Add(result);
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return GetErorrRequest(e);
            }

            return Created(model);
        }

        [ClaimsAuthorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<ActualTradeInvestment> patch)
        {
            try
            {
                var model = Context.Set<ActualTradeInvestment>().FirstOrDefault(x => x.Id == key);

                var oldModel = new ActualTradeInvestment
                {
                    Disabled = model.Disabled,
                    DeletedDate = model.DeletedDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ClientTreeId = model.ClientTree.Id,
                    BrandTech = model.BrandTech,
                    BrandTechId = model.BrandTechId,
                    TIType = model.TIType,
                    TISubType = model.TISubType,
                    SizePercent = model.SizePercent,
                    MarcCalcROI = model.MarcCalcROI,
                    MarcCalcBudgets = model.MarcCalcBudgets,
                    Year = model.Year,
                    Id = model.Id
                };

                if (model == null)
                {
                    return NotFound();
                }

                patch.Patch(model);
                // делаем UTC +3
                model.StartDate = ChangeTimeZoneUtil.ResetTimeZone(model.StartDate);
                model.EndDate = ChangeTimeZoneUtil.ResetTimeZone(model.EndDate);
                model.Year = model.StartDate.Value.Year;

                BrandTech newBrandtech = model.BrandTechId != null ? Context.Set<BrandTech>().Find(model.BrandTechId) : null;
                var newModel = new ActualTradeInvestment
                {
                    Disabled = model.Disabled,
                    DeletedDate = model.DeletedDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ClientTreeId = model.ClientTree.Id,
                    BrandTech = newBrandtech != null ? newBrandtech : null,
                    BrandTechId = model.BrandTechId,
                    TIType = model.TIType,
                    TISubType = model.TISubType,
                    SizePercent = model.SizePercent,
                    MarcCalcROI = model.MarcCalcROI,
                    MarcCalcBudgets = model.MarcCalcBudgets,
                    Year = model.Year,
                    Id = model.Id
                };

                string promoPatchDateCheckMsg = PromoDateCheck(oldModel, newModel);
                if (promoPatchDateCheckMsg != null)
                {
                    return InternalServerError(new Exception(promoPatchDateCheckMsg));
                }

                //Проверка пересечения по времени на клиенте
                if (!DateCheck(model))
                {
                    string msg = "There can not be two ActualTradeInvestment of such client, brandTech, Type and SubType in some Time";
                    return InternalServerError(new Exception(msg)); //Json(new { success = false, message = msg });
                }
                if (model.StartDate.Value.Year != model.EndDate.Value.Year)
                {
                    string msg = "Start and End date must be in same year";
                    return InternalServerError(new Exception(msg));
                }

                Context.Set<ChangesIncident>().Add(new ChangesIncident
                {
                    Id = Guid.NewGuid(),
                    DirectoryName = nameof(ActualTradeInvestment),
                    ItemId = model.Id.ToString(),
                    CreateDate = DateTimeOffset.Now,
                    Disabled = false
                });

                await Context.SaveChangesAsync();

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

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Delete([FromODataUri] System.Guid key)
        {
            try
            {
                var model = Context.Set<ActualTradeInvestment>().Find(key);
                if (model == null)
                {
                    return NotFound();
                }

                model.DeletedDate = System.DateTime.Now;
                model.Disabled = true;

                string promoDeleteDateCheckMsg = PromoDateCheck(model, null);
                if (promoDeleteDateCheckMsg != null)
                {
                    return InternalServerError(new Exception(promoDeleteDateCheckMsg));
                }

                await Context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<ActualTradeInvestment>().Count(e => e.Id == key) > 0;
        }

        [ClaimsAuthorize]
        public IHttpActionResult IsTIRecalculatePreviousYearButtonAvailable()
        {
            var previousYear = DateTimeOffset.Now.AddYears(-1).Year;
            var previousYearActualTIs = Context.Set<ActualTradeInvestment>()
                .Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value.Year == previousYear &&
                x.EndDate.HasValue && x.EndDate.Value.Year == previousYear);

            var isAvailable = (previousYearActualTIs.Count() != 0) && (!previousYearActualTIs.Any(x => x.IsTIIncidentCreated));

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new
            {
                success = true,
                isRecalculatePreviousYearButtonAvailable = isAvailable
            }));
        }

        [ClaimsAuthorize]
        public IHttpActionResult PreviousYearPromoList()
        {
            var previousYear = DateTimeOffset.Now.AddYears(-1).Year;
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST", "Finished,Closed");
            var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = Context.Set<Promo>()
                .Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value.Year == previousYear)
                .Select(x => new
                {
                    Number = x.Number,
                    Name = x.Name,
                    BrandTechName = x.BrandTech.BrandsegTechsub,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DispatchesStart = x.DispatchesStart,
                    DispatchesEnd = x.DispatchesEnd,
                    PromoStatusName = x.PromoStatus.Name,
                })
                .ToList()
                .Where(x => checkPromoStatuses.Contains(x.PromoStatusName))
                .OrderBy(x => x.Number);

            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new
            {
                success = true,
                promoes
            }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        public async Task<IHttpActionResult> CreateActualTIChangeIncidents()
        {
            var previousYear = DateTimeOffset.Now.AddYears(-1).Year;
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST", "Finished,Closed");
            var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var promoes = Context.Set<Promo>()
                .Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value.Year == previousYear)
                .Where(x => checkPromoStatuses.Contains(x.PromoStatus.Name))
                .Select(x => x.Id).ToList();
            var actualTIForPrevYear = Context.Set<ActualTradeInvestment>().Where(x => x.Year == previousYear && !x.Disabled).ToList();

            foreach (var item in promoes)
            {
                var changesIncident = new ChangesIncident
                {
                    Id = Guid.NewGuid(),
                    DirectoryName = nameof(Promo) + nameof(ActualTradeInvestment),
                    ItemId = item.ToString(),
                    CreateDate = DateTimeOffset.Now,
                    Disabled = false
                };

                Context.Set<ChangesIncident>().Add(changesIncident);
            }

            foreach (var ti in actualTIForPrevYear)
            {
                ti.IsTIIncidentCreated = true;
            }

            await Context.SaveChangesAsync();
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true }));
        }

        public static IEnumerable<Column> GetExportSettings()
        {
            IEnumerable<Column> columns = new List<Column>() {
                new Column() { Order = 0, Field = "StartDate", Header = "StartDate", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 1, Field = "EndDate", Header = "EndDate", Quoting = false, Format = "dd.MM.yyyy"  },
                new Column() { Order = 2, Field = "ClientTree.FullPathName", Header = "Client", Quoting = false },
                new Column() { Order = 3, Field = "ClientTree.ObjectId", Header = "ClientId", Quoting = false },
                new Column() { Order = 4, Field = "BrandTech.BrandsegTechsub", Header = "BrandTech", Quoting = false },
                new Column() { Order = 5, Field = "TIType", Header = "TI Type", Quoting = false },
                new Column() { Order = 6, Field = "TISubType", Header = "TI SubType", Quoting = false },
                new Column() { Order = 7, Field = "SizePercent", Header = "Size Percent", Quoting = false },
                new Column() { Order = 8, Field = "MarcCalcROI", Header = "Marc Calc ROI", Quoting = false },
                new Column() { Order = 9, Field = "MarcCalcBudgets", Header = "Marc Calc Budgets", Quoting = false }
            };
            return columns;
        }
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> ExportXLSX(ODataQueryOptions<ActualTradeInvestment> options)
        {
            IQueryable results = options.ApplyTo(GetConstraintedQuery().Where(x => !x.Disabled));
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            string handlerName = "ExportHandler";

            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TModel", typeof(ActualTradeInvestment), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("TKey", typeof(Guid), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnInstance", typeof(ActualTradeInvestmentsController), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("GetColumnMethod", nameof(ActualTradeInvestmentsController.GetExportSettings), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("SqlString", results.ToTraceQuery(), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = $"Export {nameof(ActualTradeInvestment)} dictionary",
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
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();

            return Content(HttpStatusCode.OK, "success");
        }

        [ClaimsAuthorize]
        public async Task<HttpResponseMessage> FullImportXLSX()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string importDir = Core.Settings.AppSettingsManager.GetSetting("IMPORT_DIRECTORY", "ImportFiles");
                string fileName = await FileUtility.UploadFile(Request, importDir);

                NameValueCollection form = System.Web.HttpContext.Current.Request.Form;
                await CreateImportTask(fileName, "FullXLSXTradeInvestmentUpdateImportHandler", form);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent("success = true");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return result;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private async Task CreateImportTask(string fileName, string importHandler, NameValueCollection paramForm)
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            ImportResultFilesModel resiltfile = new ImportResultFilesModel();
            ImportResultModel resultmodel = new ImportResultModel();

            HandlerData data = new HandlerData();
            FileModel file = new FileModel()
            {
                LogicType = "Import",
                Name = System.IO.Path.GetFileName(fileName),
                DisplayName = System.IO.Path.GetFileName(fileName)
            };

            // параметры импорта
            HandlerDataHelper.SaveIncomingArgument("CrossParam.Year", paramForm.GetStringValue("year"), data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportDestination", "ActualTI", data, throwIfNotExists: false);

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportTradeInvestment), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportTradeInvestment).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportTradeInvestment), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла ImportActualTradeInvestment",
                Name = "Module.Host.TPM.Handlers." + importHandler,
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                RunGroup = typeof(ImportTradeInvestment).Name,
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };
            handler.SetParameterData(data);
            Context.LoopHandlers.Add(handler);
            await Context.SaveChangesAsync();

        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                string templateDir = AppSettingsManager.GetSetting("TEMPLATE_DIRECTORY", "Templates");
                string templateFilePath = Path.Combine(templateDir, "ActualTIPreTemplate.xlsx");
                using (FileStream templateStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook twb = new XSSFWorkbook(templateStream);

                    string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                    string filename = string.Format("{0}Template.xlsx", "ActualTradeInvestment");
                    if (!Directory.Exists(exportDir))
                    {
                        Directory.CreateDirectory(exportDir);
                    }
                    string filePath = Path.Combine(exportDir, filename);
                    string file = Path.GetFileName(filePath);

                    DateTime dt = DateTime.Now;
                    List<ClientTree> clientsList = Context.Set<ClientTree>().Where(x => x.Type == "root"
                    || (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0))).ToList();

                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        ISheet sheet2 = twb.GetSheet("Лист2");
                        ICreationHelper cH = twb.GetCreationHelper();

                        int i = 0;
                        foreach (ClientTree ct in clientsList)
                        {
                            IRow clientRow = sheet2.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(ct.FullPathName);

                            ICell idCell = clientRow.CreateCell(1);
                            idCell.SetCellValue(ct.ObjectId);
                            i++;
                        }
                        sheet2.AutoSizeColumn(0);
                        sheet2.AutoSizeColumn(1);

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

        // Логика проверки пересечения времени
        public bool DateCheck(ActualTradeInvestment toCheck)
        {
            List<ActualTradeInvestment> clientTIs = GetConstraintedQuery()
                .Where(y => y.ClientTreeId == toCheck.ClientTreeId && y.BrandTechId == toCheck.BrandTechId
                && y.TIType == toCheck.TIType && y.TISubType == toCheck.TISubType
                && y.Id != toCheck.Id && !y.Disabled).ToList();
            foreach (ActualTradeInvestment item in clientTIs)
            {
                if ((item.StartDate <= toCheck.StartDate && item.EndDate >= toCheck.StartDate) ||
                    (item.StartDate <= toCheck.EndDate && item.EndDate >= toCheck.EndDate) ||
                    (item.StartDate >= toCheck.StartDate && item.EndDate <= toCheck.EndDate))
                {
                    return false;
                }
            }
            return true;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This Trade Investment has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }

        public string PromoDateCheck(ActualTradeInvestment model, ActualTradeInvestment newModel)
        {
            //Получаем все дочерние ClientTreeId 
            var currClientTree = Context.Set<ClientTree>().Where(x => !x.EndDate.HasValue && x.Id == model.ClientTreeId).FirstOrDefault();
            var clientTreesIds = Helper.getClientTreeAllChildren(currClientTree.ObjectId, Context).Select(x => x.Id).ToList();
            clientTreesIds.Add(currClientTree.Id);

            List<int> parentIds = new List<int>();
            while (currClientTree.Type != "root" && currClientTree.parentId != 5000000)
            {
                currClientTree = Context.Set<ClientTree>().Where(x => !x.EndDate.HasValue && x.ObjectId == currClientTree.parentId).FirstOrDefault();
                parentIds.Add(currClientTree.Id);
            };

            //Получаем только те промо, которые могли быть привязаны к изменяемому TI
            var promoesToCheck = this.GetPromoesForCheck(Context, model, clientTreesIds);

            //Забираем TI только для соответствующего дерева клиентов
            var models = Context.Set<ActualTradeInvestment>().Where(x => x.Id != model.Id && !x.Disabled && (parentIds.Contains(x.ClientTreeId) || clientTreesIds.Contains(x.ClientTreeId))).ToList();

            if (newModel != null)
            {
                models.Add(newModel);
            }

            //Соединяем, для получения полного дерева
            clientTreesIds.AddRange(parentIds);

            var invalidPromoesByTI = this.GetInvalidPromoesByActualTradeInvestment(models, promoesToCheck, clientTreesIds);
            if (invalidPromoesByTI.Any())
            {
                return $"Promo with numbers {string.Join(", ", invalidPromoesByTI)} will not have Trade Investment after that.";
            }

            return null;
        }

        private IEnumerable<SimplePromoTradeInvestment> GetPromoesForCheck(DatabaseContext databaseContext, ActualTradeInvestment model, List<int> ClientTreeIds)
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var statusesSetting = settingsManager.GetSetting<string>("ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST", "Finished, Closed");
            var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var query = databaseContext.Set<Promo>()
                .Where(x => !x.Disabled && x.ClientTreeId.HasValue && ClientTreeIds.Contains((int)x.ClientTreeKeyId)
                && model.StartDate.HasValue && model.EndDate.HasValue && x.StartDate.HasValue
                && DateTimeOffset.Compare(model.StartDate.Value, x.StartDate.Value) <= 0
                && DateTimeOffset.Compare(model.EndDate.Value, x.StartDate.Value) >= 0
                && checkPromoStatuses.Contains(x.PromoStatus.Name));

            if (model.BrandTech != null)
            {
                query = query.Where(x => x.BrandTech.BrandsegTechsub == model.BrandTech.BrandsegTechsub);
            }

            List<SimplePromoTradeInvestment> promos = new List<SimplePromoTradeInvestment>();
            foreach (var item in query)
            {
                promos.Add(new SimplePromoTradeInvestment(item));
            }

            return promos;
        }

        private IEnumerable<int?> GetInvalidPromoesByActualTradeInvestment(
            IEnumerable<ActualTradeInvestment> models, IEnumerable<SimplePromoTradeInvestment> promoes, List<int> clientTreeIds)
        {
            IEnumerable<SimplePromoTradeInvestment> invalidPromoes = new List<SimplePromoTradeInvestment>();

            //Группируем промо, что бы проверять только родительские ClientTree
            var promoGroups = promoes.GroupBy(x => x.ClientTreeId);
            //Получаем всё дерево клиентов
            List<ClientTree> clientTrees = Context.Set<ClientTree>().Where(x => clientTreeIds.Contains(x.Id)).ToList();
            ClientTree clientTree;
            List<SimplePromoTradeInvestment> promoesToRemove;
            List<SimplePromoTradeInvestment> promoesGroupList;
            List<ActualTradeInvestment> ClientActualTIEmptyBrandtech;
            List<ActualTradeInvestment> ClientActualTINotEmptyBrandtech;
            IEnumerable<ActualTradeInvestment> FittedTI;
            List<int?> invalidPromoesNumbers = new List<int?>();

            foreach (var promoGroup in promoGroups)
            {
                promoesGroupList = promoGroup.ToList();
                promoesToRemove = new List<SimplePromoTradeInvestment>();
                clientTree = clientTrees.Where(x => x.Id == promoGroup.Key).FirstOrDefault();
                if (clientTree != null)
                {
                    while (clientTree != null && promoesGroupList.Count > 0)
                    {
                        ClientActualTIEmptyBrandtech = models.Where(x => x.ClientTreeId == clientTree.Id && x.BrandTech == null).ToList();
                        ClientActualTINotEmptyBrandtech = models.Where(x => x.ClientTreeId == clientTree.Id && x.BrandTech != null).ToList();
                        foreach (SimplePromoTradeInvestment promo in promoesGroupList)
                        {
                            if (ClientActualTIEmptyBrandtech != null)
                            {
                                FittedTI = ClientActualTIEmptyBrandtech.Where(x => DateTimeOffset.Compare(x.StartDate.Value, promo.StartDate.Value) <= 0
                                       && DateTimeOffset.Compare(x.EndDate.Value, promo.StartDate.Value) >= 0);
                                if (FittedTI.Any())
                                {
                                    promoesToRemove.Add(promo);
                                }
                            }

                            if (ClientActualTINotEmptyBrandtech != null)
                            {
                                FittedTI = ClientActualTINotEmptyBrandtech.Where(x => DateTimeOffset.Compare(x.StartDate.Value, promo.StartDate.Value) <= 0
                                       && DateTimeOffset.Compare(x.EndDate.Value, promo.StartDate.Value) >= 0
                                       && (promo.BrandTechName == x.BrandTech.BrandsegTechsub));
                                if (FittedTI.Any())
                                {
                                    promoesToRemove.Add(promo);
                                }
                            }
                        }
                        promoesGroupList = promoesGroupList.Except(promoesToRemove).ToList();
                        clientTree = clientTrees.Where(x => x.ObjectId == clientTree.parentId).FirstOrDefault();
                    }
                    if (promoesGroupList.Count > 0)
                    {
                        invalidPromoesNumbers.AddRange(promoesGroupList.Select(x => x.Number));
                    }
                }
            }
            return invalidPromoesNumbers;
        }
    }
}