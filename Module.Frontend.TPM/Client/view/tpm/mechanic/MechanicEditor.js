Ext.define('App.view.tpm.mechanic.MechanicEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.mechaniceditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('Name'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('SystemName'),
        }]
    }
});