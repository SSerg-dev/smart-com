using Core.Dependency;
using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    /// <summary>
    /// Класс для проверки наличия новых клиентов из DataLake и создания инцидента для отправки нотификации
    /// </summary>
    public class MarsCustomersCheckAction : BaseAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    var marsCustomers = context.SqlQuery<MarsUniversalPetcareCustomers>
                       (@"
                        SELECT ZCUSTHG02, ZCUSTHG02___T, ZCUSTHG03, ZCUSTHG03___T, ZCUSTHG04, ZCUSTHG04___T
                        FROM [DefaultSchemaSetting].MARS_UNIVERSAL_PETCARE_CUSTOMERS
                        WHERE Active_Till IS NULL
                        ").ToList();

                    if (marsCustomers != null && marsCustomers.Count > 0)
                    {
                        List<Tuple<string, string>> pairList = new List<Tuple<string, string>>();
                        List<Tuple<string, string>> incidentValues = new List<Tuple<string, string>>();
                        List<string> addedNodes = new List<string>();

                        var uniqueValues = marsCustomers.Where(n => n.ZCUSTHG02 != "0" && n.ZCUSTHG02 != null).Select(x => x.ZCUSTHG02).Distinct().ToList();
                        foreach (var value in uniqueValues)
                        {
                            pairList.Add(new Tuple<string, string>("ZCUSTHG02", value));
                        }

                        uniqueValues = marsCustomers.Where(n => n.ZCUSTHG03 != "0" && n.ZCUSTHG03 != null).Select(x => x.ZCUSTHG03).Distinct().ToList();
                        foreach (var value in uniqueValues)
                        {
                            pairList.Add(new Tuple<string, string>("ZCUSTHG03", value));
                        }

                        uniqueValues = marsCustomers.Where(n => n.ZCUSTHG04 != "0" && n.ZCUSTHG04 != null).Select(x => x.ZCUSTHG04).Distinct().ToList();
                        foreach (var value in uniqueValues)
                        {
                            pairList.Add(new Tuple<string, string>("ZCUSTHG04", value));
                        }

                        var clientTreeNodes = context.Set<ClientTree>().Where(x => !x.EndDate.HasValue);
                        var clientGhierarchyCodeList = clientTreeNodes.Select(x => x.GHierarchyCode).ToList();

                        // упорядочиваем, чтобы проверка начиналась с младших узлов
                        pairList = pairList.OrderByDescending(x => x.Item1).ToList();

                        foreach (var pair in pairList)
                        {
                            if (!clientGhierarchyCodeList.Contains(pair.Item2))
                            {
                                if (!addedNodes.Contains(pair.Item2))
                                {
                                    incidentValues.Add(pair);

                                    // добавили узел = попала вся ветка (чтобы не было дубликатов в нотификации)
                                    if (pair.Item1 == "ZCUSTHG02")
                                    {
                                        addedNodes.Add(pair.Item2);
                                    }
                                    else if (pair.Item1 == "ZCUSTHG03")
                                    {
                                        var rec = marsCustomers.Where(x => x.ZCUSTHG03 == pair.Item2).FirstOrDefault();
                                        addedNodes.Add(rec.ZCUSTHG02);
                                        addedNodes.Add(rec.ZCUSTHG03);
                                    }
                                    else if (pair.Item1 == "ZCUSTHG04")
                                    {
                                        var rec = marsCustomers.Where(x => x.ZCUSTHG04 == pair.Item2).FirstOrDefault();
                                        addedNodes.Add(rec.ZCUSTHG02);
                                        addedNodes.Add(rec.ZCUSTHG03);
                                        addedNodes.Add(rec.ZCUSTHG04);
                                    }
                                }
                            }
                        }

                        foreach (var incident in incidentValues)
                        {
                            ClientTreeNeedUpdateIncident clientTreeNeedUpdateIncident = new ClientTreeNeedUpdateIncident
                            {
                                Disabled = false,
                                DeletedDate = null,
                                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                ProcessDate = null,
                                PropertyName = incident.Item1,
                                PropertyValue = incident.Item2
                            };
                            context.Set<ClientTreeNeedUpdateIncident>().Add(clientTreeNeedUpdateIncident);
                        }

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while cheking Mars customers", e.ToString());
                Errors.Add(msg);
            }
        }
    }
}