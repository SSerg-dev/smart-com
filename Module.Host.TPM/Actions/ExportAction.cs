using System;
using Persist;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using System.Collections.Generic;
using Persist.Model;
using Utility;
using Module.Persist.TPM.Model.DTO;
using Persist.ScriptGenerator.Filter;
using Module.Persist.TPM.Utils;
using Looper.Parameters;
using Interfaces.Implementation.Action;
using NLog;
using System.Web.Http.OData.Query;
using System.Linq.Expressions;
using LinqToQuerystring;
using System.Text.RegularExpressions;
using Frontend.Core.Extensions.Export;
using Module.Frontend.TPM.Util;
using Core.Security.Models;
using Core.Data;
using Raven.Abstractions.Extensions;
using Core.Settings;
using System.IO;
using System.Reflection;
using System.Data.Entity;
using Module.Host.TPM.Util;
using System.Collections;
using Module.Frontend.TPM.Controllers;
using Core.Extensions;
using System.Net.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData;
using Module.Persist.TPM.Model.Interfaces;

namespace Module.Host.TPM.Actions.Notifications
{
    public class ExportAction<TModel, TKey> : BaseAction where TModel : class, IEntity<TKey>
    {

        private Guid UserId;
        private UserInfo User;
        private Guid RoleId;
        private string Role;
        private IEnumerable<Column> Columns;
        private string SqlString;
        private Type DBModel;
        private bool SimpleModel;
        private bool IsActuals;
        private string CustomFileName;
        private TPMmode TPMmode;
        private string url;

