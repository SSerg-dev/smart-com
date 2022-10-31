Ext.define('App.view.tpm.promoproductpriceincreasesview.PromoProductPriceIncreasesViewEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promoproductpriceincreasesvieweditor',
    width: 500,
    minWidth: 750,
    maxHeight: 500,
    cls: 'readOnlyFields',

    afterWindowShow: function (scope, isCreating) {
        scope.down('numberfield[name=PlanProductUpliftPercent]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [
            {
                xtype: 'singlelinedisplayfield',
                name: 'ZREP',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('ZREP')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'ProductEN',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('ProductEN')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'PlanProductBaselineLSV',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductBaselineLSV'),
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            },
            {
                xtype: 'numberfield',
                name: 'PlanProductUpliftPercent',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductUpliftPercent'),
                decimalSeparator: '.',
                listeners: {
                    change: function (button) {

                        if (button.value <= 0) {

                            button.up().up().up().up().down('#ok').setDisabled(true);
                        } else {

                            button.up().up().up().up().down('#ok').setDisabled(false);
                        }
                    },
                },
                validator: function (value) {
                    return new Number(value) > 0;
                },
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'PlanProductIncrementalLSV',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductIncrementalLSV'),
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'PlanProductLSV',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductLSV'),
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'PlanProductBaselineCaseQty',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductBaselineCaseQty'),
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'PlanProductIncrementalCaseQty',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('PlanProductIncrementalCaseQty'),
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        return Ext.util.Format.number(value, '0.00');
                    } else {
                        return null;
                    }
                }
            },
            {
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
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'AverageMarker',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('AverageMarker'),
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'IsCorrection',
                fieldLabel: l10n.ns('tpm', 'PromoProductsView').value('IsCorrection')
            }
        ]
    }
});
