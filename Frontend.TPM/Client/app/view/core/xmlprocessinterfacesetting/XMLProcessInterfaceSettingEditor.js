Ext.define('App.view.core.xmlprocessinterfacesetting.XMLProcessInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.xmlprocessinterfacesettingeditor',
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
            fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('InterfaceId'),
        }, {
            xtype: 'textfield',
            name: 'RootElement',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('RootElement')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProcessHandler',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('ProcessHandler')
        }]
    }
});