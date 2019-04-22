Ext.define('App.view.tpm.demand.HistoricalDemand', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicaldemand',
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
            model: 'App.model.tpm.demand.HistoricalDemand',
            storeId: 'historicaldemandstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.demand.HistoricalDemand',
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
                text: l10n.ns('tpm', 'HistoricalDemand').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalDemand').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalDemand').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalDemand').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalDemand', 'OperationType'),
                filter: {
                    type: 'combo',
                    valueField: 'id',
                    store: {
                        type: 'operationtypestore'
                    },
                    operator: 'eq'
                }
            }
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.demand.HistoricalDemand',
        items: [
        {
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalDemand', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Number'),
            name: 'Number'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientCommercialSubnetCommercialNetName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesStart',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesStart'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesEnd'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanBaseline',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanBaseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanDuration',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanDuration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanUplift',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanUplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanIncremental',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanIncremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanActivity',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanActivity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanSteal',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanSteal'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactBaseline',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactBaseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactDuration',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactDuration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactUplift',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactUplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactIncremental',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactIncremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactActivity',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactActivity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactSteal',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactSteal'),
        }]
    }]
});
