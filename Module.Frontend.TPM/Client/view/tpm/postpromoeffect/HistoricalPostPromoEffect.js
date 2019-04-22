Ext.define('App.view.tpm.postpromoeffect.HistoricalPostPromoEffect', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalpostpromoeffect',
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
            model: 'App.model.tpm.postpromoeffect.HistoricalPostPromoEffect',
            storeId: 'historicalpostpromoeffectstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.postpromoeffect.HistoricalPostPromoEffect',
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
                text: l10n.ns('tpm', 'HistoricalPostPromoEffect').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPostPromoEffect').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPostPromoEffect').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalPostPromoEffect').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPostPromoEffect', 'OperationType'),
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
        model: 'App.model.tpm.postpromoeffect.HistoricalPostPromoEffect',
        items:[{
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ProductTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EffectWeek1',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek1'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EffectWeek2',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek2'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TotalEffect',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('TotalEffect'),
        }]
    }]
});
