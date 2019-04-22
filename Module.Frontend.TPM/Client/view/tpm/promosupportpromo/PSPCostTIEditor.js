Ext.define('App.view.tpm.promosupportpromo.PSPCostTIEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.pspcosttieditor',
    width: 450,
    minWidth: 450,
    maxHeight: 600,
    title: l10n.ns('core').value('updateWindowTitle'),

    pspId: null,

    afterWindowShow: function (scope, isCreating) {
        scope.down('numberfield[name=FactCalculation]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'numberfield',
            name: 'PlanCalculation',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromo').value('PlanCostTE'),
        }, {
            xtype: 'numberfield',
            name: 'FactCalculation',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            allowOnlyWhitespace: false,
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromo').value('ActualCostTE'),
        }]
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'ok',
        ui: 'green-button-footer-toolbar',
    }]
});
