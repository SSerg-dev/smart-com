Ext.define('App.view.tpm.clientkpidata.HistoricalClientKPIData', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalclientkpidata',
    title: l10n.ns('core', 'compositePanelTitles').value('historyPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.clientkpidata.HistoricalClientKPIData',
            storeId: 'historicalclientkpidatastore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.clientkpidata.HistoricalClientKPIData',
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
                flex: 1,
                minWidth: 100
            },
            items: [
                {
                    text: l10n.ns('tpm', 'HistoricalClientKPIData').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalClientKPIData').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalClientKPIData').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalClientKPIData').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalClientKPIData', 'OperationType'),
                    filter: {
                        type: 'combo',
                        valueField: 'id',
                        store: {
                            type: 'operationtypestore'
                        },
                        operator: 'eq'
                    }
                }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.clientkpidata.HistoricalClientKPIData',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalClientKPIData').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalClientKPIData').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClientKPIData').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalClientKPIData', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClientKPIData').value('_Operation')
        }, {
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
            xtype: 'singlelinedisplayfield',
            name: 'ShopperTiPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ShopperTiPlanPercent')
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
            xtype: 'singlelinedisplayfield',
            name: 'MarketingTiPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiPlanPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketingTiPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('MarketingTiPlan')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoTiCostPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoTiCostPlanPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoTiCostPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoTiCostPlan')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoTiCostYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoTiCostYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoTiCostYTDPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoTiCostYTDPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoTiCostYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoTiCostYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoTiCostYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoTiCostYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'NonPromoTiCostPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('NonPromoTiCostPlanPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductionPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionPlanPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductionPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ProductionPlan')
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
            xtype: 'singlelinedisplayfield',
            name: 'BrandingPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BrandingPlan')
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
            xtype: 'singlelinedisplayfield',
            name: 'BTLPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLPlan')
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
            xtype: 'singlelinedisplayfield',
            name: 'ROIPlanPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('ROIPlanPercent')
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
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalNSVPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('IncrementalNSVPlan')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalNSVYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('IncrementalNSVYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalNSVYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('IncrementalNSVYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoNSVPlan',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoNSVPlan')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoNSVYTD',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoNSVYTD')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoNSVYEE',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('PromoNSVYEE')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEEPercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BTLYEEPercent',
            fieldLabel: l10n.ns('tpm', 'ClientKPIData').value('BTLYEEPercent')
        }]
    }]
});
