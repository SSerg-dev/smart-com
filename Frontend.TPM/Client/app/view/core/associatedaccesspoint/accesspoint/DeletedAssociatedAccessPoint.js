Ext.define('App.view.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedassociatedaccesspointaccesspoint',
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
            model: 'App.model.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint',
            storeId: 'deletedassociatedaccesspointaccesspointstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint',
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
				text: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('DeletedDate'),
				dataIndex: 'DeletedDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('Resource'),
				dataIndex: 'Resource'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('Action'),
				dataIndex: 'Action'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('Description'),
				dataIndex: 'Description'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint',
        items: [{ 
			xtype: 'singlelinedisplayfield',
			name: 'DeletedDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('DeletedDate')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'Resource',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('Resource')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'Action',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('Action')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'Description',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedAccessPoint').value('Description')		
		}]
    }]

});