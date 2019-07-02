Ext.define('App.view.tpm.mechanictype.MechanicTypeEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.mechanictypeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'MechanicType').value('Name'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: false, allowOnlyWhitespace: false,
            name: 'Discount',
            minValue: 0,
            maxValue: 100,
            fieldLabel: l10n.ns('tpm', 'MechanicType').value('Discount')
        }]
    }
});
