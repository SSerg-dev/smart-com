﻿Ext.define('App.view.tpm.promoroireport.PromoROIReportEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promoroireporteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    noChange: true,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [
           
            { xtype: 'numberfield', name: 'Number', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('Number') },

{ xtype: 'textfield', name: 'Client1LevelName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('Client1LevelName') },
{ xtype: 'textfield', name: 'Client2LevelName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('Client2LevelName') },
{ xtype: 'textfield', name: 'ClientName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ClientName') },

{ xtype: 'textfield', name: 'BrandName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('BrandName') },
{ xtype: 'textfield', name: 'TechnologyName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('TechnologyName') },
{ xtype: 'textfield', name: 'ProductSubrangesList', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ProductSubrangesList') },

{ xtype: 'textfield', name: 'MarsMechanicName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicName') },
{ xtype: 'textfield', name: 'MarsMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicTypeName') },
{ xtype: 'numberfield', name: 'MarsMechanicDiscount', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicDiscount') },
{ xtype: 'textfield', name: 'MechanicComment', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('MechanicComment') },

{ xtype: 'datefield', name: 'StartDate', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('StartDate') },
{ xtype: 'datefield', name: 'EndDate', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('EndDate') },
{ xtype: 'numberfield', name: 'PromoDuration', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PromoDuration') },

{ xtype: 'numberfield', name: 'DispatchDuration', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('DispatchDuration') },

{ xtype: 'textfield', name: 'EventName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('EventName') },
{ xtype: 'textfield', name: 'PromoStatusName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PromoStatusName') },

{ xtype: 'textfield', name: 'PlanInstoreMechanicName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicName') },
{ xtype: 'textfield', name: 'PlanInstoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicTypeName') },
{ xtype: 'numberfield', name: 'PlanInstoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicDiscount') },

{ xtype: 'numberfield', name: 'PlanPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineLSV') },
{ xtype: 'numberfield', name: 'PlanPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalLSV') },
{ xtype: 'numberfield', name: 'PlanPromoLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoLSV') },
{ xtype: 'numberfield', name: 'PlanPromoUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoUpliftPercent') },
{ xtype: 'numberfield', name: 'PlanPromoTIShopper', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTIShopper') },
{ xtype: 'numberfield', name: 'PlanPromoTIMarketing', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTIMarketing') },
{ xtype: 'numberfield', name: 'PlanPromoXSites', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoXSites') },
{ xtype: 'numberfield', name: 'PlanPromoCatalogue', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCatalogue') },
{ xtype: 'numberfield', name: 'PlanPromoPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPOSMInClient') },
{ xtype: 'numberfield', name: 'PlanPromoBranding', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBranding') },
{ xtype: 'numberfield', name: 'PlanPromoBTL', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBTL') },
{ xtype: 'numberfield', name: 'PlanPromoCostProduction', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProduction') },
{ xtype: 'numberfield', name: 'PlanPromoCostProdXSites', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdXSites') },
{ xtype: 'numberfield', name: 'PlanPromoCostProdCatalogue', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdCatalogue') },
{ xtype: 'numberfield', name: 'PlanPromoCostProdPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdPOSMInClient') },
{ xtype: 'numberfield', name: 'PlanPromoCost', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCost') },
{ xtype: 'numberfield', name: 'PlanPromoIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalBaseTI') },

{ xtype: 'numberfield', name: 'PlanPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalCOGS') },
{ xtype: 'numberfield', name: 'PlanPromoTotalCost', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTotalCost') },
{ xtype: 'numberfield', name: 'PlanPostPromoEffectW1', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPostPromoEffectW1') },
{ xtype: 'numberfield', name: 'PlanPostPromoEffectW2', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPostPromoEffectW2') },
{ xtype: 'numberfield', name: 'PlanPostPromoEffect', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPostPromoEffect') },
{ xtype: 'numberfield', name: 'PlanPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalLSV') },
{ xtype: 'numberfield', name: 'PlanPromoNetLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetLSV') },

{ xtype: 'numberfield', name: 'PlanPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineBaseTI') },
{ xtype: 'numberfield', name: 'PlanPromoBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaseTI') },
{ xtype: 'numberfield', name: 'PlanPromoNetNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetNSV') },

{ xtype: 'numberfield', name: 'PlanPromoIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalNSV') },
{ xtype: 'numberfield', name: 'PlanPromoNetIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalNSV') },
{ xtype: 'numberfield', name: 'PlanPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalMAC') },
{ xtype: 'numberfield', name: 'PlanPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalMAC') },
{ xtype: 'numberfield', name: 'PlanPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalEarnings') },
{ xtype: 'numberfield', name: 'PlanPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalEarnings') },
{ xtype: 'numberfield', name: 'PlanPromoROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoROIPercent') },
{ xtype: 'numberfield', name: 'PlanPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetROIPercent') },
{ xtype: 'numberfield', name: 'PlanPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetUpliftPercent') },

