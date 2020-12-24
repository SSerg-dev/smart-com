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

namespace Module.Host.TPM.Actions.Notifications
{
    public class ExportAction<TModel, TKey> : BaseAction where TModel : class, IEntity<TKey>
    {

        private Guid UserId;
        private Guid RoleId;
        private IEnumerable<Column> Columns;
        private string SqlString;
        private Type DBModel;
        private bool SimpleModel;
        private bool IsActuals;
        private string CustomFileName;

        private readonly object locker = new object();

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();
        
        public ExportAction(Guid userId, Guid roleId, IEnumerable<Column> columns, string sqlString, Type dbModel, bool simpleModel, bool isActuals = false, string customFileName = null)
        {
            UserId = userId;
            RoleId = roleId;
            Columns = columns;
            SqlString = sqlString;
            DBModel = dbModel;
            SimpleModel = simpleModel;
            IsActuals = isActuals;
            CustomFileName = customFileName;
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
                        IQueryable records = null;

                        if (DBModel != null)
                        {
                            List<object> ids = context.Database.SqlQuery<TModel>(SqlString).Select(q => q.Id as object).ToList();
                            records = ConvertHelper.Convert(ids, DBModel, typeof(TModel), context);
                        }
                        else if (typeof(TModel).Name.Equals(typeof(PlanIncrementalReport).Name))
                        {
                            IEnumerable<string> ids = context.Database.SqlQuery<PlanIncrementalReport>(SqlString).Select(q => q.PromoNameId).AsEnumerable();
                            records = context.Set<PlanIncrementalReport>().Where(q => ids.Contains(q.PromoNameId)).AsQueryable();
                        }
                        else if (typeof(TModel).Name.Equals(typeof(PromoProduct).Name) && IsActuals == true)
                        {
                            records = getActuals(context, SqlString);
                        }
                        else if (typeof(TModel).Name.Equals(typeof(PlanPostPromoEffectReportWeekView).Name))
                        {
                            var dbRecords = context.Database.SqlQuery<TModel>(SqlString).AsQueryable();
                            records = PlanPostPromoEffectReportsController.MapToReport(dbRecords);
                        }
                        else if (SimpleModel)
                        {
                            records = context.Database.SqlQuery<TModel>(SqlString).AsQueryable();
                        }
                        else
                        {
                            List<TModel> tmp = new List<TModel>();
                            IEnumerable<TKey> ids = context.Database.SqlQuery<TModel>(SqlString).Select(q => q.Id).AsEnumerable();
                            foreach (IEnumerable<TKey> idsPart in ids.Partition(1000))
                            {
                                tmp.AddRange(context.Set<TModel>().Where(q => idsPart.Contains(q.Id)));
                            }
                            records = tmp.AsQueryable();
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
                string msg = String.Format("Error exporting calendar: {0}", e.ToString());
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
    }
}