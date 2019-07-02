Ext.define('App.view.tpm.rejectreason.RejectReasonEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.rejectreasoneditor',
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
            fieldLabel: l10n.ns('tpm', 'RejectReason').value('Name')
        }, {
            xtype: 'textfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'RejectReason').value('SystemName')
        }]
    }
});
