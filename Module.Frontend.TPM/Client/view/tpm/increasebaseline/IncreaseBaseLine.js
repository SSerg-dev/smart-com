Ext.define('App.view.tpm.increasebaseline.IncreaseBaseLine', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.increasebaseline',
    title: l10n.ns('tpm', 'compositePanelTitles').value('IncreaseBaseLine'),

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
                    action: 'ExportXLSX'
                }]
            }
        }
    ],

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.increasebaseline.IncreaseBaseLine',
            storeId: 'increasebaselinestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.increasebaseline.IncreaseBaseLine',
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
                minWidth: 110
            },
            items: [{
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('ProductZREP'),
                dataIndex: 'ProductZREP',
                filter: {
                    type: 'search',
                    selectorWidget: 'product',
                    valueField: 'ZREP',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.product.Product',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.product.Product',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('ClientTreeDemandCode'),
                dataIndex: 'DemandCode'
            }, {
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('InputBaselineQTY'),
                dataIndex: 'InputBaselineQTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('SellInBaselineQTY'),
                dataIndex: 'SellInBaselineQTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('SellOutBaselineQTY'),
                dataIndex: 'SellOutBaselineQTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('Type'),
                dataIndex: 'Type'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.increasebaseline.IncreaseBaseLine',
        items: [{
            xtype: 'searchfield',
            name: 'ProductId',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('ProductZREP'),
            selectorWidget: 'product',
            valueField: 'Id',
            displayField: 'ZREP',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.product.Product',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.product.Product',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'ZREP',
                to: 'ProductZREP'
            }]
        }, {
            xtype: 'textfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('ClientTreeDemandCode')
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('StartDate'),
            allowBlank: false,
            allowOnlyWhitespace: false,
        }, {
            xtype: 'numberfield',
            name: 'InputBaselineQTY',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('InputBaselineQTY'),
        }, {
            xtype: 'numberfield',
            name: 'SellInBaselineQTY',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('SellInBaselineQTY'),
        }, {
            xtype: 'numberfield',
            name: 'SellOutBaselineQTY',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('SellOutBaselineQTY'),
        }, {
            xtype: 'numberfield',
            name: 'Type',
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('Type'),
        }]
    }]
});