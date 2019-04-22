Ext.define('App.view.core.interface.InterfaceEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.interfaceeditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ 
			xtype: 'textfield',
			name: 'Name',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'Interface').value('Name')		
		}, { 
			xtype: 'textfield',
			name: 'Direction',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'Interface').value('Direction')		
		}, { 
			xtype: 'textfield',
			name: 'Description',
			allowOnlyWhitespace: true,
			allowBlank: true,
			fieldLabel: l10n.ns('core', 'Interface').value('Description')		
		}]
    }
});