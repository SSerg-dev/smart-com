﻿Ext.define('App.view.tpm.promoroireport.PromoROIReport', {
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
                    itemId: 'exportbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                    action: 'ExportXLSX'
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
                {   
                    text: l10n.ns('tpm', 'PromoROIReport').value('TPMmode'),
                    dataIndex: 'TPMmode',
                    filter: {
                        type: 'combo',
                        valueField: 'id',
                        displayField: 'alias',
                        store: {
                            type: 'modestore'
                        },
                        operator: 'eq'
                    }
                },
                { text: l10n.ns('tpm', 'PromoROIReport').value('Number'), dataIndex: 'Number', format: '0' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('Client1LevelName'), dataIndex: 'Client1LevelName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('Client2LevelName'), dataIndex: 'Client2LevelName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('ClientName'), dataIndex: 'ClientName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('BrandName'), dataIndex: 'BrandName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('TechnologyName'), dataIndex: 'TechnologyName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('SubName'), dataIndex: 'SubName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('ProductSubrangesList'), dataIndex: 'ProductSubrangesList' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicName'), dataIndex: 'MarsMechanicName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicTypeName'), dataIndex: 'MarsMechanicTypeName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('MarsMechanicDiscount'), dataIndex: 'MarsMechanicDiscount', format: '0' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('MechanicComment'), dataIndex: 'MechanicComment' },
                { xtype: 'datecolumn', text: l10n.ns('tpm', 'PromoROIReport').value('StartDate'), dataIndex: 'StartDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
                {
                    text: l10n.ns('tpm', 'PromoROIReport').value('MarsStartDate'),
                    dataIndex: 'MarsStartDate',
                    width: 150,
                    filter: {
                        xtype: 'marsdatefield',
                        operator: 'like',
                        validator: function (value) {
                            // дает возможность фильтровать только по году
                            return true;
                        },
                    }
                },
                { xtype: 'datecolumn', text: l10n.ns('tpm', 'PromoROIReport').value('EndDate'), dataIndex: 'EndDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
                {
                    text: l10n.ns('tpm', 'PromoROIReport').value('MarsEndDate'),
                    dataIndex: 'MarsEndDate',
                    width: 125,
                    filter: {
                        xtype: 'marsdatefield',
                        operator: 'like'
                    }
                },
                { text: l10n.ns('tpm', 'PromoROIReport').value('BudgetYear'), dataIndex: 'BudgetYear' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('PromoDuration'), dataIndex: 'PromoDuration', format: '0' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('EventName'), dataIndex: 'EventName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('PromoStatusName'), dataIndex: 'PromoStatusName' },
                {
                    text: l10n.ns('tpm', 'PromoROIReport').value('InOut'),
                    dataIndex: 'InOut',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                {
                    text: l10n.ns('tpm', 'PromoROIReport').value('IsGrowthAcceleration'),
                    dataIndex: 'IsGrowthAcceleration',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                { text: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicName'), dataIndex: 'PlanInstoreMechanicName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicTypeName'), dataIndex: 'PlanInstoreMechanicTypeName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicDiscount'), dataIndex: 'PlanInstoreMechanicDiscount', format: '0' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('PlanInStoreShelfPrice'), dataIndex: 'PlanInStoreShelfPrice', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PCPrice'), dataIndex: 'PCPrice', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoVolume'), dataIndex: 'PlanPromoVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineVolume'), dataIndex: 'PlanPromoBaselineVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalVolume'), dataIndex: 'PlanPromoIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalVolume'), dataIndex: 'PlanPromoNetIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectVolume'), dataIndex: 'PlanPromoPostPromoEffectVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectVolumeW1'), dataIndex: 'PlanPromoPostPromoEffectVolumeW1', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectVolumeW2'), dataIndex: 'PlanPromoPostPromoEffectVolumeW2', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineLSV'), dataIndex: 'PlanPromoBaselineLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalLSV'), dataIndex: 'PlanPromoIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoLSV'), dataIndex: 'PlanPromoLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoUpliftPercent'), dataIndex: 'PlanPromoUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTIShopper'), dataIndex: 'PlanPromoTIShopper', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTIMarketing'), dataIndex: 'PlanPromoTIMarketing', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoXSites'), dataIndex: 'PlanPromoXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCatalogue'), dataIndex: 'PlanPromoCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPOSMInClient'), dataIndex: 'PlanPromoPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBranding'), dataIndex: 'PlanPromoBranding', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBTL'), dataIndex: 'PlanPromoBTL', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProduction'), dataIndex: 'PlanPromoCostProduction', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdXSites'), dataIndex: 'PlanPromoCostProdXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdCatalogue'), dataIndex: 'PlanPromoCostProdCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCostProdPOSMInClient'), dataIndex: 'PlanPromoCostProdPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoCost'), dataIndex: 'PlanPromoCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('TIBasePercent'), dataIndex: 'TIBasePercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalBaseTI'), dataIndex: 'PlanPromoIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalBaseTI'), dataIndex: 'PlanPromoNetIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('COGSPercent'), dataIndex: 'COGSPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('COGSTn'), dataIndex: 'COGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalCOGS'), dataIndex: 'PlanPromoIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalCOGS'), dataIndex: 'PlanPromoNetIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalCOGSTn'), dataIndex: 'PlanPromoIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalCOGSTn'), dataIndex: 'PlanPromoNetIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTotalCost'), dataIndex: 'PlanPromoTotalCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSV'), dataIndex: 'PlanPromoPostPromoEffectLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalLSV'), dataIndex: 'PlanPromoNetIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetLSV'), dataIndex: 'PlanPromoNetLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineBaseTI'), dataIndex: 'PlanPromoBaselineBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaseTI'), dataIndex: 'PlanPromoBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetBaseTI'), dataIndex: 'PlanPromoNetBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNSVtn'), dataIndex: 'PlanPromoNSVtn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNSV'), dataIndex: 'PlanPromoNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetNSV'), dataIndex: 'PlanPromoNetNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalNSV'), dataIndex: 'PlanPromoIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalNSV'), dataIndex: 'PlanPromoNetIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalMAC'), dataIndex: 'PlanPromoIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalMAC'), dataIndex: 'PlanPromoNetIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalEarnings'), dataIndex: 'PlanPromoIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalEarnings'), dataIndex: 'PlanPromoNetIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoROIPercent'), dataIndex: 'PlanPromoROIPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetROIPercent'), dataIndex: 'PlanPromoNetROIPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetUpliftPercent'), dataIndex: 'PlanPromoNetUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanAddTIMarketingApproved'), dataIndex: 'PlanAddTIMarketingApproved', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanAddTIShopperCalculated'), dataIndex: 'PlanAddTIShopperCalculated', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanAddTIShopperApproved'), dataIndex: 'PlanAddTIShopperApproved', format: '0.00' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreMechanicName'), dataIndex: 'ActualInStoreMechanicName' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreMechanicTypeName'), dataIndex: 'ActualInStoreMechanicTypeName' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreDiscount'), dataIndex: 'ActualInStoreDiscount', format: '0' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualInStoreShelfPrice'), dataIndex: 'ActualInStoreShelfPrice', format: '0.00' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('InvoiceNumber'), dataIndex: 'InvoiceNumber' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoVolume'), dataIndex: 'ActualPromoVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineVolume'), dataIndex: 'ActualPromoBaselineVolume', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalVolume'), dataIndex: 'ActualPromoIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalVolume'), dataIndex: 'ActualPromoNetIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectVolume'), dataIndex: 'ActualPromoPostPromoEffectVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoVolumeByCompensation'), dataIndex: 'ActualPromoVolumeByCompensation', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoVolumeSI'), dataIndex: 'ActualPromoVolumeSI', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineLSV'), dataIndex: 'ActualPromoBaselineLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalLSV'), dataIndex: 'ActualPromoIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVByCompensation'), dataIndex: 'ActualPromoLSVByCompensation', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSV'), dataIndex: 'ActualPromoLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVSI'), dataIndex: 'ActualPromoLSVSI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVSO'), dataIndex: 'ActualPromoLSVSO', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoUpliftPercent'), dataIndex: 'ActualPromoUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetUpliftPercent'), dataIndex: 'ActualPromoNetUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIShopper'), dataIndex: 'ActualPromoTIShopper', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTIMarketing'), dataIndex: 'ActualPromoTIMarketing', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoXSites'), dataIndex: 'ActualPromoXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCatalogue'), dataIndex: 'ActualPromoCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPOSMInClient'), dataIndex: 'ActualPromoPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBranding'), dataIndex: 'ActualPromoBranding', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBTL'), dataIndex: 'ActualPromoBTL', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProduction'), dataIndex: 'ActualPromoCostProduction', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdXSites'), dataIndex: 'ActualPromoCostProdXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdCatalogue'), dataIndex: 'ActualPromoCostProdCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCostProdPOSMInClient'), dataIndex: 'ActualPromoCostProdPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoCost'), dataIndex: 'ActualPromoCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalBaseTI'), dataIndex: 'ActualPromoIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalBaseTI'), dataIndex: 'ActualPromoNetIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalCOGS'), dataIndex: 'ActualPromoIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalCOGS'), dataIndex: 'ActualPromoNetIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalCOGSTn'), dataIndex: 'ActualPromoIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalCOGSTn'), dataIndex: 'ActualPromoNetIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoTotalCost'), dataIndex: 'ActualPromoTotalCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectLSV'), dataIndex: 'ActualPromoPostPromoEffectLSV', format: '0' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalLSV'), dataIndex: 'ActualPromoNetIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetLSV'), dataIndex: 'ActualPromoNetLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalNSV'), dataIndex: 'ActualPromoIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalNSV'), dataIndex: 'ActualPromoNetIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineBaseTI'), dataIndex: 'ActualPromoBaselineBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaseTI'), dataIndex: 'ActualPromoBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetBaseTI'), dataIndex: 'ActualPromoNetBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNSVtn'), dataIndex: 'ActualPromoNSVtn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNSV'), dataIndex: 'ActualPromoNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetNSV'), dataIndex: 'ActualPromoNetNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalMAC'), dataIndex: 'ActualPromoIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalMAC'), dataIndex: 'ActualPromoNetIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalEarnings'), dataIndex: 'ActualPromoIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalEarnings'), dataIndex: 'ActualPromoNetIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoROIPercent'), dataIndex: 'ActualPromoROIPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetROIPercent'), dataIndex: 'ActualPromoNetROIPercent', format: '0.00' },
                //Add TI
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualAddTIMarketing'), dataIndex: 'ActualAddTIMarketing', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualAddTIShopper'), dataIndex: 'ActualAddTIShopper', format: '0.00' },
                { text: l10n.ns('tpm', 'PromoROIReport').value('PromoTypesName'), dataIndex: 'PromoTypesName' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('SumInvoice'), dataIndex: 'SumInvoice', format: '0.00' },
                //New calculation parameters for ROI
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalMACLSV'), dataIndex: 'PlanPromoIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalMACLSV'), dataIndex: 'PlanPromoNetIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalMACLSV'), dataIndex: 'ActualPromoIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalMACLSV'), dataIndex: 'ActualPromoNetIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalEarningsLSV'), dataIndex: 'PlanPromoIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalEarningsLSV'), dataIndex: 'PlanPromoNetIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalEarningsLSV'), dataIndex: 'ActualPromoIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalEarningsLSV'), dataIndex: 'ActualPromoNetIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoROIPercentLSV'), dataIndex: 'PlanPromoROIPercentLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetROIPercentLSV'), dataIndex: 'PlanPromoNetROIPercentLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoROIPercentLSV'), dataIndex: 'ActualPromoROIPercentLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetROIPercentLSV'), dataIndex: 'ActualPromoNetROIPercentLSV', format: '0.00' },
                {
                    text: l10n.ns('tpm', 'PromoROIReport').value('IsApolloExport'),
                    dataIndex: 'IsApolloExport',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                {
                    text: l10n.ns('tpm', 'PromoROIReport').value('MLmodel'),
                    dataIndex: 'MLmodel',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
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
            { xtype: 'textfield', name: 'SubName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('SubName') },
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
            { xtype: 'numberfield', name: 'InOut', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('InOut') },
            { xtype: 'numberfield', name: 'IsGrowthAcceleration', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('IsGrowthAcceleration') },
            { xtype: 'textfield', name: 'PlanInstoreMechanicName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicName') },
            { xtype: 'textfield', name: 'PlanInstoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicTypeName') },
            { xtype: 'numberfield', name: 'PlanInstoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInstoreMechanicDiscount') },
            { xtype: 'numberfield', name: 'PlanInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanInStoreShelfPrice') },
            { xtype: 'numberfield', name: 'PCPrice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PCPrice') },
            { xtype: 'numberfield', name: 'PlanPromoVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoVolume') },
            { xtype: 'numberfield', name: 'PlanPromoBaselineVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineVolume') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalVolume') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalVolume') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectVolume') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectVolumeW1', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectVolumeW1') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectVolumeW2', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectVolumeW2') },

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
            { xtype: 'numberfield', name: 'COGSTn', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('COGSTn') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalCOGS') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalCOGS') },
            { xtype: 'numberfield', name: 'PlanPromoTotalCost', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoTotalCost') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoPostPromoEffectLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetLSV') },
            { xtype: 'numberfield', name: 'PlanPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaselineBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNetBaseTI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNSVtn', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNSVtn') },
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
            { xtype: 'numberfield', name: 'ActualPromoVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoVolume') },
            { xtype: 'numberfield', name: 'ActualPromoBaselineVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineVolume') },

            { xtype: 'numberfield', name: 'ActualPromoIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalVolume') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalVolume') },
            { xtype: 'numberfield', name: 'ActualPromoPostPromoEffectVolume', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoPostPromoEffectVolume') },
            { xtype: 'numberfield', name: 'ActualPromoVolumeByCompensation', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoVolumeByCompensation') },
            { xtype: 'numberfield', name: 'ActualPromoVolumeSI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoVolumeSI') },

            { xtype: 'numberfield', name: 'ActualPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoBaselineLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalLSV') },
            { xtype: 'numberfield', name: 'ActualPromoLSVByCompensation', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVByCompensation') },
            { xtype: 'numberfield', name: 'ActualPromoLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSV') },
            { xtype: 'numberfield', name: 'ActualPromoLSVSI', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVSI') },
            { xtype: 'numberfield', name: 'ActualPromoLSVSO', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoLSVSO') },

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
            { xtype: 'numberfield', name: 'ActualPromoNSVtn', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNSVtn') },
            { xtype: 'numberfield', name: 'ActualPromoNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetNSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetNSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalMAC') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalMAC') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalEarnings') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalEarnings') },
            { xtype: 'numberfield', name: 'ActualPromoROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoROIPercent') },
            { xtype: 'numberfield', name: 'ActualPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetROIPercent') },
            { xtype: 'textfield', name: 'PromoTypesName', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PromoTypesName') },
            { xtype: 'numberfield', name: 'SumInvoice', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('SumInvoice') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'PlanPromoROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoROIPercentLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('PlanPromoNetROIPercentLSV') },
            { xtype: 'numberfield', name: 'ActualPromoROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoROIPercentLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoROIReport').value('ActualPromoNetROIPercentLSV') },
        ]
    }]
});
