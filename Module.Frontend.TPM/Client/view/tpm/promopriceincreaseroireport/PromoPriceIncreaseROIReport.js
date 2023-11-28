Ext.define('App.view.tpm.promopriceincreaseroireport.PromoPriceIncreaseROIReport', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promopriceincreaseroireport',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoPriceIncreaseROIReport'),

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
            model: 'App.model.tpm.promopriceincreaseroireport.PromoPriceIncreaseROIReport',
            storeId: 'promopriceincreasestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promopriceincreaseroireport.PromoPriceIncreaseROIReport',
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
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('TPMmode'),
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
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('Number'), dataIndex: 'Number', format: '0' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('Client1LevelName'), dataIndex: 'Client1LevelName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('Client2LevelName'), dataIndex: 'Client2LevelName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ClientName'), dataIndex: 'ClientName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('BrandName'), dataIndex: 'BrandName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('TechnologyName'), dataIndex: 'TechnologyName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('SubName'), dataIndex: 'SubName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ProductSubrangesList'), dataIndex: 'ProductSubrangesList' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsMechanicName'), dataIndex: 'MarsMechanicName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsMechanicTypeName'), dataIndex: 'MarsMechanicTypeName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsMechanicDiscount'), dataIndex: 'MarsMechanicDiscount', format: '0' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MechanicComment'), dataIndex: 'MechanicComment' },
                { xtype: 'datecolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('StartDate'), dataIndex: 'StartDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
                {
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsStartDate'),
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
                { xtype: 'datecolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('EndDate'), dataIndex: 'EndDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
                {
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsEndDate'),
                    dataIndex: 'MarsEndDate',
                    width: 125,
                    filter: {
                        xtype: 'marsdatefield',
                        operator: 'like'
                    }
                },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('BudgetYear'), dataIndex: 'BudgetYear' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PromoDuration'), dataIndex: 'PromoDuration', format: '0' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('EventName'), dataIndex: 'EventName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PromoStatusName'), dataIndex: 'PromoStatusName' },
                {
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('InOut'),
                    dataIndex: 'InOut',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                {
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('IsGrowthAcceleration'),
                    dataIndex: 'IsGrowthAcceleration',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInstoreMechanicName'), dataIndex: 'PlanInstoreMechanicName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInstoreMechanicTypeName'), dataIndex: 'PlanInstoreMechanicTypeName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInstoreMechanicDiscount'), dataIndex: 'PlanInstoreMechanicDiscount', format: '0' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInStoreShelfPrice'), dataIndex: 'PlanInStoreShelfPrice', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PCPrice'), dataIndex: 'PCPrice', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoVolume'), dataIndex: 'PlanPromoVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaselineVolume'), dataIndex: 'PlanPromoBaselineVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalVolume'), dataIndex: 'PlanPromoIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalVolume'), dataIndex: 'PlanPromoNetIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectVolume'), dataIndex: 'PlanPromoPostPromoEffectVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectVolumeW1'), dataIndex: 'PlanPromoPostPromoEffectVolumeW1', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectVolumeW2'), dataIndex: 'PlanPromoPostPromoEffectVolumeW2', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaselineLSV'), dataIndex: 'PlanPromoBaselineLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalLSV'), dataIndex: 'PlanPromoIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoLSV'), dataIndex: 'PlanPromoLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoUpliftPercent'), dataIndex: 'PlanPromoUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoTIShopper'), dataIndex: 'PlanPromoTIShopper', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoTIMarketing'), dataIndex: 'PlanPromoTIMarketing', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoXSites'), dataIndex: 'PlanPromoXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCatalogue'), dataIndex: 'PlanPromoCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPOSMInClient'), dataIndex: 'PlanPromoPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBranding'), dataIndex: 'PlanPromoBranding', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBTL'), dataIndex: 'PlanPromoBTL', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProduction'), dataIndex: 'PlanPromoCostProduction', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProdXSites'), dataIndex: 'PlanPromoCostProdXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProdCatalogue'), dataIndex: 'PlanPromoCostProdCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProdPOSMInClient'), dataIndex: 'PlanPromoCostProdPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCost'), dataIndex: 'PlanPromoCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('TIBasePercent'), dataIndex: 'TIBasePercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalBaseTI'), dataIndex: 'PlanPromoIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalBaseTI'), dataIndex: 'PlanPromoNetIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('COGSPercent'), dataIndex: 'COGSPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('COGSTn'), dataIndex: 'COGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalCOGS'), dataIndex: 'PlanPromoIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalCOGS'), dataIndex: 'PlanPromoNetIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalCOGSTn'), dataIndex: 'PlanPromoIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalCOGSTn'), dataIndex: 'PlanPromoNetIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoTotalCost'), dataIndex: 'PlanPromoTotalCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectLSV'), dataIndex: 'PlanPromoPostPromoEffectLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalLSV'), dataIndex: 'PlanPromoNetIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetLSV'), dataIndex: 'PlanPromoNetLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaselineBaseTI'), dataIndex: 'PlanPromoBaselineBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaseTI'), dataIndex: 'PlanPromoBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetBaseTI'), dataIndex: 'PlanPromoNetBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNSVtn'), dataIndex: 'PlanPromoNSVtn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNSV'), dataIndex: 'PlanPromoNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetNSV'), dataIndex: 'PlanPromoNetNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalNSV'), dataIndex: 'PlanPromoIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalNSV'), dataIndex: 'PlanPromoNetIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalMAC'), dataIndex: 'PlanPromoIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalMAC'), dataIndex: 'PlanPromoNetIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalEarnings'), dataIndex: 'PlanPromoIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalEarnings'), dataIndex: 'PlanPromoNetIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoROIPercent'), dataIndex: 'PlanPromoROIPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetROIPercent'), dataIndex: 'PlanPromoNetROIPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetUpliftPercent'), dataIndex: 'PlanPromoNetUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanAddTIMarketingApproved'), dataIndex: 'PlanAddTIMarketingApproved', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanAddTIShopperCalculated'), dataIndex: 'PlanAddTIShopperCalculated', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanAddTIShopperApproved'), dataIndex: 'PlanAddTIShopperApproved', format: '0.00' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreMechanicName'), dataIndex: 'ActualInStoreMechanicName' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreMechanicTypeName'), dataIndex: 'ActualInStoreMechanicTypeName' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreDiscount'), dataIndex: 'ActualInStoreDiscount', format: '0' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreShelfPrice'), dataIndex: 'ActualInStoreShelfPrice', format: '0.00' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('InvoiceNumber'), dataIndex: 'InvoiceNumber' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoVolume'), dataIndex: 'ActualPromoVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaselineVolume'), dataIndex: 'ActualPromoBaselineVolume', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalVolume'), dataIndex: 'ActualPromoIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalVolume'), dataIndex: 'ActualPromoNetIncrementalVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoPostPromoEffectVolume'), dataIndex: 'ActualPromoPostPromoEffectVolume', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoVolumeByCompensation'), dataIndex: 'ActualPromoVolumeByCompensation', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoVolumeSI'), dataIndex: 'ActualPromoVolumeSI', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaselineLSV'), dataIndex: 'ActualPromoBaselineLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalLSV'), dataIndex: 'ActualPromoIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSVByCompensation'), dataIndex: 'ActualPromoLSVByCompensation', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSV'), dataIndex: 'ActualPromoLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSVSI'), dataIndex: 'ActualPromoLSVSI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSVSO'), dataIndex: 'ActualPromoLSVSO', format: '0.00' },

                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoUpliftPercent'), dataIndex: 'ActualPromoUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetUpliftPercent'), dataIndex: 'ActualPromoNetUpliftPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoTIShopper'), dataIndex: 'ActualPromoTIShopper', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoTIMarketing'), dataIndex: 'ActualPromoTIMarketing', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoXSites'), dataIndex: 'ActualPromoXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCatalogue'), dataIndex: 'ActualPromoCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoPOSMInClient'), dataIndex: 'ActualPromoPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBranding'), dataIndex: 'ActualPromoBranding', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBTL'), dataIndex: 'ActualPromoBTL', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProduction'), dataIndex: 'ActualPromoCostProduction', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProdXSites'), dataIndex: 'ActualPromoCostProdXSites', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProdCatalogue'), dataIndex: 'ActualPromoCostProdCatalogue', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProdPOSMInClient'), dataIndex: 'ActualPromoCostProdPOSMInClient', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCost'), dataIndex: 'ActualPromoCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalBaseTI'), dataIndex: 'ActualPromoIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalBaseTI'), dataIndex: 'ActualPromoNetIncrementalBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalCOGS'), dataIndex: 'ActualPromoIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalCOGS'), dataIndex: 'ActualPromoNetIncrementalCOGS', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalCOGSTn'), dataIndex: 'ActualPromoIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalCOGSTn'), dataIndex: 'ActualPromoNetIncrementalCOGSTn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoTotalCost'), dataIndex: 'ActualPromoTotalCost', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoPostPromoEffectLSV'), dataIndex: 'ActualPromoPostPromoEffectLSV', format: '0' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalLSV'), dataIndex: 'ActualPromoNetIncrementalLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetLSV'), dataIndex: 'ActualPromoNetLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalNSV'), dataIndex: 'ActualPromoIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalNSV'), dataIndex: 'ActualPromoNetIncrementalNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaselineBaseTI'), dataIndex: 'ActualPromoBaselineBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaseTI'), dataIndex: 'ActualPromoBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetBaseTI'), dataIndex: 'ActualPromoNetBaseTI', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNSVtn'), dataIndex: 'ActualPromoNSVtn', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNSV'), dataIndex: 'ActualPromoNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetNSV'), dataIndex: 'ActualPromoNetNSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalMAC'), dataIndex: 'ActualPromoIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalMAC'), dataIndex: 'ActualPromoNetIncrementalMAC', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalEarnings'), dataIndex: 'ActualPromoIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalEarnings'), dataIndex: 'ActualPromoNetIncrementalEarnings', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoROIPercent'), dataIndex: 'ActualPromoROIPercent', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetROIPercent'), dataIndex: 'ActualPromoNetROIPercent', format: '0.00' },
                //Add TI
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualAddTIMarketing'), dataIndex: 'ActualAddTIMarketing', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualAddTIShopper'), dataIndex: 'ActualAddTIShopper', format: '0.00' },
                { text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PromoTypesName'), dataIndex: 'PromoTypesName' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('SumInvoice'), dataIndex: 'SumInvoice', format: '0.00' },
                //New calculation parameters for ROI
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalMACLSV'), dataIndex: 'PlanPromoIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalMACLSV'), dataIndex: 'PlanPromoNetIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalMACLSV'), dataIndex: 'ActualPromoIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalMACLSV'), dataIndex: 'ActualPromoNetIncrementalMACLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalEarningsLSV'), dataIndex: 'PlanPromoIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalEarningsLSV'), dataIndex: 'PlanPromoNetIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalEarningsLSV'), dataIndex: 'ActualPromoIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalEarningsLSV'), dataIndex: 'ActualPromoNetIncrementalEarningsLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoROIPercentLSV'), dataIndex: 'PlanPromoROIPercentLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetROIPercentLSV'), dataIndex: 'PlanPromoNetROIPercentLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoROIPercentLSV'), dataIndex: 'ActualPromoROIPercentLSV', format: '0.00' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetROIPercentLSV'), dataIndex: 'ActualPromoNetROIPercentLSV', format: '0.00' },
                {
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('IsPriceIncrease'),
                    dataIndex: 'IsPriceIncrease',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                {
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('IsApolloExport'),
                    dataIndex: 'IsApolloExport',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                {
                    text: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MLmodel'),
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
        model: 'App.model.tpm.promopriceincreaseroireport.PromoPriceIncreaseROIReport',
        items: [
            { xtype: 'numberfield', name: 'Number', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('Number') },
            { xtype: 'textfield', name: 'Client1LevelName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('Client1LevelName') },
            { xtype: 'textfield', name: 'Client2LevelName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('Client2LevelName') },
            { xtype: 'textfield', name: 'ClientName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ClientName') },
            { xtype: 'textfield', name: 'BrandName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('BrandName') },
            { xtype: 'textfield', name: 'TechnologyName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('TechnologyName') },
            { xtype: 'textfield', name: 'SubName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('SubName') },
            { xtype: 'textfield', name: 'ProductSubrangesList', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ProductSubrangesList') },
            { xtype: 'textfield', name: 'MarsMechanicName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsMechanicName') },
            { xtype: 'textfield', name: 'MarsMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsMechanicTypeName') },
            { xtype: 'numberfield', name: 'MarsMechanicDiscount', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MarsMechanicDiscount') },
            { xtype: 'textfield', name: 'MechanicComment', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('MechanicComment') },
            { xtype: 'datefield', name: 'StartDate', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('StartDate') },
            { xtype: 'datefield', name: 'EndDate', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('EndDate') },
            { xtype: 'numberfield', name: 'PromoDuration', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PromoDuration') },
            { xtype: 'textfield', name: 'EventName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('EventName') },
            { xtype: 'textfield', name: 'PromoStatusName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PromoStatusName') },
            { xtype: 'numberfield', name: 'InOut', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('InOut') },
            { xtype: 'numberfield', name: 'IsGrowthAcceleration', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('IsGrowthAcceleration') },
            { xtype: 'numberfield', name: 'IsPriceIncrease', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('IsPriceIncrease') },
            { xtype: 'textfield', name: 'PlanInstoreMechanicName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInstoreMechanicName') },
            { xtype: 'textfield', name: 'PlanInstoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInstoreMechanicTypeName') },
            { xtype: 'numberfield', name: 'PlanInstoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInstoreMechanicDiscount') },
            { xtype: 'numberfield', name: 'PlanInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanInStoreShelfPrice') },
            { xtype: 'numberfield', name: 'PCPrice', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PCPrice') },
            { xtype: 'numberfield', name: 'PlanPromoVolume', fieldLabel: l10n.ns('tpm', 'PlanPromoVolume').value('PlanPromoVolume') },
            { xtype: 'numberfield', name: 'PlanPromoBaselineVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaselineVolume') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalVolume') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalVolume') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectVolume') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectVolumeW1', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectVolumeW1') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectVolumeW2', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectVolumeW2') },

            { xtype: 'numberfield', name: 'PlanPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaselineLSV') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalLSV') },
            { xtype: 'numberfield', name: 'PlanPromoLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoLSV') },
            { xtype: 'numberfield', name: 'PlanPromoUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoUpliftPercent') },
            { xtype: 'numberfield', name: 'PlanPromoTIShopper', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoTIShopper') },
            { xtype: 'numberfield', name: 'PlanPromoTIMarketing', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoTIMarketing') },
            { xtype: 'numberfield', name: 'PlanPromoXSites', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoXSites') },
            { xtype: 'numberfield', name: 'PlanPromoCatalogue', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCatalogue') },
            { xtype: 'numberfield', name: 'PlanPromoPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPOSMInClient') },
            { xtype: 'numberfield', name: 'PlanPromoBranding', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBranding') },
            { xtype: 'numberfield', name: 'PlanPromoBTL', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBTL') },
            { xtype: 'numberfield', name: 'PlanPromoCostProduction', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProduction') },
            { xtype: 'numberfield', name: 'PlanPromoCostProdXSites', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProdXSites') },
            { xtype: 'numberfield', name: 'PlanPromoCostProdCatalogue', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProdCatalogue') },
            { xtype: 'numberfield', name: 'PlanPromoCostProdPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCostProdPOSMInClient') },
            { xtype: 'numberfield', name: 'PlanPromoCost', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoCost') },
            { xtype: 'numberfield', name: 'TIBasePercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('TIBasePercent') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalBaseTI') },
            { xtype: 'numberfield', name: 'COGSPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('COGSPercent') },
            { xtype: 'numberfield', name: 'COGSTn', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('COGSTn') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalCOGS') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalCOGS') },
            { xtype: 'numberfield', name: 'PlanPromoTotalCost', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoTotalCost') },
            { xtype: 'numberfield', name: 'PlanPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoPostPromoEffectLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetLSV') },
            { xtype: 'numberfield', name: 'PlanPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaselineBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNetBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetBaseTI') },
            { xtype: 'numberfield', name: 'PlanPromoNSVtn', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNSVtn') },
            { xtype: 'numberfield', name: 'PlanPromoNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetNSV') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalNSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalNSV') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalMAC') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalMAC') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalEarnings') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalEarnings') },
            { xtype: 'numberfield', name: 'PlanPromoROIPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoROIPercent') },
            { xtype: 'numberfield', name: 'PlanPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetROIPercent') },
            { xtype: 'numberfield', name: 'PlanPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetUpliftPercent') },
            { xtype: 'textfield', name: 'ActualInStoreMechanicName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreMechanicName') },
            { xtype: 'textfield', name: 'ActualInStoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreMechanicTypeName') },
            { xtype: 'numberfield', name: 'ActualInStoreDiscount', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreDiscount') },
            { xtype: 'numberfield', name: 'ActualInStoreShelfPrice', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualInStoreShelfPrice') },
            { xtype: 'textfield', name: 'InvoiceNumber', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('InvoiceNumber') },
            { xtype: 'numberfield', name: 'ActualPromoVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoVolume') },
            { xtype: 'numberfield', name: 'ActualPromoBaselineVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaselineVolume') },

            { xtype: 'numberfield', name: 'ActualPromoIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalVolume') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalVolume') },
            { xtype: 'numberfield', name: 'ActualPromoPostPromoEffectVolume', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoPostPromoEffectVolume') },
            { xtype: 'numberfield', name: 'ActualPromoVolumeByCompensation', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoVolumeByCompensation') },
            { xtype: 'numberfield', name: 'ActualPromoVolumeSI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoVolumeSI') },

            { xtype: 'numberfield', name: 'ActualPromoBaselineLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaselineLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalLSV') },
            { xtype: 'numberfield', name: 'ActualPromoLSVByCompensation', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSVByCompensation') },
            { xtype: 'numberfield', name: 'ActualPromoLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSV') },
            { xtype: 'numberfield', name: 'ActualPromoLSVSI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSVSI') },
            { xtype: 'numberfield', name: 'ActualPromoLSVSO', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoLSVSO') },

            { xtype: 'numberfield', name: 'ActualPromoUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoUpliftPercent') },
            { xtype: 'numberfield', name: 'ActualPromoNetUpliftPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetUpliftPercent') },
            { xtype: 'numberfield', name: 'ActualPromoTIShopper', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoTIShopper') },
            { xtype: 'numberfield', name: 'ActualPromoTIMarketing', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoTIMarketing') },
            { xtype: 'numberfield', name: 'ActualPromoXSites', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoXSites') },
            { xtype: 'numberfield', name: 'ActualPromoCatalogue', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCatalogue') },
            { xtype: 'numberfield', name: 'ActualPromoPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoPOSMInClient') },
            { xtype: 'numberfield', name: 'ActualPromoBranding', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBranding') },
            { xtype: 'numberfield', name: 'ActualPromoBTL', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBTL') },
            { xtype: 'numberfield', name: 'ActualPromoCostProduction', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProduction') },
            { xtype: 'numberfield', name: 'ActualPromoCostProdXSites', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProdXSites') },
            { xtype: 'numberfield', name: 'ActualPromoCostProdCatalogue', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProdCatalogue') },
            { xtype: 'numberfield', name: 'ActualPromoCostProdPOSMInClient', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCostProdPOSMInClient') },
            { xtype: 'numberfield', name: 'ActualPromoCost', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoCost') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalCOGS') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalCOGS', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalCOGS') },
            { xtype: 'numberfield', name: 'ActualPromoTotalCost', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoTotalCost') },
            { xtype: 'numberfield', name: 'ActualPromoPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoPostPromoEffectLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalNSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalNSV') },
            { xtype: 'numberfield', name: 'ActualPromoBaselineBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaselineBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoNetBaseTI', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetBaseTI') },
            { xtype: 'numberfield', name: 'ActualPromoNSVtn', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNSVtn') },
            { xtype: 'numberfield', name: 'ActualPromoNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetNSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetNSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalMAC') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalMAC', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalMAC') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalEarnings') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalEarnings', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalEarnings') },
            { xtype: 'numberfield', name: 'ActualPromoROIPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoROIPercent') },
            { xtype: 'numberfield', name: 'ActualPromoNetROIPercent', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetROIPercent') },
            { xtype: 'textfield', name: 'PromoTypesName', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PromoTypesName') },
            { xtype: 'numberfield', name: 'SumInvoice', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('SumInvoice') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalMACLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalMACLSV') },
            { xtype: 'numberfield', name: 'PlanPromoIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'ActualPromoIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetIncrementalEarningsLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetIncrementalEarningsLSV') },
            { xtype: 'numberfield', name: 'PlanPromoROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoROIPercentLSV') },
            { xtype: 'numberfield', name: 'PlanPromoNetROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('PlanPromoNetROIPercentLSV') },
            { xtype: 'numberfield', name: 'ActualPromoROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoROIPercentLSV') },
            { xtype: 'numberfield', name: 'ActualPromoNetROIPercentLSV', fieldLabel: l10n.ns('tpm', 'PromoPriceIncreaseROIReport').value('ActualPromoNetROIPercentLSV') },
        ]
    }]
});
