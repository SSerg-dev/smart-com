﻿Ext.define('App.view.tpm.promoroireport.PromoROIReportEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promoroireporteditor',
    width: 1000,
    minWidth: 1000,
    maxHeight: 500,
    cls: 'readOnlyFields',

    noChange: true,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
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
            { xtype: 'textfield', name: 'EventName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('EventName') },
            { xtype: 'textfield', name: 'PromoStatusName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PromoStatusName') },
            {
                xtype: 'textfield',
                name: 'InOut',
                fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('InOut'),
                listeners: {
                    afterrender: function (value) {
                        this.setValue(value.rawValue === 'true' ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false'));
                    }
                }
            },
            {
                xtype: 'textfield',
                name: 'IsGrowthAcceleration',
                fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('IsGrowthAcceleration'),
                listeners: {
                    afterrender: function (value) {
                        this.setValue(value.rawValue === 'true' ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false'));
                    }
                }
            },
            { xtype: 'textfield', name: 'PlanInstoreMechanicName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicName') },
            { xtype: 'textfield', name: 'PlanInstoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicTypeName') },
            { xtype: 'numberfield', name: 'PlanInstoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicDiscount') },
            { xtype: 'numberfield', name: 'PlanInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInStoreShelfPrice') },
            { xtype: 'numberfield', name: 'PCPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PCPrice') },
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
            { xtype: 'numberfield', name: 'TIBasePercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('TIBasePercent') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalBaseTI') },
            { xtype: 'numberfield', name: 'COGSPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('COGSPercent') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalCOGS') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalCOGS') },
            { xtype: 'numberfield', name: 'PlanPromoTotalCost', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTotalCost') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetLSV') },
            { xtype: 'numberfield', name: 'PlanPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNetBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNSV') },
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
            { xtype: 'numberfield', name: 'ActualInStoreDiscount', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreDiscount') },
            { xtype: 'numberfield', name: 'ActualInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreShelfPrice') },
            { xtype: 'textfield', name: 'InvoiceNumber', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('InvoiceNumber') },
            { xtype: 'numberfield', name: 'ActualPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalLSV') },
            { xtype: 'numberfield', name: 'ActualPromoLSVByCompensation', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVByCompensation') },
            { xtype: 'numberfield', name: 'ActualPromoLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSV') },
            { xtype: 'numberfield', name: 'ActualPromoUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoUpliftPercent') },
            { xtype: 'numberfield', name: 'ActualPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetUpliftPercent') },
            { xtype: 'numberfield', name: 'ActualPromoTIShopper', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIShopper') },
            { xtype: 'numberfield', name: 'ActualPromoTIMarketing', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIMarketing') },
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
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalCOGS') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalCOGS') },
            { xtype: 'numberfield', name: 'ActualPromoTotalCost', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTotalCost') },
            { xtype: 'numberfield', name: 'ActualPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalNSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalNSV') },
            { xtype: 'numberfield', name: 'ActualPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoNetBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetNSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalMAC') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalMAC') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalEarnings') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalEarnings') },
            { xtype: 'numberfield', name: 'ActualPromoROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoROIPercent') },
            { xtype: 'numberfield', name: 'ActualPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetROIPercent') },
            { xtype: 'textfield', name: 'PromoTypesName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PromoTypesName') },

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
