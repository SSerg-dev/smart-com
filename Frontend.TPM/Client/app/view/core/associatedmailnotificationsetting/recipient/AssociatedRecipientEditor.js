Ext.define('App.view.core.associatedmailnotificationsetting.recipient.AssociatedRecipientEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.associatedrecipienteditor',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'combobox',
            name: 'Type',
            editable: false,
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'Recipient').value('Type'),
            queryMode: 'local',
            valueField: 'id',
            forceSelection: true,
            store: {
                type: 'recipienttypestore'
            }
        }, {
            xtype: 'textfield',
            name: 'Value',
            id: 'emailnotificationfield',
            vtype: 'extendEmail',
            allowOnlyWhitespace: false,
            allowBlank: false,
            hidden: true,
            disabled: true,
            fieldLabel: l10n.ns('core', 'Recipient').value('Value')
        }, {
            xtype: 'searchfield',
            name: 'Value',
            id: 'usernotificationfield',
            allowOnlyWhitespace: false,
            allowBlank: true,
            hidden: true,
            disabled: true,
            fieldLabel: l10n.ns('core', 'Recipient').value('Value'),
            selectorWidget: 'user',
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
            fieldLabel: l10n.ns('core', 'Recipient').value('Value'),
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
            fieldLabel: l10n.ns('core', 'Recipient').value('Value')
        }]
    }
});