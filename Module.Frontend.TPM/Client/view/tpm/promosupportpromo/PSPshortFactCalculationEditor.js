Ext.define('App.view.tpm.promosupportpromo.PSPshortFactCalculationEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.pspshortfactcalculationeditor',
    width: 450,
    minWidth: 450,
    maxHeight: 600,

    afterWindowShow: function (scope, isCreating) {
        scope.down('numberfield[name=FactCalculation]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'BudgetSubItemName',
            fieldLabel: l10n.ns('tpm', 'PSPshortFactCalculation').value('BudgetSubItemName'),
        }, {
            xtype: 'numberfield',
            name: 'FactCalculation',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PSPshortFactCalculation').value('FactCalculation'),
        }]
    }
});
