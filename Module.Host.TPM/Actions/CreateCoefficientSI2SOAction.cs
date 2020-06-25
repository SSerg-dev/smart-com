using AutoMapper;
using Core.Data;
using Core.Extensions;
using Core.History;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.History;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    class CreateCoefficientSI2SOAction : BaseAction
    {
        private readonly List<string> BrandTechCode;
        private readonly string DemandCode;
        private readonly double CValue;

        public CreateCoefficientSI2SOAction(List<string> brandTechCode, string demandCode, double cValue)
        {
            BrandTechCode = brandTechCode;
            DemandCode = demandCode;
            CValue = cValue;
        }

        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    var clientDemandCode = context.Set<ClientTree>().Where(c => c.DemandCode == DemandCode && !c.EndDate.HasValue).Select(c => c.DemandCode).FirstOrDefault();

                    List<Guid> brandTechIds = new List<Guid>(); 

                    //Если добавляется новый BrandTech - создаем записи со всеми Demand Code
                    if (BrandTechCode != null)
                    {

                        foreach (string bTCode in BrandTechCode)
                        {
                            brandTechIds.Add(context.Set<BrandTech>().Where(b => b.BrandsegTechsub_code == bTCode && !b.Disabled).Select(b => b.Id).FirstOrDefault());
                        }

                        var demandCodes = context.Set<ClientTree>().Where(d => !d.EndDate.HasValue).Select(d => d.DemandCode).Distinct().ToList();

                        foreach (string demCode in demandCodes)
                        {
                            if (demCode != "" && demCode != null)
                            {
                                foreach (Guid id in brandTechIds)
                                {
                                        CoefficientSI2SO newRecord = new CoefficientSI2SO { Id = Guid.NewGuid(), DemandCode = demCode, BrandTechId = id, CoefficientValue = CValue, Disabled = false };
                                    var proxy = context.Set<CoefficientSI2SO>().Create<CoefficientSI2SO>();
                                    var result = (CoefficientSI2SO)Mapper.Map(newRecord, proxy, typeof(CoefficientSI2SO), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
                                    context.Set<CoefficientSI2SO>().Add(result);
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                

                    //Если добавляется новый DemandCode - создаем записи со всеми Brand Tech
                    if (DemandCode != null)
                    {
                        bool haveDC = context.Set<CoefficientSI2SO>().Where(c => c.DemandCode == DemandCode).Any();

                        if(!haveDC)
                        {
                            var brandTechCodes = context.Set<BrandTech>().Where(b => !b.Disabled).Select(b => b.Id).Distinct().ToList();

                            foreach (Guid brTechCode in brandTechCodes)
                            {
                                CoefficientSI2SO newRecord = new CoefficientSI2SO { Id = Guid.NewGuid(), DemandCode = clientDemandCode, BrandTechId = brTechCode, CoefficientValue = CValue, Disabled = false };
                                var proxy = context.Set<CoefficientSI2SO>().Create<CoefficientSI2SO>();
                                var result = (CoefficientSI2SO)Mapper.Map(newRecord, proxy, typeof(CoefficientSI2SO), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
                                context.Set<CoefficientSI2SO>().Add(result);
                                context.SaveChanges();
                            }
                        }
                        else 
                        {
                            Errors.Add("Records with that Demand Code already exists");
                        }

                    }
                }
            }

            catch (Exception e)
            {
                string msg = String.Format("An error occurred while inserting: {0}", e.ToString());
                Errors.Add(msg);
            }
        }

        private void proxy(IMappingOperationOptions obj)
        {
            throw new NotImplementedException();
        }
    }


}