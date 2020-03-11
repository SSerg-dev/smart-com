Ext.define('App.view.core.filebuffer.FileBufferEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.filebuffereditor',
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
            fieldLabel: l10n.ns('core', 'FileBuffer').value('InterfaceId'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceDirection',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileBuffer').value('InterfaceDirection')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            allowOnlyWhitespace: false,
            allowBlank: false,
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'FileBuffer').value('CreateDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FileName',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileBuffer').value('FileName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            allowOnlyWhitespace: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'FileBuffer').value('Status')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProcessDate',
            allowOnlyWhitespace: false,
            allowBlank: false,
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'FileBuffer').value('ProcessDate')
        }]
    }
});