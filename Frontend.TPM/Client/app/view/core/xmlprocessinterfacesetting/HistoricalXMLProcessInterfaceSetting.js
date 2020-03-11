Ext.define('App.view.core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalxmlprocessinterfacesetting',
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
            model: 'App.model.core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSetting',
            storeId: 'historicalxmlprocessinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSetting',
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
                text: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalXMLProcessInterfaceSetting', 'OperationType'),
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
        model: 'App.model.core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSetting',
        items: [{
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_User')
        }, {
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_Role')
        }, {
            name: '_EditDate',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_EditDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
        }, {
            name: '_Operation',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('_Operation'),
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalXMLProcessInterfaceSetting', 'OperationType')
        }, {
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('InterfaceName')
        }, {
            name: 'RootElement',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('RootElement')
        }, {
            name: 'ProcessHandler',
            fieldLabel: l10n.ns('core', 'HistoricalXMLProcessInterfaceSetting').value('ProcessHandler')
        }]
    }]

});