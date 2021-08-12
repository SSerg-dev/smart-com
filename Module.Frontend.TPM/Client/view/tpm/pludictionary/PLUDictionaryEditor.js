Ext.define('App.view.tpm.pludictionary.PLUDictionaryEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.pludictionaryeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeName',
            fieldLabel: l10n.ns('tpm', 'PLUDictionary').value('ClientTreeName'),
            selectText:false,
            focus: function () {
			}
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ObjectId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientTreeObjectId'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN_PC',
            fieldLabel: l10n.ns('tpm', 'PLUDictionary').value('EAN_PC'),
        }, {
            xtype: 'textfield',
            name: 'PluCode',
            fieldLabel: l10n.ns('tpm', 'PLUDictionary').value('PluCode'),
            allowBlank: true,
            allowOnlyWhitespace: true,
        }]
    }
})