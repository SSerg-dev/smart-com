Ext.define('App.view.tpm.budgetsubitem.BudgetSubItem', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.budgetsubitem',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BudgetSubItem'),

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
            model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
            storeId: 'budgetsubitemstore',
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
                text: l10n.ns('tpm', 'BudgetSubItem').value('BudgetName'),
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
                text: l10n.ns('tpm', 'BudgetSubItem').value('BudgetItemName'),
                dataIndex: 'BudgetItemName',
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
                text: l10n.ns('tpm', 'BudgetSubItem').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'BudgetSubItem').value('Description_ru'),
                dataIndex: 'Description_ru'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetName'),
            name: 'BudgetName',
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetItemName'),
            name: 'BudgetItemId',
            selectorWidget: 'budgetitem',
            valueField: 'Id',
            displayField: 'Name',
            onTrigger2Click: function () {
                var budget = this.up().down('[name=BudgetName]');

                this.clearValue();
                budget.setValue(null);
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    var budget = field.up().down('[name=BudgetName]');
                    var budgetValue = newValue != undefined ? field.record.get('BudgetName') : null;

                    budget.setValue(budgetValue);
                }
            },
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budgetitem.BudgetItem',
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
                to: 'BudgetItemName'
            }]
        }, {
            xtype: 'textfield',
            name: 'Name',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Name'),
        }, {
            xtype: 'textfield',
            name: 'Description_ru',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Description_ru'),
        }]
    }]
});