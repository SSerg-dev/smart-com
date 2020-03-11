Ext.define('App.view.core.interface.InterfaceEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.interfaceeditor',
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
            fieldLabel: l10n.ns('core', 'Interface').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Direction',
            fieldLabel: l10n.ns('core', 'Interface').value('Direction')
        }, {
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'Interface').value('Description')
        }]
    }
});     