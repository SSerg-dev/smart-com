Ext.define('App.view.tpm.promocalculating.CalculatingInfoLogEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.calculatinginfologeditor',
    width: 800,
    minWidth: 800,
    minHeight: 280,
    resizeHandles:'all',
    layout: {
        type: 'fit',
        align: 'stretch'
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'CalculatingInfoLog').value('Type'),
            name: 'Type',
        }, {
            fieldLabel: '',
            xtype: 'textarea',
            name: 'Message',
            //fieldLabel: l10n.ns('tpm', 'CalculatingInfoLog').value('Message')
            }],
        listeners: {
            resize: function (panel) {
                panel.down('textarea[name=Message]').setHeight(panel.getHeight() - panel.down('singlelinedisplayfield[name=Type]').getHeight() - 30);
            }
        }
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
