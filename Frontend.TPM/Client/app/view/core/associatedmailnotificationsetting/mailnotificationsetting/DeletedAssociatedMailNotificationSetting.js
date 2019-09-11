Ext.define('App.view.core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedassociatedmailnotificationsettingmailnotificationsetting',
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
            model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting',
            storeId: 'deletedassociatedmailnotificationsettingmailnotificationsettingstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting',
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
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Name'),
				dataIndex: 'Name'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Description'),
				dataIndex: 'Description'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Subject'),
				dataIndex: 'Subject'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Body'),
				dataIndex: 'Body'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('IsDisabled'),
				dataIndex: 'IsDisabled',
				xtype: 'booleancolumn',
				trueText: l10n.ns('core', 'booleanValues').value('true'),
				falseText: l10n.ns('core', 'booleanValues').value('false'),
				renderer: App.RenderHelper.getBooleanRenderer()
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('To'),
				dataIndex: 'To'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('CC'),
				dataIndex: 'CC'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('BCC'),
				dataIndex: 'BCC'
			}, { 
				text: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('DeletedDate'),
				dataIndex: 'DeletedDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting',
        items: [{ 
					xtype: 'singlelinedisplayfield',
					name: 'Name',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Name')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'Description',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Description')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'Subject',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Subject')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'Body',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('Body')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'IsDisabled',
			renderer: App.RenderHelper.getBooleanRenderer(),
			trueText: l10n.ns('core', 'booleanValues').value('true'),
			falseText: l10n.ns('core', 'booleanValues').value('false'),
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('IsDisabled')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'To',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('To')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'CC',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('CC')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'BCC',
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('BCC')		
		}, { 
					xtype: 'singlelinedisplayfield',
					name: 'DeletedDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'DeletedAssociatedMailNotificationSetting').value('DeletedDate')		
		}]
    }]

});