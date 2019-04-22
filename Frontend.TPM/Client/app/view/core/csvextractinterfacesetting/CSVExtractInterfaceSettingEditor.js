Ext.define('App.view.core.csvextractinterfacesetting.CSVExtractInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.csvextractinterfacesettingeditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ 
			xtype: 'searchfield',
			name: 'InterfaceId',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'CSVExtractInterfaceSetting').value('InterfaceId'),
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
			name: 'FileNameMask',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'CSVExtractInterfaceSetting').value('FileNameMask')		
		}, { 
			xtype: 'textfield',
			name: 'ExtractHandler',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'CSVExtractInterfaceSetting').value('ExtractHandler')		
		}]
    }
});