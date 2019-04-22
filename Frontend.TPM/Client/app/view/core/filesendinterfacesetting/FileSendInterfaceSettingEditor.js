Ext.define('App.view.core.filesendinterfacesetting.FileSendInterfaceSettingEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.filesendinterfacesettingeditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ 
			xtype: 'searchfield',
			name: 'InterfaceId',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('InterfaceId'),
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
			name: 'TargetPath',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetPath')		
		}, { 
			xtype: 'textfield',
			name: 'TargetFileMask',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetFileMask')		
		}, { 
			xtype: 'textfield',
			name: 'SendHandler',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('SendHandler')		
		}]
    }
});