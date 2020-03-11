Ext.define('App.view.core.filesendinterfacesetting.FileSendInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.filesendinterfacesettingeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceName',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('InterfaceId'),
        }, {
            xtype: 'textfield',
            name: 'TargetPath',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetPath')
        }, {
            xtype: 'textfield',
            name: 'TargetFileMask',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetFileMask')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SendHandler',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('SendHandler')
        }]
    }
});