Ext.define('App.view.core.xmlprocessinterfacesetting.XMLProcessInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.xmlprocessinterfacesettingeditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ 
			xtype: 'searchfield',
			name: 'InterfaceId',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('InterfaceId'),
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
			name: 'RootElement',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('RootElement')		
		}, { 
			xtype: 'textfield',
			name: 'ProcessHandler',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('ProcessHandler')		
		}]
    }
});