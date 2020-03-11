Ext.define('App.view.core.filesendinterfacesetting.HistoricalFileSendInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalfilesendinterfacesetting',
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
            model: 'App.model.core.filesendinterfacesetting.HistoricalFileSendInterfaceSetting',
            storeId: 'historicalfilesendinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.filesendinterfacesetting.HistoricalFileSendInterfaceSetting',
                    modelId: 'efselectionmodel'
                }]
            },
            sorters: [{
                property: '_EditDate',
                direction: 'DESC'
            }],
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1
            },
            items: [{
                text: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileSendInterfaceSetting', 'OperationType'),
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
        model: 'App.model.core.filesendinterfacesetting.HistoricalFileSendInterfaceSetting',
        items: [{
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_User')
        }, {
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_Role')
        }, {
            name: '_EditDate',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_EditDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
        }, {
            name: '_Operation',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('_Operation'),
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileSendInterfaceSetting', 'OperationType')
        }, {
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('InterfaceName')
        }, {
            name: 'TargetPath',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('TargetPath')
        }, {
            name: 'TargetFileMask',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('TargetFileMask')
        }, {
            name: 'SendHandler',
            fieldLabel: l10n.ns('core', 'HistoricalFileSendInterfaceSetting').value('SendHandler')
        }]
    }]

});