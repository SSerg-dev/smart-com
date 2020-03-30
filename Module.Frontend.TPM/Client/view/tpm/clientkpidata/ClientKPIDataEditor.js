Ext.define('App.view.tpm.clientkpidata.ClientKPIDataEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.clientkpidataeditor',
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields clientkpidataeditor',

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ObjectId',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientHierarchy',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ClientHierarchy')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Year',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('Year')
        }, {
            xtype: 'numberfield',
            name: 'ShopperTiPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ShopperTiPlanPercent'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ShopperTiPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ShopperTiPlan')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ShopperTiYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ShopperTiYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ShopperTiYTDPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ShopperTiYTDPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ShopperTiYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ShopperTiYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ShopperTiYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ShopperTiYEEPercent')
        }, {
            xtype: 'numberfield',
            name: 'MarketingTiPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiPlanPercent'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketingTiPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiPlan')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketingTiYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketingTiYTDPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiYTDPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketingTiYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketingTiYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductionPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionPlanPercent')
        }, {
            xtype: 'numberfield',
            name: 'ProductionPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionPlan'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductionYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductionYTDPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionYTDPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductionYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductionYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandingPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandingPlanPercent')
        }, {
            xtype: 'numberfield',
            name: 'BrandingPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandingPlan'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandingYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandingYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandingYTDPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandingYTDPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandingYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandingYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandingYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandingYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLPlanPercent')
        }, {
            xtype: 'numberfield',
            name: 'BTLPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLPlan'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYTDPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYTDPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEEPercent')
        }, {
            xtype: 'numberfield',
            name: 'ROIPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ROIPlanPercent'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ROIYTDPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ROIYTDPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ROIYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ROIYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'LSVPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('LSVPlan')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'LSVYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('LSVYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'LSVYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('LSVYEE')
        }, {
            xtype: 'numberfield',
            name: 'IncrementalNSVPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('IncrementalNSVPlan'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalNSVYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('IncrementalNSVYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalNSVYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('IncrementalNSVYEE')
        }, {
            xtype: 'numberfield',
            name: 'PromoNSVPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoNSVPlan'),
            readOnly: true,
            readOnlyCls: 'readOnlyField'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoNSVYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoNSVYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoNSVYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoNSVYEE')
        }]
    },

    afterWindowShow: function () {
    },
});     