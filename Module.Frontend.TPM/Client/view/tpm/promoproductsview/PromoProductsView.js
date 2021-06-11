Ext.define('App.view.tpm.promoproductsview.PromoProductsView', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promoproductsview',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoProductsView'),

    promoId: null,
    isReadable: true,
    crudAccess: [],

    storePromoProductsView: Ext.create('Ext.data.Store', {
        model: 'App.model.tpm.promoproductcorrection.PromoProductCorrection',
        autoLoad: false,
        root: {}
    }),

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
                itemId: 'importExport',
                items: [{
                    glyph: 0xf220,
                    itemgroup: 'customloadimportbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                    resource: '{0}',
                    action: 'FullImportXLSX',
                    allowFormat: ['zip', 'xlsx']
                }, {
                    glyph: 0xf21d,
                    itemId: 'customloadimporttemplatexlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                    action: 'DownloadTemplateXLSX'
                }, {
                    glyph: 0xf21d,
                    itemId: 'customexportxlsxbutton',
                    promoId: this.promoId,
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
            model: 'App.model.tpm.promoproductsview.PromoProductsView',
            storeId: 'promoproductsviewstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoproductsview.PromoProductsView',
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
                text: l10n.ns('tpm', 'PromoProductsView').value('ZREP'),
                dataIndex: 'ZREP'
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('ProductEN'),
                dataIndex: 'ProductEN'
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('PlanProductBaselineLSV'),
                dataIndex: 'PlanProductBaselineLSV',
                extraOperator: 'gte_lt',
                additionalDivision: 1000000.0,
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        valueToDisplay = value / 1000000.0;
                        return Ext.util.Format.number(valueToDisplay, '0.00');
                    } else {
                        return null;
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('PlanProductUpliftPercent'),
                dataIndex: 'PlanProductUpliftPercent',
                extraOperator: 'gte_lt',
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('PlanProductIncrementalLSV'),
                dataIndex: 'PlanProductIncrementalLSV',
                extraOperator: 'gte_lt',
                additionalDivision: 1000000.0,
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        valueToDisplay = value / 1000000.0;
                        return Ext.util.Format.number(valueToDisplay, '0.00');
                    } else {
                        return null;
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('PlanProductLSV'),
                dataIndex: 'PlanProductLSV',
                extraOperator: 'gte_lt',
                additionalDivision: 1000000.0,
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        valueToDisplay = value / 1000000.0;
                        return Ext.util.Format.number(valueToDisplay, '0.00');
                    } else {
                        return null;
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('PlanProductBaselineCaseQty'),
                dataIndex: 'PlanProductBaselineCaseQty',
                extraOperator: 'gte_lt',
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('PlanProductIncrementalCaseQty'),
                dataIndex: 'PlanProductIncrementalCaseQty',
                extraOperator: 'gte_lt',
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('PlanProductCaseQty'),
                dataIndex: 'PlanProductCaseQty',
                extraOperator: 'gte_lt',
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('AverageMarker'),
                dataIndex: 'AverageMarker'
            }, {
                text: l10n.ns('tpm', 'PromoProductsView').value('IsCorrection'),
                dataIndex: 'IsCorrection'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promoproductsview.PromoProductsView',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('ZREP')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('ProductEN')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanProductBaselineLSV',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductBaselineLSV'),
            renderer: function (value) {
                if (value !== null && value !== undefined) {
                    valueToDisplay = value / 1000000.0;
                    return Ext.util.Format.number(valueToDisplay, '0.00');
                } else {
                    return null;
                }
            }
        }, {
            xtype: 'numberfield',
            name: 'PlanProductUpliftPercent',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductUpliftPercent'),
            renderer: function (value) {
                if (value !== null && value !== undefined) {
                    return Ext.util.Format.number(value, '0.00');
                } else {
                    return null;
                }
            }
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanProductIncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductIncrementalLSV'),
            renderer: function (value) {
                if (value !== null && value !== undefined) {
                    valueToDisplay = value / 1000000.0;
                    return Ext.util.Format.number(valueToDisplay, '0.00');
                } else {
                    return null;
                }
            }
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanProductLSV',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductLSV'),
            renderer: function (value) {
                if (value !== null && value !== undefined) {
                    valueToDisplay = value / 1000000.0;
                    return Ext.util.Format.number(valueToDisplay, '0.00');
                } else {
                    return null;
                }
            }
        }, {
            xtype: 'numberfield',
            name: 'PlanProductBaselineCaseQty',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductBaselineCaseQty'),
            renderer: function (value) {
                if (value !== null && value !== undefined) {
                    return Ext.util.Format.number(value, '0.00');
                } else {
                    return null;
                }
            }
        }, {
            xtype: 'numberfield',
            name: 'PlanProductIncrementalCaseQty',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductIncrementalCaseQty'),
            renderer: function (value) {
                if (value !== null && value !== undefined) {
                    return Ext.util.Format.number(value, '0.00');
                } else {
                    return null;
                }
            }
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanProductCaseQty',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductCaseQty'),
            renderer: function (value) {
                if (value !== null && value !== undefined) {
                    return Ext.util.Format.number(value, '0.00');
                } else {
                    return null;
                }
            }
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SumInvoiceProduct',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('SumInvoiceProduct')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'AverageMarker',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('AverageMarker')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IsCorrection',
            fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('IsCorrection')
        }]
    }]
});