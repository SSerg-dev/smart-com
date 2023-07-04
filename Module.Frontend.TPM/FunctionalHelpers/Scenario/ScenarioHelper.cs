using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Persist.TPM.Model.Interfaces;
using Module.Frontend.TPM.FunctionalHelpers.RA;

namespace Module.Frontend.TPM.FunctionalHelpers.Scenario
{
    public static class ScenarioHelper
    {
        public static void CreateScenarioPeriod(Promo promo, DatabaseContext Context, TPMmode tPMmode)
        {
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == promo.ClientTreeId);
            if (tPMmode == TPMmode.RS)
            {
                CreateRSPeriod(promo, client, Context);
            }
            if (tPMmode == TPMmode.RA)
            {
                CreateRAPeriod(promo, client, Context);
            }

            Context.SaveChanges();
        }
        public static void CreateScenarioPeriod(List<Promo> promoes, DatabaseContext Context, TPMmode tPMmode)
        {
            foreach (Promo promo in promoes)
            {
                CreateScenarioPeriod(promo, Context, tPMmode);
            }
        }
        private static void CreateRSPeriod(Promo promo, ClientTree client, DatabaseContext Context)
        {
            List<string> outStatuses = new List<string> { RSstateNames.WAITING, RSstateNames.APPROVED };
            StartEndModel startEndModel = RSPeriodHelper.GetRSPeriod(Context);

            RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .FirstOrDefault(g => g.ClientTreeId == promo.ClientTreeKeyId && !g.Disabled && !outStatuses.Contains(g.RSstatus));

            if (rollingScenarioExist == null)
            {
                RollingScenario rollingScenario = new RollingScenario
                {
                    StartDate = startEndModel.StartDate,
                    EndDate = startEndModel.EndDate,
                    RSstatus = RSstateNames.DRAFT,
                    ClientTree = client,
                    Promoes = new List<Promo>()
                };
                rollingScenario.Promoes.Add(promo);
                Context.Set<RollingScenario>().Add(rollingScenario);
            }
            else
            {
                rollingScenarioExist.Promoes.Add(promo);
            }
        }
        private static void CreateRAPeriod(Promo promo, ClientTree client, DatabaseContext Context)
        {
            List<string> outStatuses = new List<string> { RSstateNames.WAITING, RSstateNames.APPROVED };
            StartEndModel startEndModel = RAmodeHelper.GetRAPeriod();

            RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .FirstOrDefault(g => g.ClientTreeId == promo.ClientTreeKeyId && !g.Disabled && !outStatuses.Contains(g.RSstatus));
            
            if (rollingScenarioExist == null)
            {
                RollingScenario rollingScenario = new RollingScenario
                {
                    StartDate = startEndModel.StartDate,
                    EndDate = startEndModel.EndDate,
                    RSstatus = RSstateNames.DRAFT,
                    ClientTree = client,
                    Promoes = new List<Promo>()
                };
                rollingScenario.Promoes.Add(promo);
                Context.Set<RollingScenario>().Add(rollingScenario);
            }
            else
            {
                rollingScenarioExist.Promoes.Add(promo);
            }
        }

        public static RollingScenario GetActiveScenario(int clientObjectId, DatabaseContext Context)
        {
            List<string> activeStatuses = new List<string> { RSstateNames.DRAFT, RSstateNames.ON_APPROVAL };
            return Context.Set<RollingScenario>().Include(x => x.Promoes).SingleOrDefault(x => !x.Disabled 
                        && activeStatuses.Contains(x.RSstatus) 
                        && x.ClientTree.ObjectId == clientObjectId);
        }
    } 
}
