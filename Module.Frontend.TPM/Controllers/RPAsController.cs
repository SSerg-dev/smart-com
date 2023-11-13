using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Core.Settings;
using Frontend.Core.Controllers.Base;
using Frontend.Core.Extensions;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Model;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Newtonsoft.Json;
using Persist.Model;
using System;
using System.Collections.Generic;
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
using LiteDB;
using Module.Persist.TPM.Model.DTO;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Utility;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Persist.ScriptGenerator.Filter;
using Utility.FileWorker;
using Column = Frontend.Core.Extensions.Export.Column;
using UserInfoCore = Core.Security.Models.UserInfo;

namespace Module.Frontend.TPM.Controllers
{
    public class RPAsController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;
        private Core.Security.Models.UserInfo user;
        private string role;
        private Guid roleId;
        private IList<Constraint> constraints;
        private static object locker = new object();
        public RPAsController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
            this.user = authorizationManager.GetCurrentUser();
            this.role = authorizationManager.GetCurrentRoleName();
            this.roleId = this.user.Roles.ToList().Find(role => role.SystemName == this.role).Id.Value;
        }

        protected IQueryable<RPA> GetConstraintedQuery()
        {

            this.constraints = this.user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<RPA> query = Context.Set<RPA>();

            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public SingleResult<RPA> GetRPA([FromODataUri] System.Guid key)
        {
            return SingleResult.Create<RPA>(GetConstraintedQuery());
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue)]
        public IQueryable<RPA> GetRPAs()
        {
            return GetConstraintedQuery();
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<RPA> GetFilteredData(ODataQueryOptions<RPA> options)
        {
            var query = GetConstraintedQuery();

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            };

            var optionsPost = new ODataQueryOptionsPost<RPA>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<RPA>;
        }

        /// <summary>
        /// Метод создания RPA (новый функционал)
        /// </summary>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> SaveRPA()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HttpRequest currentRequest = HttpContext.Current.Request;
            RPA rpaModel = JsonConvert.DeserializeObject<RPA>(currentRequest.Params.Get("Model"));
            string rpaType = currentRequest.Params.Get("RPAType");
            RPA proxy = Context.Set<RPA>().Create<RPA>();
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<RPA, RPA>().ReverseMap());
            IMapper mapper = configuration.CreateMapper();
            RPA result = mapper.Map(rpaModel, proxy);
            Context.Set<RPA>().Add(result);
            try
            {
                int maxFileByteLength = 25000000;

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (Request.Content.Headers.ContentLength > maxFileByteLength)
                {
                    throw new FileLoadException("The file size must be less than 25mb.");
                }

                //Save file
                string directory = Core.Settings.AppSettingsManager.GetSetting("RPA_DIRECTORY", "RPAFiles");
                string fileName = await FileUtility.UploadFile(Request, directory);
                IList<Constraint> constraints = Context.Constraints
                                                        .Where(x => x.UserRole.UserId == user.Id && x.UserRole.Role.Id == roleId)
                                                        .ToList();
                IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
                var clientTrees = Context.Set<ClientTree>().Where(x => x.EndDate == null);
                var clientIds = constraints.Where(c => c.Prefix == "CLIENT_ID").Select(x => Int32.Parse(x.Value));
                clientTrees = clientTrees.Where(x => clientIds.Any(y => x.ObjectId == y));
                result.Constraint = string.Join(",", clientTrees.Select(x => x.Name));
                result.CreateDate = DateTime.UtcNow;
                result.FileURL = Path.GetFileName(fileName);
                // Save RPA
                var resultSaveChanges = await Context.SaveChangesAsync();
                var rpaId = result.Id;
                switch (rpaType)
                {
                    case "Events":
                        result.HandlerId = await CreateRPAEventImportTask(fileName, rpaId);
                        break;
                    case "PromoSupport":
                        result.HandlerId = await CreateRPAPromoSupportTask(fileName, rpaId);
                        break;
                    case "NonPromoSupport":
                        result.HandlerId = await CreateRPANonPromoSupportTask(fileName, rpaId);
                        break;
                    case "Actuals_EAN_PC":
                        result.HandlerId = await CreateRPAActualEanPcTask(fileName, rpaId);
                        break;
                    case "Actuals_PLU":
                        result.HandlerId = await CreateRpaActualPluTask(fileName, rpaId);
                        break;
                    case "TLC_Draft":
                        result.HandlerId = await CreateRpaTLCdraftTask(fileName, rpaId);
                        break;
                    case "TLC_Closed":
                        result.HandlerId = await CreateRpaTLCclosedTask(fileName, rpaId);
                        break;
                    case "TLC_Actual_Shelf_Price_Discount":
                        result.HandlerId = await CreateActualShelfPriceTask(fileName, rpaId);
                        break;
                }
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return GetErorrRequest(ex);
            }
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, message = "RPA save and upload done." }));
        }

        private async Task<Guid> CreateRPAEventImportTask(string fileName, Guid rpaId)
        {
            string importHandler = "FullXLSXRPAEventImportHandler";

            Core.Security.Models.UserInfo user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            FileModel file = new FileModel()
            {
                LogicType = "Import",
                Name = Path.GetFileName(fileName),
                DisplayName = Path.GetFileName(fileName)
            };

            HandlerDataHelper.SaveIncomingArgument("File", file, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRPAEvent), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRPAEvent).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRPAEvent), data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка импорта из файла " + typeof(ImportRPAEvent).Name,
                Name = "Module.Host.TPM.Handlers." + importHandler,
                ExecutionPeriod = null,
                RunGroup = typeof(ImportRPAEvent).Name,
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
            return handler.Id;
        }

        private async Task<Guid> CreateRPAPromoSupportTask(string fileName, Guid rpaId)
        {
            var handlerName = "FullXLSXRPAPromoSupportImportHandler";
            UserInfoCore user = authorizationManager.GetCurrentUser();
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

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRPAPromoSupport).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<string>() { "Name" }, data);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoSupport).Name,
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
            return handler.Id;
        }

        private async Task<Guid> CreateRPANonPromoSupportTask(string fileName, Guid rpaId)
        {
            var handlerName = "FullXLSXRPANonPromoSupportImportHandler";
            UserInfoCore user = authorizationManager.GetCurrentUser();
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

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRPAPromoSupport).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRPAPromoSupport), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<string>() { "Name" }, data);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoSupport).Name,
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
            return handler.Id;
        }

        private async Task<Guid> CreateRPAActualEanPcTask(string fileName, Guid rpaId)
        {
            var handlerName = "FullXLSXRPAActualEANPCImportHandler";
            UserInfoCore user = authorizationManager.GetCurrentUser();
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

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRpaActualEanPc), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRpaActualEanPc).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRpaActualEanPc), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<string>() { "Name" }, data);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoSupport).Name,
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
            return handler.Id;
        }

        private async Task<Guid> CreateRpaActualPluTask(string fileName, Guid rpaId)
        {
            var handlerName = "FullXLSXRpaActualPluImportHandler";
            UserInfoCore user = authorizationManager.GetCurrentUser();
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

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRpaActualPlu), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRpaActualPlu).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRpaActualPlu), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<string>() { "Name" }, data);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoSupport).Name,
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
            return handler.Id;
        }

        private async Task<Guid> CreateRpaTLCclosedTask(string fileName, Guid rpaId)
        {
            var handlerName = "FullXLSXRpaTCLclosedImportHandler";
            UserInfoCore user = authorizationManager.GetCurrentUser();
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

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRpaTLCclosed), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRpaTLCclosed).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRpaTLCclosed), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<string>() { "Name" }, data);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoSupport).Name,
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
            return handler.Id;
        }

        private async Task<Guid> CreateActualShelfPriceTask(string fileName, Guid rpaId)
        {
            var handlerName = "FullXLSXRpaActualShelfPriceImportHandler";
            UserInfoCore user = authorizationManager.GetCurrentUser();
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

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportActualShelfPriceDiscount), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportActualShelfPriceDiscount).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportActualShelfPriceDiscount), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<string>() { "PromoId" }, data);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoSupport).Name,
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
            return handler.Id;
        }

        private async Task<Guid> CreateRpaTLCdraftTask(string fileName, Guid rpaId)
        {
            var handlerName = "FullXLSXRpaTCLdraftImportHandler";
            UserInfoCore user = authorizationManager.GetCurrentUser();
            Guid userId = user == null ? Guid.Empty : (user.Id.HasValue ? user.Id.Value : Guid.Empty);
            RoleInfo role = authorizationManager.GetCurrentRole();
            Guid roleId = role == null ? Guid.Empty : (role.Id.HasValue ? role.Id.Value : Guid.Empty);

            HandlerData data = new HandlerData();
            FileModel file = new FileModel()
            {
                LogicType = "Import",
                Name = System.IO.Path.GetFileName(fileName),
                DisplayName = System.IO.Path.GetFileName(fileName)
            };

            HandlerDataHelper.SaveIncomingArgument("File", file, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UserId", userId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("RoleId", roleId, data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportType", typeof(ImportRpaTLCdraft), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ImportTypeDisplay", typeof(ImportRpaTLCdraft).Name, data, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("ModelType", typeof(ImportRpaTLCdraft), data, visible: false, throwIfNotExists: false);
            HandlerDataHelper.SaveIncomingArgument("UniqueFields", new List<string>() { "Name" }, data);
            HandlerDataHelper.SaveIncomingArgument("RPAId", rpaId, data, visible: false, throwIfNotExists: false);

            LoopHandler handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Загрузка шаблона из файла " + typeof(RPA).Name,
                Name = "Module.Host.TPM.Handlers." + handlerName,
                ExecutionPeriod = null,
                RunGroup = typeof(PromoSupport).Name,
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
            return handler.Id;
        }

        private IHttpActionResult DownloadTLCClosedTemplateXLSX()
        {
            try
            {
                string templateDir = AppSettingsManager.GetSetting("TEMPLATE_DIRECTORY", "Templates");
                string templateFilePath = Path.Combine(templateDir, "TLCClosedTemplate.xlsx");
                using (FileStream templateStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook twb = new XSSFWorkbook(templateStream);

                    string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                    string filename = string.Format("{0}Template.xlsx", "TLCClosed");
                    if (!Directory.Exists(exportDir))
                    {
                        Directory.CreateDirectory(exportDir);
                    }
                    string filePath = Path.Combine(exportDir, filename);
                    string file = Path.GetFileName(filePath);

                    UserInfo user = authorizationManager.GetCurrentUser();
                    string role = authorizationManager.GetCurrentRoleName();
                    IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                        .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                        .ToList() : new List<Constraint>();
                    IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);

                    DateTime dt = DateTime.Now;
                    IQueryable<ClientTree> query = Context.Set<ClientTree>().Where(x => x.Type != "root" && x.parentId != 5000000
                        && (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0) && x.IsBaseClient == true));

                    List<ClientTree> clientsList = query.ToList();

                    List<BrandTech> brandtechs = Context.Set<BrandTech>().Where(x => !x.Disabled).ToList();
                    List<Mechanic> mecanics = Context.Set<Mechanic>().Where(x => !x.Disabled).ToList();
                    List<MechanicType> mecanicTypes = Context.Set<MechanicType>().Where(x => !x.Disabled).ToList();

                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        ISheet sheet2 = twb.GetSheet("Clients");
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

                        ISheet sheet3 = twb.GetSheet("BrandTech");
                        i = 0;
                        foreach (BrandTech bt in brandtechs)
                        {
                            IRow clientRow = sheet3.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(bt.BrandsegTechsub);
                            i++;
                        }
                        sheet2.AutoSizeColumn(0);
                        sheet2.AutoSizeColumn(1);
                        sheet3.AutoSizeColumn(0);

                        ISheet sheet6 = twb.GetSheet("Subrange");
                        var brandtechsQuery = Context.Set<BrandTech>().Where(x => !x.Disabled);
                        var exitingSubRange = Context.Set<ProductTree>().Where(x => x.Type == "Subrange" && !x.EndDate.HasValue);
                        var existingTechnology = Context.Set<ProductTree>().Where(x => x.Type == "Technology" && !x.EndDate.HasValue);
                        var existingNameTechnology = existingTechnology.Join(exitingSubRange,
                            p => p.ObjectId,
                            p1 => p1.parentId,
                            (p, p1) => new { Technology = p.Name, Subrange = p1.Name });

                        i = 0;
                        foreach (var item in existingNameTechnology)
                        {
                            IRow subrangeRow = sheet6.CreateRow(i);
                            ICell hcell = subrangeRow.CreateCell(0);
                            hcell.SetCellValue(item.Technology);
                            hcell = subrangeRow.CreateCell(1);
                            hcell.SetCellValue(item.Subrange);
                            i++;
                        }
                        sheet6.AutoSizeColumn(0);
                        sheet6.AutoSizeColumn(1);

                        ISheet sheet4 = twb.GetSheet("Mechanics");
                        i = 0;
                        foreach (Mechanic m in mecanics)
                        {
                            IRow clientRow = sheet4.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(m.Name);
                            i++;
                        }
                        sheet4.AutoSizeColumn(0);

                        ISheet sheet5 = twb.GetSheet("Mechanics Type");
                        i = 0;
                        foreach (MechanicType m in mecanicTypes)
                        {
                            IRow clientRow = sheet5.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue("VP");
                            hcell = clientRow.CreateCell(1);
                            hcell.SetCellValue(m.Name);
                            i++;
                        }
                        sheet5.AutoSizeColumn(0);
                        sheet5.AutoSizeColumn(1);

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

        private IHttpActionResult DownloadTLCDraftTemplateXLSX()
        {
            try
            {
                string templateDir = AppSettingsManager.GetSetting("TEMPLATE_DIRECTORY", "Templates");
                string templateFilePath = Path.Combine(templateDir, "TLCDraftTemplate.xlsx");
                using (FileStream templateStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook twb = new XSSFWorkbook(templateStream);

                    string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                    string filename = string.Format("{0}Template.xlsx", "TLCDraft");
                    if (!Directory.Exists(exportDir))
                    {
                        Directory.CreateDirectory(exportDir);
                    }
                    string filePath = Path.Combine(exportDir, filename);
                    string file = Path.GetFileName(filePath);

                    UserInfo user = authorizationManager.GetCurrentUser();
                    string role = authorizationManager.GetCurrentRoleName();
                    IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                        .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                        .ToList() : new List<Constraint>();
                    IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);

                    DateTime dt = DateTime.Now;
                    IQueryable<ClientTree> query = Context.Set<ClientTree>().Where(x => x.Type != "root" && x.parentId != 5000000
                        && (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0) && x.IsBaseClient == true));

                    List<ClientTree> clientsList = query.ToList();

                    List<BrandTech> brandtechs = Context.Set<BrandTech>().Where(x => !x.Disabled).ToList();
                    List<Mechanic> mecanics = Context.Set<Mechanic>().Where(x => !x.Disabled).ToList();
                    List<MechanicType> mecanicTypes = Context.Set<MechanicType>().Where(x => !x.Disabled).ToList();

                    using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        ISheet sheet2 = twb.GetSheet("Clients");
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

                        ISheet sheet3 = twb.GetSheet("BrandTech");
                        i = 0;
                        foreach (BrandTech bt in brandtechs)
                        {
                            IRow clientRow = sheet3.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(bt.BrandsegTechsub);
                            i++;
                        }
                        sheet2.AutoSizeColumn(0);
                        sheet2.AutoSizeColumn(1);
                        sheet3.AutoSizeColumn(0);

                        ISheet sheet6 = twb.GetSheet("Subrange");
                        var brandtechsQuery = Context.Set<BrandTech>().Where(x => !x.Disabled);
                        var exitingSubRange = Context.Set<ProductTree>().Where(x => x.Type == "Subrange" && !x.EndDate.HasValue);
                        var existingTechnology = Context.Set<ProductTree>().Where(x => x.Type == "Technology" && !x.EndDate.HasValue);
                        var existingNameTechnology = existingTechnology.Join(exitingSubRange,
                             p => p.ObjectId,
                             p1 => p1.parentId,
                             (p, p1) => new { Technology = p.Name, Subrange = p1.Name });

                        i = 0;
                        foreach (var item in existingNameTechnology)
                        {
                            IRow subrangeRow = sheet6.CreateRow(i);
                            ICell hcell = subrangeRow.CreateCell(0);
                            hcell.SetCellValue(item.Technology);
                            hcell = subrangeRow.CreateCell(1);
                            hcell.SetCellValue(item.Subrange);
                            i++;
                        }
                        sheet6.AutoSizeColumn(0);
                        sheet6.AutoSizeColumn(1);

                        ISheet sheet4 = twb.GetSheet("Mechanics");
                        i = 0;
                        foreach (Mechanic m in mecanics)
                        {
                            IRow clientRow = sheet4.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(m.Name);
                            i++;
                        }
                        sheet4.AutoSizeColumn(0);

                        ISheet sheet5 = twb.GetSheet("Mechanics Type");
                        i = 0;
                        foreach (MechanicType m in mecanicTypes)
                        {
                            IRow clientRow = sheet5.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue("VP");
                            hcell = clientRow.CreateCell(1);
                            hcell.SetCellValue(m.Name);
                            i++;
                        }
                        sheet5.AutoSizeColumn(0);
                        sheet5.AutoSizeColumn(1);

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

        private IHttpActionResult DownloadTLCActualShelfPriceTemplateXLSX()
        {
            try
            {
                string templateDir = AppSettingsManager.GetSetting("TEMPLATE_DIRECTORY", "Templates");
                string templateFilePath = Path.Combine(templateDir, "ActualShelfPriceTemplate.xlsx");
                using (FileStream templateStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook twb = new XSSFWorkbook(templateStream);

                    string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                    string filename = string.Format("{0}Template.xlsx", "ActualShelfPrice");
                    if (!Directory.Exists(exportDir))
                    {
                        Directory.CreateDirectory(exportDir);
                    }
                    string filePath = Path.Combine(exportDir, filename);
                    string file = Path.GetFileName(filePath);

                    var user = authorizationManager.GetCurrentUser();
                    var role = authorizationManager.GetCurrentRoleName();
                    IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                        .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                        .ToList() : new List<Constraint>();
                    IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);

                    DateTime dt = DateTime.Now;
                    IQueryable<ClientTree> query = Context.Set<ClientTree>().Where(x => x.Type != "root" && x.parentId != 5000000
                    && (DateTime.Compare(x.StartDate, dt) <= 0 && (!x.EndDate.HasValue || DateTime.Compare(x.EndDate.Value, dt) > 0) && x.IsBaseClient == true));
                    List<Mechanic> mechanics = Context.Set<Mechanic>().Where(x => !x.Disabled && x.PromoTypes.SystemName == "Regular").ToList();
                    List<MechanicType> mechanicTypes = Context.Set<MechanicType>().Where(x => !x.Disabled).ToList();

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        ICreationHelper cH = twb.GetCreationHelper();
                        int i = 0;
                        ISheet sheet4 = twb.GetSheet("Mechanics");
                        foreach (var m in mechanics)
                        {
                            IRow clientRow = sheet4.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(m.Name);
                            i++;
                        }
                        sheet4.AutoSizeColumn(0);

                        ISheet sheet5 = twb.GetSheet("Mechanics Type");
                        i = 0;
                        foreach (var m in mechanicTypes)
                        {
                            IRow clientRow = sheet5.CreateRow(i);
                            ICell hcell = clientRow.CreateCell(0);
                            hcell.SetCellValue(m.Name);
                            i++;
                        }
                        sheet5.AutoSizeColumn(0);
                        sheet5.AutoSizeColumn(1);

                        twb.Write(stream);
                        stream.Close();
                    }
                    var fileDispatcher = new FileDispatcher();
                    fileDispatcher.UploadToBlob(Path.GetFileName(filePath), Path.GetFullPath(filePath), exportDir.Split('\\').Last());
                    return Content(HttpStatusCode.OK, file);
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [ClaimsAuthorize]
        public IHttpActionResult DownloadTemplateXLSX()
        {
            try
            {
                var currentRequest = HttpContext.Current.Request;
                var handlerId = JsonConvert.DeserializeObject<string>(currentRequest.Params.Get("handlerId"));
                Guid testId = Guid.Parse(handlerId);
                RPASetting setting = Context.Set<RPASetting>()
                    .First(s => s.Id == testId);

                if (setting.Name == "TLC Draft Handler")
                {
                    return DownloadTLCDraftTemplateXLSX();
                }
                else if (setting.Name == "TLC Closed Handler")
                {
                    return DownloadTLCClosedTemplateXLSX();
                }
                else if (setting.Name == "Actual Shelf Price & Discount Handler")
                {
                    return DownloadTLCActualShelfPriceTemplateXLSX();
                }
                else
                {
                    var columnHeaders = JsonConvert.DeserializeObject<RPAEventJsonField>(setting.Json).templateColumns;
                    var columns = columnHeaders.Select(c => JsonConvert.DeserializeObject<Column>(c.ToString()));
                    XLSXExporter exporter = new XLSXExporter(columns);
                    string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
                    string filename = string.Format("{0}Template_{1}.xlsx", "RPA", DateTime.UtcNow.ToString("yyyyddMMHHmmss"));
                    if (!Directory.Exists(exportDir))
                    {
                        Directory.CreateDirectory(exportDir);
                    }
                    string filePath = Path.Combine(exportDir, filename);
                    exporter.Export(Enumerable.Empty<RPA>(), filePath);
                    string file = Path.GetFileName(filePath);
                    return Content(HttpStatusCode.OK, file);
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [ClaimsAuthorize]
        [HttpGet]
        [Route("odata/RPAs/DownloadFile")]
        public HttpResponseMessage DownloadFile(string fileName)
        {
            try
            {
                string directory = Core.Settings.AppSettingsManager.GetSetting("RPA_DIRECTORY", "RPAFiles");
                string type = Core.Settings.AppSettingsManager.GetSetting("HANDLER_LOG_TYPE", "File");
                HttpResponseMessage result;
                switch (type)
                {
                    case "File":
                        {
                            result = FileUtility.DownloadFile(directory, fileName);
                            break;
                        }
                    case "Azure":
                        {
                            result = FileUtility.DownloadFileAzure(directory, fileName);
                            break;
                        }
                    default:
                        {
                            result = FileUtility.DownloadFile(directory, fileName);
                            break;
                        }
                }
                return result;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            return InternalServerError(GetExceptionMessage.GetInnerException(e));
        }

    }
}
