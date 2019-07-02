Ext.define('App.view.tpm.promoroireport.PromoROIReport', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promoroireport',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoROIReport'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right',
        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            },
        }, '-', {
            itemId: 'extfilterbutton',
            glyph: 0xf349,
            text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
        }, '-', '->', '-', {
            itemId: 'extfilterclearbutton',
            ui: 'blue-button-toolbar',
            disabled: true,
            glyph: 0xf232,
            text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            overCls: '',
            style: {
                'cursor': 'default'
            }
        }]
    }],


    customHeaderItems: [
    ResourceMgr.getAdditionalMenu('core').base = {
        glyph: 0xf068,
        text: l10n.ns('core', 'additionalMenu').value('additionalBtn'),

        menu: {
            xtype: 'customheadermenu',
            items: [{
                glyph: 0xf4eb,
                itemId: 'gridsettings',
                text: l10n.ns('core', 'additionalMenu').value('gridSettingsMenuItem'),
                action: 'SaveGridSettings',
                resource: 'Security'
            }]
        }
    },
    ResourceMgr.getAdditionalMenu('core').import = {
        glyph: 0xf21b,
        text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

        menu: {
            xtype: 'customheadermenu',
            items: [{
                glyph: 0xf21d,
                itemId: 'exportxlsxbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                action: 'ExportPromoROIReportXLSX'
            }]
        }
    }
    ],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promoroireport.PromoROIReport',
            storeId: 'promostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoroireport.PromoROIReport',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [
{ text: l10n.ns('tpm', 'PromoROIReport').value('Number'), dataIndex: 'Number' , format: '0'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('Client1LevelName'), dataIndex: 'Client1LevelName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('Client2LevelName'), dataIndex: 'Client2LevelName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('ClientName'), dataIndex: 'ClientName' },

{ text: l10n.ns('tpm', 'PromoROIReport').value('BrandName'), dataIndex: 'BrandName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('TechnologyName'), dataIndex: 'TechnologyName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('ProductSubrangesList'), dataIndex: 'ProductSubrangesList' },

{ text: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicName'), dataIndex: 'MarsMechanicName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicTypeName'), dataIndex: 'MarsMechanicTypeName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicDiscount'), dataIndex: 'MarsMechanicDiscount' , format: '0'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('MechanicComment'), dataIndex: 'MechanicComment' },

{ xtype: 'datecolumn', text: l10n.ns('tpm', 'PromoROIReport').value('StartDate'), dataIndex: 'StartDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
{ xtype: 'datecolumn', text: l10n.ns('tpm', 'PromoROIReport').value('EndDate'), dataIndex: 'EndDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
{ text: l10n.ns('tpm', 'PromoROIReport').value('PromoDuration'), dataIndex: 'PromoDuration' , format: '0'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('DispatchDuration'), dataIndex: 'DispatchDuration' , format: '0'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('EventName'), dataIndex: 'EventName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('PromoStatusName'), dataIndex: 'PromoStatusName' },

{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicName'), dataIndex: 'PlanInstoreMechanicName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicTypeName'), dataIndex: 'PlanInstoreMechanicTypeName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicDiscount'), dataIndex: 'PlanInstoreMechanicDiscount' , format: '0'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineLSV'), dataIndex: 'PlanPromoBaselineLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalLSV'), dataIndex: 'PlanPromoIncrementalLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoLSV'), dataIndex: 'PlanPromoLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoUpliftPercent'), dataIndex: 'PlanPromoUpliftPercent' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTIShopper'), dataIndex: 'PlanPromoTIShopper' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTIMarketing'), dataIndex: 'PlanPromoTIMarketing' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoXSites'), dataIndex: 'PlanPromoXSites' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCatalogue'), dataIndex: 'PlanPromoCatalogue' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPOSMInClient'), dataIndex: 'PlanPromoPOSMInClient' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBranding'), dataIndex: 'PlanPromoBranding' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBTL'), dataIndex: 'PlanPromoBTL' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProduction'), dataIndex: 'PlanPromoCostProduction' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdXSites'), dataIndex: 'PlanPromoCostProdXSites' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdCatalogue'), dataIndex: 'PlanPromoCostProdCatalogue' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdPOSMInClient'), dataIndex: 'PlanPromoCostProdPOSMInClient' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCost'), dataIndex: 'PlanPromoCost' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalBaseTI'), dataIndex: 'PlanPromoIncrementalBaseTI' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalCOGS'), dataIndex: 'PlanPromoIncrementalCOGS' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTotalCost'), dataIndex: 'PlanPromoTotalCost' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSVW1'), dataIndex: 'PlanPromoPostPromoEffectLSVW1' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSVW2'), dataIndex: 'PlanPromoPostPromoEffectLSVW2' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSV'), dataIndex: 'PlanPromoPostPromoEffectLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalLSV'), dataIndex: 'PlanPromoNetIncrementalLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetLSV'), dataIndex: 'PlanPromoNetLSV' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineBaseTI'), dataIndex: 'PlanPromoBaselineBaseTI' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaseTI'), dataIndex: 'PlanPromoBaseTI' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetNSV'), dataIndex: 'PlanPromoNetNSV' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalNSV'), dataIndex: 'PlanPromoIncrementalNSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalNSV'), dataIndex: 'PlanPromoNetIncrementalNSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalMAC'), dataIndex: 'PlanPromoIncrementalMAC' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalMAC'), dataIndex: 'PlanPromoNetIncrementalMAC' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalEarnings'), dataIndex: 'PlanPromoIncrementalEarnings' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalEarnings'), dataIndex: 'PlanPromoNetIncrementalEarnings' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoROIPercent'), dataIndex: 'PlanPromoROIPercent', format: '0' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetROIPercent'), dataIndex: 'PlanPromoNetROIPercent', format: '0' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetUpliftPercent'), dataIndex: 'PlanPromoNetUpliftPercent' , format: '0'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreMechanicName'), dataIndex: 'ActualInStoreMechanicName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreMechanicTypeName'), dataIndex: 'ActualInStoreMechanicTypeName' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreDiscount'), dataIndex: 'ActualInStoreDiscount' , format: '0'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreShelfPrice'), dataIndex: 'ActualInStoreShelfPrice' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('PlanInStoreShelfPrice'), dataIndex: 'PlanInStoreShelfPrice' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('InvoiceNumber'), dataIndex: 'InvoiceNumber' },
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineLSV'), dataIndex: 'ActualPromoBaselineLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalLSV'), dataIndex: 'ActualPromoIncrementalLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVByCompensation'), dataIndex: 'ActualPromoLSVByCompensation' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSV'), dataIndex: 'ActualPromoLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoUpliftPercent'), dataIndex: 'ActualPromoUpliftPercent' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIShopper'), dataIndex: 'ActualPromoTIShopper' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIMarketing'), dataIndex: 'ActualPromoTIMarketing' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoXSites'), dataIndex: 'ActualPromoXSites' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCatalogue'), dataIndex: 'ActualPromoCatalogue' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPOSMInClient'), dataIndex: 'ActualPromoPOSMInClient' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBranding'), dataIndex: 'ActualPromoBranding' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBTL'), dataIndex: 'ActualPromoBTL' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProduction'), dataIndex: 'ActualPromoCostProduction' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdXSites'), dataIndex: 'ActualPromoCostProdXSites' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdCatalogue'), dataIndex: 'ActualPromoCostProdCatalogue' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdPOSMInClient'), dataIndex: 'ActualPromoCostProdPOSMInClient' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCost'), dataIndex: 'ActualPromoCost' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalBaseTI'), dataIndex: 'ActualPromoIncrementalBaseTI' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalCOGS'), dataIndex: 'ActualPromoIncrementalCOGS' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTotalCost'), dataIndex: 'ActualPromoTotalCost' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSVW1'), dataIndex: 'ActualPromoPostPromoEffectLSVW1' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSVW2'), dataIndex: 'ActualPromoPostPromoEffectLSVW2' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSV'), dataIndex: 'ActualPromoPostPromoEffectLSV' , format: '0'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalLSV'), dataIndex: 'ActualPromoNetIncrementalLSV' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetLSV'), dataIndex: 'ActualPromoNetLSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalNSV'), dataIndex: 'ActualPromoIncrementalNSV' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalNSV'), dataIndex: 'ActualPromoNetIncrementalNSV' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineBaseTI'), dataIndex: 'ActualPromoBaselineBaseTI' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaseTI'), dataIndex: 'ActualPromoBaseTI' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetNSV'), dataIndex: 'ActualPromoNetNSV' , format: '0.00'},

