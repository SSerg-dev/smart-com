Ext.define('App.view.tpm.clientkpidata.ClientKPIData', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.clientkpidata',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientKPIData'),
    height: 500,
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
        }, {
            itemId: 'updatebutton',
            action: 'Patch',
            glyph: 0xf64f,
            text: l10n.ns('core', 'crud').value('updateButtonText'),
            tooltip: l10n.ns('core', 'crud').value('updateButtonText')
        }, {
            itemId: 'historybutton',
            resource: 'HistoricalClientDashboards',
            action: 'GetHistoricalClientDashboards',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText')
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
        //TODO: не работает для сложных хедеров
        //ResourceMgr.getAdditionalMenu('core').base = {
        //    glyph: 0xf068,
        //    text: l10n.ns('core', 'additionalMenu').value('additionalBtn'),

        //    menu: {
        //        xtype: 'customheadermenu',
        //        items: [{
        //            glyph: 0xf4eb,
        //            itemId: 'gridsettings',
        //            text: l10n.ns('core', 'additionalMenu').value('gridSettingsMenuItem'),
        //            action: 'SaveGridSettings',
        //            resource: 'Security'
        //        }]
        //    }
        //},
        ResourceMgr.getAdditionalMenu('core').import = {
            glyph: 0xf21b,
            text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

            menu: {
                xtype: 'customheadermenu',
                items: [{
                    glyph: 0xf220,
                    itemId: 'customloadimportbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                    resource: 'ClientDashboardViews',
                    action: 'FullImportXLSX',
                    allowFormat: ['zip', 'xlsx']
                }, {
                    glyph: 0xf21d,
                    itemId: 'customloadimporttemplatebutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                    action: 'DownloadTemplateXLSX'
                }, {
                    glyph: 0xf21d,
                    itemId: 'customexportbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                    action: 'ExportXLSX'
                }]
            }
        }
    ],

    items: [{
        xtype: 'customlockedgrid',
        itemId: 'datatable',
        enableColumnMove: false,
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            //Что бы не ломался скролл при быстром прокручивании
            pageSize: 20000,
            type: 'directorystore',
            model: 'App.model.tpm.clientkpidata.ClientKPIData',
            storeId: 'clientkpidatastore',
            sorters: [{
                property: 'Year',
                direction: 'DESC'
            }, {
                property: 'ObjectId',
                direction: 'ASC'
            }, {
                property: 'BrandTechName',
                direction: 'ASC'
            }],
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.clientkpidata.ClientKPIData',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
        },


        columns: [{
            text: l10n.ns('tpm', 'ClientKPIData').value('ObjectId'),
            dataIndex: 'ObjectId',
            locked: true,
            plugins: ['sortbutton'],
            menuDisabled: true,
            filter: true,
            minWidth: 80,
            width: 80,
            listeners: {
                resize: function (me, width, height, oldWidth, oldHeight) {
                    if (oldWidth) {
                        var lockedGrid = me.up('customlockedgrid').lockedGrid;
                        if (!lockedGrid.resizing) {
                            lockedGrid.resizing = true;
                            lockedGrid.setWidth(lockedGrid.width + (width - oldWidth));
                            lockedGrid.resizing = false;
                        }
                    }
                }
            }
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('ClientHierarchy'),
            dataIndex: 'ClientHierarchy',
            locked: true,
            plugins: ['sortbutton'],
            menuDisabled: true,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'FullPathName',
                    displayField: 'FullPathName',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },
                },
            minWidth: 200,
            width: 200,
            listeners: {
                resize: function (me, width, height, oldWidth, oldHeight) {
                    if (oldWidth) {
                        var lockedGrid = me.up('customlockedgrid').lockedGrid;
                        if (!lockedGrid.resizing) {
                            lockedGrid.resizing = true;
                            lockedGrid.setWidth(lockedGrid.width + (width - oldWidth));
                            lockedGrid.resizing = false;
                        }
                    }
                }
            }
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('BrandTechName'),
            dataIndex: 'BrandTechName',
            locked: true,
            plugins: ['sortbutton'],
                menuDisabled: true,
                filter: {
                    type: 'search',
                    selectorWidget: 'brandtech',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.brandtech.BrandTech',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                },
            minWidth: 135,
            width: 135,
            listeners: {
                resize: function (me, width, height, oldWidth, oldHeight) {
                    if (oldWidth) {
                        var lockedGrid = me.up('customlockedgrid').lockedGrid;
                        if (!lockedGrid.resizing) {
                            lockedGrid.resizing = true;
                            lockedGrid.setWidth(lockedGrid.width + (width - oldWidth));
                            lockedGrid.resizing = false;
                        }
                    }
                }
            }
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('Year'),
            dataIndex: 'Year',
            locked: true,
            plugins: ['sortbutton'],
            menuDisabled: true,
            filter: true,
            minWidth: 70,
            width: 70,
            listeners: {
                resize: function (me, width, height, oldWidth, oldHeight) {
                    if (oldWidth) {
                        var lockedGrid = me.up('customlockedgrid').lockedGrid;
                        if (!lockedGrid.resizing) {
                            lockedGrid.resizing = true;
                            lockedGrid.setWidth(lockedGrid.width + (width - oldWidth));
                            lockedGrid.resizing = false;
                        }
                    }
                }
            }
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('ShopperTI'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                dataIndex: 'ShopperTiPlanPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'ShopperTiPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTDPercent'),
                dataIndex: 'ShopperTiYTDPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'ShopperTiYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEEPercent'),
                dataIndex: 'ShopperTiYEEPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'ShopperTiYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('MarketingTI'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                dataIndex: 'MarketingTiPlanPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'MarketingTiPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('PromoTiCost'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                dataIndex: 'PromoTiCostPlanPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'PromoTiCostPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTDPercent'),
                dataIndex: 'PromoTiCostYTDPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'PromoTiCostYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEEPercent'),
                dataIndex: 'PromoTiCostYEEPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'PromoTiCostYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('NonPromoTiCost'),
                minWidth: 140,
                columns: [{
                    text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                    dataIndex: 'NonPromoTiCostPlanPercent',
                    plugins: ['sortbutton'],
                    menuDisabled: true,
                    sortable: true,
                    minWidth: 140,
                    width: 140,
                    renderer: function (value) {
                        return rounder(value);
                    }
                },]
            }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('Production'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                dataIndex: 'ProductionPlanPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'ProductionPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTDPercent'),
                dataIndex: 'ProductionYTDPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'ProductionYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEEPercent'),
                dataIndex: 'ProductionYEEPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'ProductionYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('Branding'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                dataIndex: 'BrandingPlanPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'BrandingPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTDPercent'),
                dataIndex: 'BrandingYTDPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'BrandingYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEEPercent'),
                dataIndex: 'BrandingYEEPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'BrandingYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('BTL'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                dataIndex: 'BTLPlanPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'BTLPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTDPercent'),
                dataIndex: 'BTLYTDPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'BTLYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEEPercent'),
                dataIndex: 'BTLYEEPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'BTLYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('ROI'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanPercent'),
                dataIndex: 'ROIPlanPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTDPercent'),
                dataIndex: 'ROIYTDPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEEPercent'),
                dataIndex: 'ROIYEEPercent',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 70,
                width: 70,
                renderer: function (value) {
                    return rounder(value);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('LSV'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'LSVPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'LSVYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'LSVYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('IncrementalNSV'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'IncrementalNSVPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'IncrementalNSVYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'IncrementalNSVYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
        }, {
            text: l10n.ns('tpm', 'ClientKPIData').value('PromoNSV'),
            columns: [{
                text: l10n.ns('tpm', 'ClientKPIData').value('PlanMln'),
                dataIndex: 'PromoNSVPlan',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 80,
                width: 80,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YTD'),
                dataIndex: 'PromoNSVYTD',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }, {
                text: l10n.ns('tpm', 'ClientKPIData').value('YEE'),
                dataIndex: 'PromoNSVYEE',
                plugins: ['sortbutton'],
                menuDisabled: true,
                sortable: true,
                minWidth: 50,
                width: 50,
                renderer: function (value) {
                    return rounder(value, true);
                }
            }]
        }]
    }],
});
