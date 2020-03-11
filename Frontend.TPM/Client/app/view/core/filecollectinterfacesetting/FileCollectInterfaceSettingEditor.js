Ext.define('App.view.core.filecollectinterfacesetting.FileCollectInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.filecollectinterfacesettingeditor',
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
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('InterfaceId'),
        }, {
            xtype: 'textfield',
            name: 'SourcePath',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('SourcePath')
        }, {
            xtype: 'textfield',
            name: 'SourceFileMask',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('SourceFileMask')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CollectHandler',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('CollectHandler')
        }]
    }
});