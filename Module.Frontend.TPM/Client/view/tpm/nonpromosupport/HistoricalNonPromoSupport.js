Ext.define('App.view.tpm.nonpromosupport.HistoricalNonPromoSupport', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalnonpromosupport',
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
            model: 'App.model.tpm.nonpromosupport.HistoricalNonPromoSupport',
            storeId: 'historicalnonpromosupportstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonpromosupport.HistoricalNonPromoSupport',
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
                text: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalNonPromoSupport', 'OperationType'),
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
        model: 'App.model.tpm.nonpromosupport.HistoricalNonPromoSupport',
			items: [{
				xtype: 'singlelinedisplayfield',
				name: '_User',
				fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_User')
			}, {
				xtype: 'singlelinedisplayfield',
				name: '_Role',
				fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Role')
			}, {
				xtype: 'singlelinedisplayfield',
				name: '_EditDate',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
				fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_EditDate')
			}, {
				xtype: 'singlelinedisplayfield',
				name: '_Operation',
				renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBudgetSubItem', 'OperationType'),
				fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Operation')
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'PlanQuantity',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('PlanQuantity'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'ActualQuantity',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('ActualQuantity'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'PlanCostTE',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('PlanCostTE'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'ActualCostTE',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('ActualCostTE'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'StartDate',
				renderer: Ext.util.Format.dateRenderer('d.m.Y'),
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('StartDate'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'EndDate',
				renderer: Ext.util.Format.dateRenderer('d.m.Y'),
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('EndDate'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'AttachFileName',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('AttachFileName'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'InvoiceNumber',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('InvoiceNumber'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'ClientTreeFullPathName',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('ClientTreeFullPathName'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'BrandTechName',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('BrandTechName'),
			}, {
				xtype: 'singlelinedisplayfield',
				name: 'NonPromoEquipmentEquipmentType',
				fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('NonPromoEquipmentEquipmentType'),
			}]
    }]
});
             