using AutoMapper;
using Interfaces.Implementation.Action;
using LinqToQuerystring;
using Looper.Parameters;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Utils;
using NLog;
using Persist;
using Persist.Model;
using Persist.ScriptGenerator.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utility;

namespace Module.Host.TPM.Actions.Notifications
{
    /// <summary>
    /// Класс для экспорта календаря в EXCEL
    /// </summary>
    public class SchedulerExportAction : BaseAction
    {
        private readonly IEnumerable<int> Clients;

        private readonly int Year;

        private readonly Guid UserId;

        private readonly Guid RoleId;

        private readonly string RawFilters;

        private readonly TPMmode tPMmode;

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public SchedulerExportAction(IEnumerable<int> clients, int year, Guid userId, Guid roleId, string rawFilters, TPMmode tPMmode)
        {
            Clients = clients;
            Year = year;
            UserId = userId;
            RoleId = roleId;
            RawFilters = rawFilters;
            this.tPMmode = tPMmode;
        }
        public override void Execute()
        {
            PerformanceLogger performanceLogger = new PerformanceLogger();
            try
            {
                performanceLogger.Start();
                using (DatabaseContext context = new DatabaseContext())
                {
                    List<PromoView> promoes;
                    IQueryable<PromoView> query;

                    //из библиотеки LinqToQuerystring нашей версии убрали datetimeoffset заменяем
                    var row = RawFilters.Replace("datetimeoffset", "datetime").Replace(".000Z", "").Replace(".00Z", "");
                    //из библиотеки LinqToQuerystring нашей версии нет данных в вииде 12d что есть в нашей версии odata заменяем
                    row = Regex.Replace(row, @"(\d+)[d]", @"$1");
                    var configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<PromoRSView, PromoView>();
                    });
                    switch (tPMmode)
                    {
                        case TPMmode.RS:
                            IQueryable<PromoRSView> queryRS = GetConstraintedQueryRS(context);
                            queryRS = queryRS.LinqToQuerystring(row);
                            var mapper = configuration.CreateMapper();
                            promoes = queryRS.ToList().ConvertAll(x => mapper.Map<PromoView>(x));
                            break;
                        case TPMmode.Current:
                            query = GetConstraintedQuery(context);
                            promoes = query.LinqToQuerystring(row).ToList();
                            break;
                        default:
                            query = GetConstraintedQuery(context);
                            promoes = query.LinqToQuerystring(row).ToList();
                            break;
                    }

                    DateTime startDate = DateTime.Now;
                    DateTime endDate = DateTime.Now;
                    bool yearExport = Year != 0;

                    if (yearExport)
                    {
                        startDate = new DateTime(Year, 1, 1);
                        endDate = new DateTime(Year, 12, 31);
                        promoes = promoes.Where(p => (p.EndDate > startDate && p.EndDate < endDate) || (p.StartDate > startDate && p.StartDate < endDate)).ToList();
                    }
                    if (promoes.Count() == 0)
                    {
                        Errors.Add("No promoes to export");
                    }
                    else
                    {
                        string userName = context.Users.FirstOrDefault(u => u.Id == UserId).Name;
                        SchedulerExporter exporter = yearExport ? new SchedulerExporter(startDate, endDate) : new SchedulerExporter();
                        string filePath = exporter.GetExportFileName(userName);
                        exporter.Export(promoes, Clients, filePath, context);
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
                performanceLogger.Stop();
            }
        }

        /// <summary>
        /// Получение списка промо с учётом ограничений
        /// </summary>
        /// <returns></returns>
        private IQueryable<PromoView> GetConstraintedQuery(DatabaseContext context)
        {
            PerformanceLogger performanceLogger = new PerformanceLogger();
            performanceLogger.Start();
            string role = context.Roles.FirstOrDefault(r => r.Id == RoleId).SystemName;
            IList<Constraint> constraints = context.Constraints
                .Where(x => x.UserRole.UserId.Equals(UserId) && x.UserRole.Role.SystemName.Equals(role))
                .ToList();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoView> query = context.Set<PromoView>();
            IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, Persist.TPM.Model.Interfaces.TPMmode.Current, filters, FilterQueryModes.Active, String.Empty);
            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == UserId);
            }
            performanceLogger.Stop();
            return query;
        }

        /// <summary>
        /// Получение списка промо с учётом ограничений
        /// </summary>
        /// <returns></returns>
        private IQueryable<PromoRSView> GetConstraintedQueryRS(DatabaseContext context)
        {
            PerformanceLogger performanceLogger = new PerformanceLogger();
            performanceLogger.Start();
            string role = context.Roles.FirstOrDefault(r => r.Id == RoleId).SystemName;
            IList<Constraint> constraints = context.Constraints
                .Where(x => x.UserRole.UserId.Equals(UserId) && x.UserRole.Role.SystemName.Equals(role))
                .ToList();
            IDictionary<string, IEnumerable<string>> filters = FilterHelper.GetFiltersDictionary(constraints);
            IQueryable<PromoRSView> query = context.Set<PromoRSView>();
            IQueryable<ClientTreeHierarchyView> hierarchy = context.Set<ClientTreeHierarchyView>().AsNoTracking();
            query = ModuleApplyFilterHelper.ApplyFilter(query, hierarchy, Persist.TPM.Model.Interfaces.TPMmode.Current, filters, FilterQueryModes.Active, String.Empty);
            // Не администраторы не смотрят чужие черновики
            if (role != "Administrator" && role != "SupportAdministrator")
            {
                query = query.Where(e => e.PromoStatusSystemName != "Draft" || e.CreatorId == UserId);
            }
            performanceLogger.Stop();
            return query;
        }
    }
}