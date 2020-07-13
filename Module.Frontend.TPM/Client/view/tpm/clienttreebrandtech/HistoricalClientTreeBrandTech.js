Ext.define('App.view.tpm.clienttreebrandtech.HistoricalClientTreeBrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalclienttreebrandtech',
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
            model: 'App.model.tpm.clienttreebrandtech.HistoricalClientTreeBrandTech',
            storeId: 'historicalclienttreebrandtechstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.clienttreebrandtech.HistoricalClientTreeBrandTech',
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
            items: [
                {
                    text: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalClientTreeBrandTech', 'OperationType'),
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
        model: 'App.model.tpm.clienttreebrandtech.HistoricalClientTreeBrandTech',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalClientTreeBrandTech', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ParentClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ParentClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeObjectId'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CurrentBrandTechName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('CurrentBrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Share',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('Share'),
        }]
    }]
});