        private readonly object locker = new object();

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public ExportAction(Guid userId, Guid roleId, IEnumerable<Column> columns, string sqlString, Type dbModel, bool simpleModel, string URL, bool isActuals = false, string customFileName = null, TPMmode tPMmode = TPMmode.Current)
        {
            UserId = userId;
            RoleId = roleId;
            Columns = columns;
            SqlString = sqlString;
            DBModel = dbModel;
            SimpleModel = simpleModel;
            url = URL;
            IsActuals = isActuals;
            CustomFileName = customFileName;
            TPMmode = tPMmode;

            User = getUserInfo(userId);
            Role = GetRole(roleId);
        }
        public override void Execute()
        {
            try
            {
                //При запуске несколько раз ломается, по этому нужен лок

                lock (locker)
                {
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        IList records = null;

                        if (DBModel != null)
                        {
                            List<object> ids = context.Database.SqlQuery<TModel>(SqlString).Select(q => q.Id as object).ToList();
                            records = ConvertHelper.Convert<TModel>(ids, DBModel, typeof(TModel), context);
                        }
                        else if (typeof(TModel).Name.Equals(typeof(PlanIncrementalReport).Name))
                        {
                            var options = getODataQueryOptions<TModel>();
                            records = options.ApplyTo(new PlanIncrementalReportsController(User, Role, RoleId).GetConstraintedQuery(true, context)).Cast<PlanIncrementalReport>().ToList();
                        }
                        else if (typeof(TModel).Name.Equals(typeof(ActualLSV).Name))
                        {
                            var options = getODataQueryOptions<TModel>();
                            records = ((IQueryable<ActualLSV>)options.ApplyTo(new ActualLSVsController(User, Role, RoleId).GetConstraintedQuery(context))).ToList();
                        }
                        else if (typeof(TModel).Name.Equals(typeof(PromoProduct).Name) && IsActuals == true)
                        {
                            records = getActuals(context, SqlString).ToList();
                        }
                        else if (typeof(TModel).Name.Equals(typeof(PlanPostPromoEffectReportWeekView).Name))
                        {
                            //TODO: починить
                            var dbRecords = context.Database.SqlQuery<TModel>(SqlString).AsQueryable();
                            records = PlanPostPromoEffectReportsController.MapToReport<PlanPostPromoEffectReport>(dbRecords).ToList();
                        }
                        else if (typeof(TModel).Name.Equals(typeof(PromoGridView).Name))
                        {
                            var options = getODataQueryOptions<TModel>();
                            records = options.ApplyTo(new PromoGridViewsController(User, Role, RoleId).GetConstraintedQuery(tPMmode: TPMmode, localContext: context)).Cast<PromoGridView>().ToList();
                        }
                        else if(typeof(TModel).Name.Equals(typeof(AssortmentMatrix).Name))
						{
                            records = getAssortmentMatrices(context, SqlString, IsActuals);
                        }
                        else if (typeof(TModel).Name.Equals(typeof(CompetitorPromo).Name))
                        {
                            records = GetCompetitorPromoes(context, SqlString);
                        }
                        else
                        {
                            records = context.Database.SqlQuery<TModel>(SqlString).ToList();
                        }

                        if ( records.Count > 0)
                        {
                            AddChildrenModel(records, context);
                        }

                        XLSXExporter exporter = new XLSXExporter(Columns);
                        var user = context.Set<User>().FirstOrDefault(u => u.Id == UserId);
                        string username = user == null ? "" : user.Name;
                        string filePath = exporter.GetExportFileName(string.IsNullOrEmpty(CustomFileName) ? typeof(TModel).Name : CustomFileName, username);
                        exporter.Export(records, filePath);
                        string fileName = System.IO.Path.GetFileName(filePath);

                        FileModel file = new FileModel()
                        {
                            LogicType = "Export",
                            Name = System.IO.Path.GetFileName(fileName),
                            DisplayName = System.IO.Path.GetFileName(fileName)
                        };
                        Results.Add("ExportFile", file);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("Error exporting: {0}", e.ToString());
                logger.Error(msg);
                Errors.Add(msg);
            }
            finally
            {
                logger.Trace("Finish");
            }
        }

        private IList getAssortmentMatrices(DatabaseContext context, string sqlQuery, bool IsActuals)
		{
            var records = context.Database.SqlQuery<AssortmentMatrix>(SqlString).ToList();
            var resultAssortmentMatrix = new List<AssortmentMatrix>();
            var clientIds = records.GroupBy(x => x.ClientTreeId).Select(x => x.Key).ToList();
            var plu = context.Set<Plu>().Where(x => clientIds.Contains(x.ClientTreeId)).ToList();
            var assortmentMatrix2Plus = context.Set<AssortmentMatrix2Plu>().Where(x => clientIds.Contains(x.ClientTreeId)).ToList();
            foreach(var item in records)
			{
				var found = assortmentMatrix2Plus.SingleOrDefault(x => x.Id == item.Id);
				if (found != null)
				{
					item.Plu = new AssortmentMatrix2Plu() { PluCode = found.PluCode };
				}
			}
            if (IsActuals)
            {
                var clientProductAssortmentMatrixGroups = records.GroupBy(x => new { x.ClientTreeId, x.ProductId });

                foreach (var clientProductAssortmentMatrixGroup in clientProductAssortmentMatrixGroups)
                {
                    var record = clientProductAssortmentMatrixGroup
                        .Where(x => x.EndDate >= new DateTime(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day) && !x.Disabled)
                        .OrderByDescending(x => x.CreateDate).FirstOrDefault();

                    if (record != null)
                    {
                        resultAssortmentMatrix.Add(record);
                    }
                }
                return resultAssortmentMatrix;
            }
            return records;
        }

        private IList GetCompetitorPromoes(DatabaseContext context, string sqlQuery)
        {
            var records = context.Database.SqlQuery<CompetitorPromo>(SqlString).ToList();
            var clientIds = context.Set<ClientTree>().ToList();
            foreach (var item in records)
            {
                var found = clientIds.SingleOrDefault(x => x.Id == item.ClientTreeObjectId.Value);
                if (found != null)
                {
                    item.ClientTree = found;
                }
            }
            return records;
        }


        private class ExportPromoProduct : PromoProduct
		{
            public string PluCode { get; set; }
        }

        private IQueryable<ExportPromoProduct> getActuals(DatabaseContext context, string sqlQuery)
        {
            
            var sumGroup = context.Database.SqlQuery<ExportPromoProduct>(sqlQuery)
                                                      .GroupBy(x => x.EAN_PC)
                                                      .Select(s => new
                                                      {
                                                          sumActualProductPCQty = s.Sum(x => x.ActualProductPCQty),
                                                          sumActualProductPCLSV = s.Sum(x => x.ActualProductPCLSV),
                                                          promoProduct = s.Select(x => x)
                                                      })
                                                      .ToList();

            List<ExportPromoProduct> promoProductList = new List<ExportPromoProduct>();
            foreach (var item in sumGroup)
            {
                ExportPromoProduct pp = item.promoProduct.ToList()[0];
                pp.ActualProductPCQty = item.sumActualProductPCQty;
                pp.ActualProductPCLSV = item.sumActualProductPCLSV;
                pp.Plu = new PromoProduct2Plu() { PluCode = pp.PluCode };
                promoProductList.Add(pp);
            }

            return promoProductList.AsQueryable();
        }

        private ODataQueryOptions<T> getODataQueryOptions<T>()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.AddEntity(typeof(T));
            var edmModel = modelBuilder.GetEdmModel();
            var oDataQueryContext = new ODataQueryContext(edmModel, typeof(T));
            return new ODataQueryOptions<T>(oDataQueryContext, request);
        }