{ xtype: 'textfield', name: 'ActualInStoreMechanicName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreMechanicName') },
{ xtype: 'textfield', name: 'ActualInStoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreMechanicTypeName') },
{ xtype: 'numberfield', name: 'ActualInStoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreMechanicDiscount') },
{ xtype: 'numberfield', name: 'ActualInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreShelfPrice') },
{ xtype: 'textfield', name: 'InvoiceNumber', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('InvoiceNumber') },
{ xtype: 'numberfield', name: 'ActualPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineLSV') },
{ xtype: 'numberfield', name: 'ActualPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalLSV') },
{ xtype: 'numberfield', name: 'ActualPromoLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSV') },
{ xtype: 'numberfield', name: 'ActualPromoUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoUpliftPercent') },
{ xtype: 'numberfield', name: 'ActualPromoTIShopper', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIShopper') },
{ xtype: 'numberfield', name: 'ActualPromoTIMarketing', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIMarketing') },

//{ xtype: 'numberfield', name: 'ActualPromoProdXSites', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoProdXSites') },
//{ xtype: 'numberfield', name: 'ActualPromoProdCatalogue', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoProdCatalogue') },
//{ xtype: 'numberfield', name: 'ActualPromoProdPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoProdPOSMInClient') },


{ xtype: 'numberfield', name: 'ActualPromoXSites', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoXSites') },
{ xtype: 'numberfield', name: 'ActualPromoCatalogue', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCatalogue') },
{ xtype: 'numberfield', name: 'ActualPromoPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPOSMInClient') },


{ xtype: 'numberfield', name: 'ActualPromoBranding', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBranding') },
{ xtype: 'numberfield', name: 'ActualPromoBTL', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBTL') },
{ xtype: 'numberfield', name: 'ActualPromoCostProduction', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProduction') },

{ xtype: 'numberfield', name: 'ActualPromoCostProdXSites', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdXSites') },
{ xtype: 'numberfield', name: 'ActualPromoCostProdCatalogue', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdCatalogue') },
{ xtype: 'numberfield', name: 'ActualPromoCostProdPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdPOSMInClient') },

{ xtype: 'numberfield', name: 'ActualPromoCost', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCost') },
{ xtype: 'numberfield', name: 'ActualPromoIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalBaseTI') },
{ xtype: 'numberfield', name: 'ActualPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalCOGS') },
{ xtype: 'numberfield', name: 'ActualPromoTotalCost', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTotalCost') },

{ xtype: 'numberfield', name: 'FactPostPromoEffectW1', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('FactPostPromoEffectW1') },
{ xtype: 'numberfield', name: 'FactPostPromoEffectW2', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('FactPostPromoEffectW2') },
{ xtype: 'numberfield', name: 'FactPostPromoEffect', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('FactPostPromoEffect') },
{ xtype: 'numberfield', name: 'ActualPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalLSV') },

{ xtype: 'numberfield', name: 'ActualPromoNetLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetLSV') },
{ xtype: 'numberfield', name: 'ActualPromoIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalNSV') },
{ xtype: 'numberfield', name: 'ActualPromoNetIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalNSV') },

{ xtype: 'numberfield', name: 'ActualPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineBaseTI') },
{ xtype: 'numberfield', name: 'ActualPromoBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaseTI') },
{ xtype: 'numberfield', name: 'ActualPromoNetNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetNSV') },

{ xtype: 'numberfield', name: 'ActualPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalMAC') },
{ xtype: 'numberfield', name: 'ActualPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalMAC') },
{ xtype: 'numberfield', name: 'ActualPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalEarnings') },
{ xtype: 'numberfield', name: 'ActualPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalEarnings') },
{ xtype: 'numberfield', name: 'ActualPromoROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoROIPercent') },
{ xtype: 'numberfield', name: 'ActualPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetROIPercent') },
{ xtype: 'numberfield', name: 'ActualPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetUpliftPercent') }
]
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'canceledit',
        hidden: true
    }, {
        text: l10n.ns('core', 'buttons').value('edit'),
        itemId: 'edit',
        hidden: true
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'ok',
        hidden: true
    }]
});
