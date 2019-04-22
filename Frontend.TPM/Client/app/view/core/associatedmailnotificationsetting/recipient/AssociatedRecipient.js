Ext.define('App.view.core.associatedmailnotificationsetting.recipient.AssociatedRecipient', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.associatedmailnotificationsettingrecipient',
    title: l10n.ns('core', 'compositePanelTitles').value('AssociatedRecipientTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'associateddirectorystore',
            model: 'App.model.core.associatedmailnotificationsetting.recipient.AssociatedRecipient',
            storeId: 'associatedmailnotificationsettingrecipientstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associatedmailnotificationsetting.recipient.AssociatedRecipient',
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
				text: l10n.ns('core', 'AssociatedRecipient').value('Type'),
				dataIndex: 'Type'
			}, { 
				text: l10n.ns('core', 'AssociatedRecipient').value('Value'),
				dataIndex: 'Value'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        editableModes: [Core.BaseEditableDetailForm.LOADED_MODE],
		model: 'App.model.core.associatedmailnotificationsetting.recipient.AssociatedRecipient',
        items: [{ 
            xtype: 'combobox',
            name: 'Type',
            editable: false,
            allowBlank: false,
            queryMode: 'local',
            valueField: 'id',
            forceSelection: true,
            store: {
                type: 'recipienttypestore'
            },
			fieldLabel: l10n.ns('core', 'AssociatedRecipient').value('Type')		
        }, {
            xtype: 'textfield',
            name: 'Value',
            id: 'emailnotificationfield',
            allowOnlyWhitespace: false,
            allowBlank: false,
            hidden: true,
            disabled: true,
            fieldLabel: l10n.ns('core', 'AssociatedRecipient').value('Value')
        }, {
            xtype: 'searchfield',
            name: 'Value',
            id: 'usernotificationfield',
            allowOnlyWhitespace: false,
            allowBlank: true,
            hidden: true,
            disabled: true,
            fieldLabel: l10n.ns('core', 'AssociatedRecipient').value('Value'),
            selectorWidget: 'associateddbuseruser',
            valueField: 'Name',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.core.user.User',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.core.user.User',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Value'
            }]
        }, {
            xtype: 'searchfield',
            name: 'Value',
            id: 'rolenotificationfield',
            allowOnlyWhitespace: false,
            allowBlank: true,
            hidden: true,
            disabled: true,
            fieldLabel: l10n.ns('core', 'AssociatedRecipient').value('Value'),
            selectorWidget: 'role',
            valueField: 'SystemName',
            displayField: 'SystemName',
            store: {
                type: 'directorystore',
                model: 'App.model.core.role.Role',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.core.role.Role',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            },
            mapping: [{
                from: 'SystemName',
                to: 'Value'
            }]
        }, {
            xtype: 'textfield',
            name: 'Value',
            id: 'functionnotificationfield',
            allowOnlyWhitespace: false,
            allowBlank: false,
            hidden: true,
            disabled: true,
            fieldLabel: l10n.ns('core', 'AssociatedRecipient').value('Value')
        }]
    }]

});