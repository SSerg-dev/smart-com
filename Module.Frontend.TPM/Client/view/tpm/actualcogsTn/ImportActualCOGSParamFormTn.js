Ext.define('App.view.tpm.actualcogsTn.ImportActualCOGSParamFormTn', {
    extend: 'App.view.core.common.ImportParamForm',
    alias: 'widget.importforecastparamform',

    initFields: function (fieldValues) {
        this.callParent([fieldValues]);
    },

    items: [{
        xtype: 'numberfield',
        name: 'year',
        fieldLabel: l10n.ns('tpm', 'ActualCOGSTn').value('Year'),
        allowBlank: false,
        value: new Date().getFullYear(),
        minValue: new Date().getFullYear() - 3,
        maxValue: new Date().getFullYear() + 10
    }]
});
