Ext.define('App.view.tpm.incrementalpromo.IncrementalPromoEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.incrementalpromoeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

    afterWindowShow: function (scope, isCreating) {
        scope.down('numberfield[name=PlanPromoIncrementalCases]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductZREP')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoClient',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoClient')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoNumber',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoNumber')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoName')
        }, {
            xtype: 'numberfield',
            name: 'PlanPromoIncrementalCases',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalCases'),
            minValue: 0,
            maxValue: 100000000000000000000,
            allowDecimals: true,
            allowExponential: false,
            allowOnlyWhitespace: false,
            allowBlank: false
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CasePrice',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('CasePrice')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoIncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalLSV')
        }]
    },
});
