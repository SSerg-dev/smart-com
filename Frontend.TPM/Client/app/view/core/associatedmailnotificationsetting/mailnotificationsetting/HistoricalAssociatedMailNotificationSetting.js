Ext.define('App.view.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalassociatedmailnotificationsettingmailnotificationsetting',
    title: l10n.ns('core', 'compositePanelTitles').value('historyPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',

        store: {
            type: 'directorystore',
            model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting',
            storeId: 'historicalassociatedmailnotificationsettingmailnotificationsettingstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting',
                    modelId: 'efselectionmodel'
                }]
            },
            sorters: [{
                property: '_EditDate',
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
                text: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalMailNotificationSetting', 'OperationType'),
                filter: {
                    type: 'combo',
                    valueField: 'id',
                    store: {
                        type: 'operationtypestore'
                    },
                    operator: 'eq'
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalMailNotificationSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Name')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Description')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Subject',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Subject')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Body',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Body')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IsDisabled',
            renderer: App.RenderHelper.getBooleanRenderer(),
            trueText: l10n.ns('core', 'booleanValues').value('true'),
            falseText: l10n.ns('core', 'booleanValues').value('false'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('IsDisabled')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'To',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('To')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CC',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('CC')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BCC',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('BCC')
        }]
    }]

});