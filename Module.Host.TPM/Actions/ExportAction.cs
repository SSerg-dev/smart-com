﻿using System;
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
        private string url;

        private readonly object locker = new object();

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public ExportAction(Guid userId, Guid roleId, IEnumerable<Column> columns, string sqlString, Type dbModel, bool simpleModel, string URL, bool isActuals = false, string customFileName = null)
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
                        else
                        {
                            records = context.Database.SqlQuery<TModel>(SqlString).ToList();
                        }

                        if (url == null)
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

        private IQueryable<PromoProduct> getActuals(DatabaseContext context, string sqlQuery)
        {
            var sumGroup = context.Database.SqlQuery<PromoProduct>(sqlQuery)
                                                      .GroupBy(x => x.EAN_PC)
                                                      .Select(s => new
                                                      {
                                                          sumActualProductPCQty = s.Sum(x => x.ActualProductPCQty),
                                                          sumActualProductPCLSV = s.Sum(x => x.ActualProductPCLSV),
                                                          promoProduct = s.Select(x => x)
                                                      })
                                                      .ToList();

            List<PromoProduct> promoProductList = new List<PromoProduct>();
            foreach (var item in sumGroup)
            {
                PromoProduct pp = item.promoProduct.ToList()[0];
                pp.ActualProductPCQty = item.sumActualProductPCQty;
                pp.ActualProductPCLSV = item.sumActualProductPCLSV;

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
                if (propType.GetInterfaces().Contains(typeof(IEntity)))
                {
                    KeyValuePair<string, List<IEntity>> tempValuesToInsert = new KeyValuePair<string, List<IEntity>>();
                    modelTypes.Add(propType.ToString());

                    GetType()
                        .GetMethod("GetDBList", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(propType).Invoke(this, new object[] { context, records, type });
                    valuesToInsert.Add(tempValuesToInsert);
                }
            }

            //AddChildrenToRecord(valuesToInsert, records, type);
        }

        private void GetDBList<TEntity>(DatabaseContext context, IList records, Type type) where TEntity : class, IEntity
        {
            TEntity singleValue;
            var toInsert = context.Set<TEntity>().ToList();
            string tableName = typeof(TEntity).Name;
            foreach (var rec in records)
            {
                var modelId = type.GetProperty(tableName + "Id").GetValue(rec);
                var sss = modelId.GetType().Name;
                if (modelId.GetType().Name == "Guid")
                {
                    singleValue = toInsert.Where(x => (Guid)x.GetType().GetProperty("Id").GetValue(x) == (Guid)modelId).FirstOrDefault();
                    type.GetProperty(tableName).SetValue(rec, singleValue);
                }
                else if (modelId.GetType().Name == "Int32")
                {
                    var sds = (int)modelId;
                    singleValue = toInsert.Where(x => (int)x.GetType().GetProperty("Id").GetValue(x) == (int)modelId).FirstOrDefault();
                    type.GetProperty(tableName).SetValue(rec, singleValue);
                }
            }
        }

        private void AddChildrenToRecord(List<KeyValuePair<string, List<IEntity>>> valuesToInsert, IList records, Type type)
        {
            List<IEntity> values;
            string key;
            IEntity singleValue;
            foreach (var keyValue in valuesToInsert)
            {
                values = keyValue.Value;
                key = keyValue.Key;
                foreach (var rec in records)
                {
                    var modelId = type.GetProperty(keyValue.Key + "Id").GetValue(rec);
                    if (modelId.GetType().Name == "Guid")
                    {
                        singleValue = values.Where(x => (Guid)x.GetType().GetProperty("Id").GetValue(rec) == (Guid)modelId).FirstOrDefault();
                        type.GetProperty(keyValue.Key).SetValue(rec, singleValue);
                    }
                    else if (modelId.GetType().Name == "int")
                    {
                        singleValue = values.Where(x => (int)x.GetType().GetProperty("Id").GetValue(rec) == (int)modelId).FirstOrDefault();
                        type.GetProperty(keyValue.Key).SetValue(rec, singleValue);
                    }
                }
            }
        }
    }
}