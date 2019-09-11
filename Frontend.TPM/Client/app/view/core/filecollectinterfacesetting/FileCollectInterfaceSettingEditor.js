Ext.define('App.view.core.filecollectinterfacesetting.FileCollectInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.filecollectinterfacesettingeditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ 
			xtype: 'searchfield',
			name: 'InterfaceId',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('InterfaceId'),
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
			xtype: 'textfield',
			name: 'CollectHandler',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('CollectHandler')		
		}]
    }
});