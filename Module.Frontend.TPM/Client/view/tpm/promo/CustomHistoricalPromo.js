Ext.define('App.view.tpm.promo.CustomHistoricalPromo', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.customhistoricalpromo',

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    dockedItems: [{
        xtype: 'editorform',
        cls: 'hierarchydetailform',
        columnsCount: 1,
        dock: 'right',
        width: 400,
        layout: 'fit',

        items: [{
            xtype: 'custompromopanel',
            margin: 2,
            overflowY: 'auto',
            height: 480,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'fieldset',
                title: 'User info',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                },
                defaults: {
                    padding: '0 3 0 3',
                },
                items: [{
                    xtype: 'singlelinedisplayfield',
                    name: '_User',
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_User')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: '_Role',
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_Role')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: '_EditDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_EditDate')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromo', 'OperationType'),
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_Operation')
                }]
            }, {
                xtype: 'fieldset',
                title: 'Change info',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                },
                defaults: {
                    padding: '0 3 0 3',
                    labelWidth: 200
                },
                items: [
                    { xtype: 'singlelinedisplayfield', name: 'PromoStatusName', fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ClientHierarchy', fieldLabel: l10n.ns('tpm', 'Promo').value('ClientHierarchy'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MarsMechanicName', fieldLabel: l10n.ns('tpm', 'Promo').value('MarsMechanicName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MarsMechanicTypeName', fieldLabel: l10n.ns('tpm', 'Promo').value('MarsMechanicTypeName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MarsMechanicDiscount', fieldLabel: l10n.ns('tpm', 'Promo').value('MarsMechanicDiscount'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanInstoreMechanicName', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanInstoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicTypeName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanInstoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicDiscount'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MechanicComment', fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicComment'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'StartDate', fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'), renderer: Ext.util.Format.dateRenderer('d.m.Y'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'EndDate', fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'), renderer: Ext.util.Format.dateRenderer('d.m.Y'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'DispatchesStart', fieldLabel: l10n.ns('tpm', 'Promo').value('DispatchesStart'), renderer: Ext.util.Format.dateRenderer('d.m.Y'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'DispatchesEnd', fieldLabel: l10n.ns('tpm', 'Promo').value('DispatchesEnd'), renderer: Ext.util.Format.dateRenderer('d.m.Y'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'EventName', fieldLabel: l10n.ns('tpm', 'Promo').value('EventName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PromoEventName', fieldLabel: l10n.ns('tpm', 'Promo').value('PromoEventName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PromoEventDescription', fieldLabel: l10n.ns('tpm', 'Promo').value('PromoEventDescription'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'CalendarPriority', fieldLabel: l10n.ns('tpm', 'Promo').value('CalendarPriority'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoTIShopper', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIShopper'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoTIMarketing', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIMarketing'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoBranding', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBranding'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoCost', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCost'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoBTL', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBTL'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoCostProduction', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProduction'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoUpliftPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoPostPromoEffectLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoROIPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoROIPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetROIPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoIncrementalNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetIncrementalNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalCOGS'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalMAC'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoTIShopper', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIShopper'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoTIMarketing', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIMarketing'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoBranding', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBranding'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoBTL', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBTL'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoCostProduction', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCostProduction'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoCost', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCost'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoUpliftPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoUpliftPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoLSVSO', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSVSO'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoLSVByCompensation', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSVByCompensation'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoROIPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoROIPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoIncrementalNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetIncrementalNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetIncrementalNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetIncrementalBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetIncrementalCOGS'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalMAC'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ProductHierarchy', fieldLabel: l10n.ns('tpm', 'Promo').value('ProductHierarchy'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'InOut', fieldLabel: l10n.ns('tpm', 'Promo').value('InOut'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'Name', fieldLabel: l10n.ns('tpm', 'Promo').value('Name'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'BrandName', fieldLabel: l10n.ns('tpm', 'Promo').value('BrandName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'BrandTechName', fieldLabel: l10n.ns('tpm', 'Promo').value('BrandTechName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'Mechanic', fieldLabel: l10n.ns('tpm', 'Promo').value('Mechanic'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MechanicIA', fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicIA'), hidden: true },
                    //{ xtype: 'singlelinedisplayfield', name: 'ColorSystemName', fieldLabel: l10n.ns('tpm', 'Promo').value('ColorSystemName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ColorDisplayName', fieldLabel: l10n.ns('tpm', 'Promo').value('ColorDisplayName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'InvoiceNumber', fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceNumber'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'InvoiceTotal', fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceTotal'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'IsOnInvoice', fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceType'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'DocumentNumber', fieldLabel: l10n.ns('tpm', 'Promo').value('DocumentNumber'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'NeedRecountUplift', fieldLabel: l10n.ns('tpm', 'Promo').value('NeedRecountUplift'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoXSites', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoXSites'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoCatalogue', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCatalogue'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoPOSMInClient', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoPOSMInClient'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoCostProdXSites', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProdXSites'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoCostProdCatalogue', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProdCatalogue'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoCostProdPOSMInClient', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProdPOSMInClient'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoXSites', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoXSites'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoCatalogue', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCatalogue'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoPOSMInClient', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoPOSMInClient'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoCostProdXSites', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCostProdXSites'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoCostProdCatalogue', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCostProdCatalogue'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoCostProdPOSMInClient', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCostProdPOSMInClient'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBaselineLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalCOGS'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoTotalCost', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTotalCost'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalMAC'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalEarnings'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalEarnings'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetUpliftPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBaselineLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualInStoreDiscount', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreDiscount'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreShelfPrice'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInStoreShelfPrice'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalCOGS'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoTotalCost', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTotalCost'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetIncrementalLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetIncrementalMAC'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalEarnings'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetIncrementalEarnings'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetROIPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetUpliftPercent'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBaselineBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoNetNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBaselineBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoBaseTI', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBaseTI'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'IsAutomaticallyApproved', fieldLabel: l10n.ns('tpm', 'Promo').value('IsAutomaticallyApproved'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'IsCMManagerApproved', fieldLabel: l10n.ns('tpm', 'Promo').value('IsCMManagerApproved'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'IsDemandPlanningApproved', fieldLabel: l10n.ns('tpm', 'Promo').value('IsDemandPlanningApproved'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'IsDemandFinanceApproved', fieldLabel: l10n.ns('tpm', 'Promo').value('IsDemandFinanceApproved'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'OtherEventName', fieldLabel: l10n.ns('tpm', 'Promo').value('OtherEventName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoPostPromoEffectLSVW1', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoPostPromoEffectLSVW1'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanPromoPostPromoEffectLSVW2', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoPostPromoEffectLSVW2'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoPostPromoEffectLSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoPostPromoEffectLSVW1', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoPostPromoEffectLSVW1'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoPostPromoEffectLSVW2', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoPostPromoEffectLSVW2'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PromoStatusColor', fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusColor'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'RejectReasonName', fieldLabel: l10n.ns('tpm', 'Promo').value('RejectReasonName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualInStoreMechanicName', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualInStoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicTypeName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ActualPromoNSV', fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNSV'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'LastChangedDate', fieldLabel: l10n.ns('tpm', 'Promo').value('LastChangedDate'), hidden: true, renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'LastChangedDateDemand', fieldLabel: l10n.ns('tpm', 'Promo').value('LastChangedDateDemand'), hidden: true, renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'LastChangedDateFinance', fieldLabel: l10n.ns('tpm', 'Promo').value('LastChangedDateFinance'), hidden: true, renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PromoDuration', fieldLabel: l10n.ns('tpm', 'Promo').value('PromoDuration'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'DispatchDuration', fieldLabel: l10n.ns('tpm', 'Promo').value('DispatchDuration'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'Number', fieldLabel: l10n.ns('tpm', 'Promo').value('Number'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'Comment', fieldLabel: l10n.ns('tpm', 'Promo').value('Comment'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ProductZREP', fieldLabel: l10n.ns('tpm', 'Promo').value('ProductZREP'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'InstoreMechanicName', fieldLabel: l10n.ns('tpm', 'Promo').value('InstoreMechanicName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'InstoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'Promo').value('InstoreMechanicTypeName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'InstoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'Promo').value('InstoreMechanicDiscount'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'Priority', fieldLabel: l10n.ns('tpm', 'Promo').value('Priority'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ProductSubrangesList', fieldLabel: l10n.ns('tpm', 'Promo').value('ProductSubrangesList'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'IsGrowthAcceleration', fieldLabel: l10n.ns('tpm', 'Promo').value('IsGrowthAcceleration'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'DeviationCoefficient', fieldLabel: l10n.ns('tpm', 'Promo').value('Adjustment'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'IsApolloExport', fieldLabel: l10n.ns('tpm', 'Promo').value('IsApolloExport'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'CreatorLogin', fieldLabel: l10n.ns('tpm', 'Promo').value('CreatorLogin'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'InvoiceType', fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceType'), hidden: true }
                ]
            }]
        }]
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        flex: 3,

        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promo.HistoricalPromo',
            storeId: 'customhistoricalpromostore',
            autoLoad: false,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promo.HistoricalPromo',
                    modelId: 'efselectionmodel'
                }]
            },
            sorters: [{
                property: '_EditDate',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 2,
                minWidth: 100
            },

            items: [{
                text: l10n.ns('tpm', 'HistoricalPromo').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPromo').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPromo').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalPromo').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromo', 'OperationType'),
                filter: {
                    type: 'combo',
                    valueField: 'id',
                    store: {
                        type: 'operationtypestore'
                    },
                    operator: 'eq'
                }
            }]
        },

        listeners: {
            select: function (cell, record) {
                var form = this.up().down('editorform');
                var fields = form.getForm().getFields();

                fields.each(function (item, index) {
                    var changedValue = record.get(item.name);
                    if (changedValue != null) {
                        item.setValue(changedValue);
                        item.show();
                    } else {
                        item.hide();
                    }
                });
            }
        }
    }]
});
