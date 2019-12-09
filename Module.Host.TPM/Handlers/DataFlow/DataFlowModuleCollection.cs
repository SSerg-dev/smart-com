using Core.Data;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Host.TPM.Handlers.DataFlow
{
    public class DataFlowModuleCollection
    {
        public PromoDataFlowModule PromoDataFlowModule { get; }
        public PromoProductDataFlowModule PromoProductDataFlowModule { get; }
        public ProductDataFlowModule ProductDataFlowModule { get; }
        public BaseLineDataFlowModule BaseLineDataFlowModule { get; }
        public AssortmentMatrixDataFlowModule AssortmentMatrixDataFlowModule { get; }
        public ClientTreeDataFlowModule ClientTreeDataFlowModule { get; }
        public ClientTreeBrandTechDataFlowModule ClientTreeBrandTechDataFlowModule { get; }
        public ProductTreeDataFlowModule ProductTreeDataFlowModule { get; }
        public PromoProductTreeDataFlowModule PromoProductTreeDataFlowModule { get; }
        public IncrementalPromoDataFlowModule IncrementalPromoDataFlowModule { get; }
        public ChangesIncidentDataFlowModule ChangesIncidentDataFlowModule { get; }
        public PromoProductsCorrectionDataFlowModule PromoProductsCorrectionDataFlowModule { get; }
        public COGSDataFlowModule COGSDataFlowModule { get; }
        public TradeInvestmentDataFlowModule TradeInvestmentDataFlowModule { get; }

        public DataFlowModuleCollection(DatabaseContext databaseContext)
        {
            this.PromoDataFlowModule = new PromoDataFlowModule(databaseContext);
            this.PromoProductDataFlowModule = new PromoProductDataFlowModule(databaseContext);
            this.ProductDataFlowModule = new ProductDataFlowModule(databaseContext);
            this.BaseLineDataFlowModule = new BaseLineDataFlowModule(databaseContext);
            this.AssortmentMatrixDataFlowModule = new AssortmentMatrixDataFlowModule(databaseContext);
            this.ClientTreeDataFlowModule = new ClientTreeDataFlowModule(databaseContext);
            this.ClientTreeBrandTechDataFlowModule = new ClientTreeBrandTechDataFlowModule(databaseContext);
            this.ProductTreeDataFlowModule = new ProductTreeDataFlowModule(databaseContext);
            this.PromoProductTreeDataFlowModule = new PromoProductTreeDataFlowModule(databaseContext);
            this.IncrementalPromoDataFlowModule = new IncrementalPromoDataFlowModule(databaseContext);
            this.ChangesIncidentDataFlowModule = new ChangesIncidentDataFlowModule(databaseContext);
            this.PromoProductsCorrectionDataFlowModule = new PromoProductsCorrectionDataFlowModule(databaseContext);
            this.COGSDataFlowModule = new COGSDataFlowModule(databaseContext);
            this.TradeInvestmentDataFlowModule = new TradeInvestmentDataFlowModule(databaseContext);
        }
    }
}
