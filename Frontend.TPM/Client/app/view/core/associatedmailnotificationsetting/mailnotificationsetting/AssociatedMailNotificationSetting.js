Ext.define('App.view.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.associatedmailnotificationsettingmailnotificationsetting',
    title: l10n.ns('core', 'compositePanelTitles').value('AssociatedMailNotificationSettingTitle'),

    dockedItems: [{
        xtype: 'standarddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting',
            storeId: 'associatedmailnotificationsettingmailnotificationsettingstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting',
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
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Description'),
                dataIndex: 'Description'
            }, {
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Subject'),
                dataIndex: 'Subject'
            }, {
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Body'),
                dataIndex: 'Body'
            }, {
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('IsDisabled'),
                dataIndex: 'IsDisabled',
                xtype: 'booleancolumn',
                trueText: l10n.ns('core', 'booleanValues').value('true'),
                falseText: l10n.ns('core', 'booleanValues').value('false'),
                renderer: App.RenderHelper.getBooleanRenderer()
            }, {
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('To'),
                dataIndex: 'To'
            }, {
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('CC'),
                dataIndex: 'CC'
            }, {
                text: l10n.ns('core', 'AssociatedMailNotificationSetting').value('BCC'),
                dataIndex: 'BCC'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Description')
        }, {
            xtype: 'textfield',
            name: 'Subject',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Subject')
        }, {
            xtype: 'textfield',
            name: 'Body',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Body')
        }, {
            xtype: 'checkboxfield',
            name: 'IsDisabled',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('IsDisabled')
        }, {
            xtype: 'textfield',
            name: 'To',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('To')
        }, {
            xtype: 'textfield',
            name: 'CC',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('CC')
        }, {
            xtype: 'textfield',
            name: 'BCC',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('BCC')
        }]
    }]

});