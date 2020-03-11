Ext.define('App.view.core.csvprocessinterfacesetting.CSVProcessInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.csvprocessinterfacesettingeditor',
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
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('InterfaceId'),
        }, {
            xtype: 'textfield',
            name: 'Delimiter',
            allowOnlyWhitespace: true,
            allowBlank: true,
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('Delimiter')
        }, {
            xtype: 'checkboxfield',
            name: 'UseQuoting',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('UseQuoting')
        }, {
            xtype: 'textfield',
            name: 'QuoteChar',
            allowOnlyWhitespace: true,
            allowBlank: true,
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('QuoteChar')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProcessHandler',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('ProcessHandler')
        }]
    }
});