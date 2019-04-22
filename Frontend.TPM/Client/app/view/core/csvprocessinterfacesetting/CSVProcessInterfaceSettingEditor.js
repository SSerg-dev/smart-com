Ext.define('App.view.core.csvprocessinterfacesetting.CSVProcessInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.csvprocessinterfacesettingeditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ 
			xtype: 'searchfield',
			name: 'InterfaceId',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('InterfaceId'),
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
			}]		
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
			xtype: 'textfield',
			name: 'ProcessHandler',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('ProcessHandler')		
		}]
    }
});