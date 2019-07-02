Ext.define('App.view.tpm.promocalculating.CalculatingInfoLogEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.calculatinginfologeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'CalculatingInfoLog').value('Type'),
            name: 'Type'
        }, {
            fieldLabel: '',
            xtype: 'textarea',
            name: 'Message',
            height: 120,
            //fieldLabel: l10n.ns('tpm', 'CalculatingInfoLog').value('Message')
        }]
    },

    buttons: [{
        itemId: 'edit'
    }, {
        itemId: 'ok',
    }, {
        itemId: 'canceledit'
    }, {
        itemId: 'close',
        text: l10n.ns('tpm', 'button').value('Close'),
    }]
});
