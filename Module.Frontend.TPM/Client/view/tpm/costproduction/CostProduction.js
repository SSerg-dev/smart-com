Ext.define('App.view.tpm.costproduction.CostProduction', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.costproduction',
    title: l10n.ns('tpm', 'compositePanelTitles').value('CostProduction'),

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
                    itemId: 'customexportxlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                    action: 'ExportXLSX',
                }]
            }
        }
    ],

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
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
            action: 'PatchCostProduction',
            glyph: 0xf64f,
            text: l10n.ns('core', 'crud').value('updateButtonText'),
            tooltip: l10n.ns('core', 'crud').value('updateButtonText')
        }, '-', {
            itemId: 'updategroupbutton',
            glyph: 0xf4f0,
            action: 'PatchCostProduction',
            text: l10n.ns('tpm', 'button').value('updateGroupButtonText'),
            tooltip: l10n.ns('tpm', 'button').value('updateGroupButtonText'),
        }, {
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
        }],

        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.costproduction.CostProduction',
            storeId: 'costproductionstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.costproduction.CostProduction',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            }
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns('tpm', 'CostProduction').value('ClientTreeFullPathName'),
                dataIndex: 'ClientTreeFullPathName',
                minWidth: 200,
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
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('BudgetSubItemBudgetItemName'),
                dataIndex: 'BudgetSubItemBudgetItemName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budgetitem',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budgetitem.BudgetItem',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budgetitem.BudgetItem',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('BudgetSubItemName'),
                dataIndex: 'BudgetSubItemName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budgetsubitem',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('PlanQuantity'),
                dataIndex: 'PlanQuantity'
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('ActualQuantity'),
                dataIndex: 'ActualQuantity'
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('PlanProdCostPer1Item'),
                dataIndex: 'PlanProdCostPer1Item'
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('ActualProdCostPer1Item'),
                dataIndex: 'ActualProdCostPer1Item'
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('PlanProdCost'),
                dataIndex: 'PlanProdCost'
            }, {
                text: l10n.ns('tpm', 'CostProduction').value('ActualProdCost'),
                dataIndex: 'ActualProdCost'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.costproduction.CostProduction',
        items: [{

        }]
    }]
});