        private UserInfo getUserInfo(Guid userId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId && !u.Disabled);
                return new UserInfo(null)
                {
                    Id = user != null ? user.Id : (Guid?)null,
                    Login = user.Name
                };
            }
        }

        private string GetRole(Guid roleId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Roles.FirstOrDefault(u => u.Id == roleId && !u.Disabled).SystemName;
            }
        }

        private void AddChildrenModel(IList records, DatabaseContext context)
        {
            Type type = typeof(TModel);
            Type propType;
            var properties = type.GetProperties();
            List<string> modelTypes = new List<string>();
            List<KeyValuePair<string, List<IEntity>>> valuesToInsert = new List<KeyValuePair<string, List<IEntity>>>();
            foreach (var prop in properties)
            {
                propType = prop.Type();
                var name = propType.Name;
                if (propType.GetInterfaces().Contains(typeof(IEntity<Guid>)))
                {
                    KeyValuePair<string, List<IEntity>> tempValuesToInsert = new KeyValuePair<string, List<IEntity>>();
                    modelTypes.Add(propType.ToString());

                    GetType()
                        .GetMethod("GetDBList", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(propType, typeof(Guid)).Invoke(this, new object[] { context, records, type });
                    valuesToInsert.Add(tempValuesToInsert);
                }
                else if (propType.GetInterfaces().Contains(typeof(IEntity<int>)))
                {
                    KeyValuePair<string, List<IEntity>> tempValuesToInsert = new KeyValuePair<string, List<IEntity>>();
                    modelTypes.Add(propType.ToString());

                    GetType()
                        .GetMethod("GetDBList", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(propType, typeof(int)).Invoke(this, new object[] { context, records, type });
                    valuesToInsert.Add(tempValuesToInsert);
                }
            }
        }

        private void GetDBList<TEntity, TId>(DatabaseContext context, IList records, Type type) where TEntity : class, IEntity<TId>
        {
            TEntity singleValue;
            var toInsert = context.Set<TEntity>();
            string tableName = typeof(TEntity).Name;
            var idProp = type.GetProperty(tableName + "Id");
            if (idProp != null)
            {
                foreach (var rec in records)
                {
                    var modelId = idProp.GetValue(rec);
                    if (modelId != null)
                    {
                        singleValue = toInsert.Where(x => x.Id.ToString().Equals(modelId.ToString())).FirstOrDefault();
                        type.GetProperty(tableName).SetValue(rec, singleValue);
                    }
                }
            }
        }
    }
}