{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalMAC'), dataIndex: 'ActualPromoIncrementalMAC' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalMAC'), dataIndex: 'ActualPromoNetIncrementalMAC' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalEarnings'), dataIndex: 'ActualPromoIncrementalEarnings' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalEarnings'), dataIndex: 'ActualPromoNetIncrementalEarnings' , format: '0.00'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoROIPercent'), dataIndex: 'ActualPromoROIPercent' , format: '0'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetROIPercent'), dataIndex: 'ActualPromoNetROIPercent' , format: '0'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetUpliftPercent'), dataIndex: 'ActualPromoNetUpliftPercent' , format: '0'},
{ text: l10n.ns('tpm', 'PromoROIReport').value('InOut'), dataIndex: 'InOut' , format: '0'},
]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promoroireport.PromoROIReport',
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
{ xtype: 'numberfield', name: 'PlanPromoPostPromoEffectLSVW1', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSVW1') },
{ xtype: 'numberfield', name: 'PlanPromoPostPromoEffectLSVW2', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSVW2') },
{ xtype: 'numberfield', name: 'PlanPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSV') },
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
{ xtype: 'numberfield', name: 'ActualInStoreDiscount', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreDiscount') },
{ xtype: 'numberfield', name: 'ActualInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreShelfPrice') },
{ xtype: 'numberfield', name: 'PlanInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInStoreShelfPrice') },
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

{ xtype: 'numberfield', name: 'ActualPromoPostPromoEffectLSVW1', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSVW1') },
{ xtype: 'numberfield', name: 'ActualPromoPostPromoEffectLSVW2', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSVW2') },
{ xtype: 'numberfield', name: 'ActualPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSV') },
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
{ xtype: 'numberfield', name: 'ActualPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetUpliftPercent') },
{ xtype: 'numberfield', name: 'InOut', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('InOut') }
        ]
    }]
});
