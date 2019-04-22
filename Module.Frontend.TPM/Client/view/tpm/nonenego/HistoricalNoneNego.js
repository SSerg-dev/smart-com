Ext.define('App.view.tpm.nonenego.HistoricalNoneNego', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalnonenego',
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
            model: 'App.model.tpm.nonenego.HistoricalNoneNego',
            storeId: 'historicalnonenegostore',

            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonenego.HistoricalNoneNego',
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
                text: l10n.ns('tpm', 'HistoricalNoneNego').value('_User'),
                dataIndex: '_User',

                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalNoneNego').value('_Role'),
                dataIndex: '_Role',

                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalNoneNego').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalNoneNego').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalNoneNego', 'OperationType'),

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
        model: 'App.model.tpm.nonenego.HistoricalNoneNego',

        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalNoneNego', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientHierarchy',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientHierarchy')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductHierarchy',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ProductHierarchy')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicTypeName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicTypeName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('Discount')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FromDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('FromDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ToDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ToDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('CreateDate')
        }]
    }]
})