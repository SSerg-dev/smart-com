Ext.define('App.view.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promoactivitydetailsinfo',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoProduct'),

    customHeaderItems: [

    ResourceMgr.getAdditionalMenu('core').import = {
        glyph: 0xf21b,
        text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

        menu: {
            xtype: 'customheadermenu',
            items: [{
                glyph: 0xf21d,
                itemId: 'customExportXlsxButton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                action: 'ExportXLSX'
            }]
        }
    }],

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

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo',
            storeId: 'actualstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo',
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
                text: l10n.ns('tpm', 'PromoProduct').value('ZREP'),
                dataIndex: 'ZREP',
            }, {
                text: l10n.ns('tpm', 'PromoProduct').value('ProductEN'),
                dataIndex: 'ProductEN',
            }, {
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'PromoProduct').value('PlanProductBaselineLSV'),
                dataIndex: 'PlanProductBaselineLSV',
                hidden: true,
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
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductBaselineLSV'),
                dataIndex: 'ActualProductBaselineLSV',
                hidden: true,
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
                text: l10n.ns('tpm', 'PromoProduct').value('PlanProductIncrementalLSV'),
                dataIndex: 'PlanProductIncrementalLSV',
                hidden: true,
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
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductIncrementalLSV'),
                dataIndex: 'ActualProductIncrementalLSV',
                hidden: true,
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
                text: l10n.ns('tpm', 'PromoProduct').value('PlanProductLSV'),
                dataIndex: 'PlanProductLSV',
                hidden: true,
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
                text: l10n.ns('tpm', 'PromoProduct').value('PlanProductPostPromoEffectLSV'),
                dataIndex: 'PlanProductPostPromoEffectLSV',
                hidden: true,
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
            //
            }, {
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductLSV'),
                dataIndex: 'ActualProductLSV',
                hidden: true,
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
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductPostPromoEffectLSV'),
                dataIndex: 'ActualProductPostPromoEffectLSV',
                hidden: true,
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
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductLSVByCompensation'),
                dataIndex: 'ActualProductLSVByCompensation',
                hidden: true,
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
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductUpliftPercent'),
                dataIndex: 'ActualProductUpliftPercent',
                hidden: true,
                extraOperator: 'gte_lt',
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo',
        items: []
    }]
});
