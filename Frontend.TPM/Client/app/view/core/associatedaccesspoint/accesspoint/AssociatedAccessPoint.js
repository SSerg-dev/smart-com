Ext.define('App.view.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.associatedaccesspointaccesspoint',
    title: l10n.ns('core', 'compositePanelTitles').value('AssociatedAccessPointTitle'),

    dockedItems: [{
        xtype: 'standarddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint',
            storeId: 'associatedaccesspointaccesspointstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint',
                    modelId: 'efselectionmodel'
				}, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
				}]
            }        
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
				text: l10n.ns('core', 'AssociatedAccessPoint').value('Resource'),
				dataIndex: 'Resource'
			}, { 
				text: l10n.ns('core', 'AssociatedAccessPoint').value('Action'),
				dataIndex: 'Action'
			}, { 
				text: l10n.ns('core', 'AssociatedAccessPoint').value('Description'),
				dataIndex: 'Description'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint',
        items: [{ 
			xtype: 'textfield',
			name: 'Resource',
			fieldLabel: l10n.ns('core', 'AssociatedAccessPoint').value('Resource')		
		}, { 
			xtype: 'textfield',
			name: 'Action',
			fieldLabel: l10n.ns('core', 'AssociatedAccessPoint').value('Action')		
		}, { 
			xtype: 'textfield',
			name: 'Description',
			allowBlank: true,
			allowOnlyWhitespace: true,
			fieldLabel: l10n.ns('core', 'AssociatedAccessPoint').value('Description')		
		}]
    }]

});