Ext.define('App.view.tpm.baseline.BaseLine', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.baseline',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BaseLine'),

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

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.baseline.BaseLine',
            storeId: 'baselinestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.baseline.BaseLine',
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
                text: l10n.ns('tpm', 'BaseLine').value('ProductZREP'),
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
                text: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode'),
                dataIndex: 'DemandCode'
            }, {
                text: l10n.ns('tpm', 'BaseLine').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'BaseLine').value('QTY'),
                dataIndex: 'QTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'BaseLine').value('Price'),
                dataIndex: 'Price'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'BaseLine').value('BaselineLSV'),
                dataIndex: 'BaselineLSV'
            }, {
                xtype: 'numbercolumn',
                format: '0.',
                text: l10n.ns('tpm', 'BaseLine').value('Type'),
                dataIndex: 'Type'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.baseline.BaseLine',
        items: [{
            xtype: 'searchfield',
            name: 'ProductId',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ProductZREP'),
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
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode')
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('StartDate'),
            allowBlank: false,
            allowOnlyWhitespace: false,
        }, {
            xtype: 'numberfield',
            name: 'QTY',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('QTY'),
        }, {
            xtype: 'numberfield',
            name: 'Price',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Price'),
        }, {
            xtype: 'numberfield',
            name: 'BaselineLSV',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('BaselineLSV'),
        }, {
            xtype: 'numberfield',
            name: 'Type',
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Type'),
        }]
    }]
});