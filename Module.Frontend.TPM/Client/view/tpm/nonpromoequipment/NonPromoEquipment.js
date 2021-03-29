Ext.define('App.view.tpm.nonpromoequipment.NonPromoEquipment', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.nonpromoequipment',
    title: l10n.ns('tpm', 'compositePanelTitles').value('NonPromoEquipment'),

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
            model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
            storeId: 'nonpromoequipmentstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
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
                text: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
                dataIndex: 'EquipmentType'
            }, {
                text: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
                dataIndex: 'Description_ru'
            }, {
                text: l10n.ns('tpm', 'NonPromoEquipment').value('BudgetItemName'),
                dataIndex: 'BudgetItemName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budgetitemshort',
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
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
        items: [{
            xtype: 'textfield',
            name: 'EquipmentType',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
        }, {
            xtype: 'textfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
        }, {
            text: l10n.ns('tpm', 'NonPromoEquipment').value('BudgetItemName'),
            dataIndex: 'BudgetItemName',
            filter: {
                type: 'search',
                selectorWidget: 'budgetitemshort',
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
        }]
    }]
});
