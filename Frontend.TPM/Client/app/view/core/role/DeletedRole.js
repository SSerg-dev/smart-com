Ext.define('App.view.core.role.DeletedRole', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedrole',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.role.DeletedRole',
            storeId: 'deletedrolestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.role.DeletedRole',
                    modelId: 'efselectionmodel'
				}, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
				}]
            },
			sorters: [{
				property: 'DeletedDate',
				direction: 'DESC'
			}]        
		},

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
				minWidth: 100
            },
            items: [{ 
				text: l10n.ns('core', 'DeletedRole').value('DeletedDate'),
				dataIndex: 'DeletedDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, { 
				text: l10n.ns('core', 'DeletedRole').value('SystemName'),
				dataIndex: 'SystemName'
			}, { 
				text: l10n.ns('core', 'DeletedRole').value('DisplayName'),
				dataIndex: 'DisplayName'
			}, { 
				text: l10n.ns('core', 'DeletedRole').value('IsAllow'),
				dataIndex: 'IsAllow',
				xtype: 'booleancolumn',
				trueText: l10n.ns('core', 'booleanValues').value('true'),
				falseText: l10n.ns('core', 'booleanValues').value('false')
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.role.DeletedRole',
        items: [{ 
			xtype: 'singlelinedisplayfield',
			name: 'DeletedDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'DeletedRole').value('DeletedDate')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'SystemName',
			fieldLabel: l10n.ns('core', 'DeletedRole').value('SystemName')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'DisplayName',
			fieldLabel: l10n.ns('core', 'DeletedRole').value('DisplayName')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'IsAllow',
			renderer: App.RenderHelper.getBooleanRenderer(),
			fieldLabel: l10n.ns('core', 'DeletedRole').value('IsAllow')		
		}]
    }]

});