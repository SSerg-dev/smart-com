Ext.define('App.view.tpm.promodemand.HistoricalPromoDemand', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalpromodemand',
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
            model: 'App.model.tpm.promodemand.HistoricalPromoDemand',
            storeId: 'historicalpromodemandstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promodemand.HistoricalPromoDemand',
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
                text: l10n.ns('tpm', 'HistoricalPromoDemand').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPromoDemand').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPromoDemand').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalPromoDemand').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromoDemand', 'OperationType'),
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
        model: 'App.model.tpm.promodemand.HistoricalPromoDemand',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechBrandName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Account',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Account'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BaseClientObjectId',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BaseClientObjectId'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Discount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Week',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Week'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Baseline',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Baseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Uplift',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Uplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Incremental',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Incremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Activity',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Activity'),
        }]
    }]

});
