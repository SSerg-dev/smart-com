Ext.define('App.view.core.filebuffer.FileBufferEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.filebuffereditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ 
			xtype: 'searchfield',
			name: 'InterfaceId',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileBuffer').value('InterfaceId'),
			selectorWidget: 'interface',
			valueField: 'Id',
			displayField: 'Name',
			store: {
				type: 'directorystore',
				model: 'App.model.core.interface.Interface',
				extendedFilter: {
					xclass: 'App.ExtFilterContext',
					supportedModels: [{
						xclass: 'App.ExtSelectionFilterModel',
						model: 'App.model.core.interface.Interface',
						modelId: 'efselectionmodel'
					}, {
						xclass: 'App.ExtTextFilterModel',
						modelId: 'eftextmodel'
					}]
				}
			},
			mapping: [{
				from: 'Name',
				to: 'InterfaceName'
			}, {
				from: 'Direction',
				to: 'InterfaceDirection'
			}]		
		}, { 
			xtype: 'textfield',
			name: 'FileName',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileBuffer').value('FileName')		
		}, { 
			xtype: 'textfield',
			name: 'Status',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileBuffer').value('Status')		
		}]
    }
});