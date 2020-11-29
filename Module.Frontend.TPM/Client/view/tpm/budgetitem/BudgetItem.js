Ext.define('App.view.tpm.budgetitem.BudgetItem', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.budgetitem',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BudgetItem'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
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
                    glyph: 0xf220,
                    itemgroup: 'loadimportbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                    resource: '{0}',
                    action: 'FullImportXLSX',
                    allowFormat: ['zip', 'xlsx']
                }, {
                    glyph: 0xf21d,
                    itemId: 'loadimporttemplatexlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                    action: 'DownloadTemplateXLSX'
                }, {
                    glyph: 0xf21d,
                    itemId: 'exportxlsxbutton',
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
            model: 'App.model.tpm.budgetitem.BudgetItem',
            storeId: 'budgetitemstore',
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
                text: l10n.ns('tpm', 'BudgetItem').value('BudgetName'),
                dataIndex: 'BudgetName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budget',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budget.Budget',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budget.Budget',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'BudgetItem').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'BudgetItem').value('Description_ru'),
                dataIndex: 'Description_ru'
            }, {
                text: l10n.ns('tpm', 'BudgetItem').value('ButtonColor'),
                dataIndex: 'ButtonColor',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div>', record.get('ButtonColor'));
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.budgetitem.BudgetItem',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('Name'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('BudgetName'),
            name: 'BudgetId',
            selectorWidget: 'budget',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budget.Budget',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.budget.Budget',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BudgetName'
            }]
        }, {
            xtype: 'textfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('Description_ru'),
        }, {
            text: l10n.ns('tpm', 'BudgetItem').value('ButtonColor'),
            dataIndex: 'ButtonColor',
            renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div>', record.get('ButtonColor'));
            }
        }]
    }]
});