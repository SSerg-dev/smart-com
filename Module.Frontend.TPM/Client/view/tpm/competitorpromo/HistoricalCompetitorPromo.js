﻿Ext.define('App.view.tpm.competitorpromo.HistoricalCompetitorPromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalcompetitorpromo',
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
            model: 'App.model.tpm.competitorpromo.HistoricalCompetitorPromo',
            storeId: 'historicalcompetitorpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.competitorpromo.HistoricalCompetitorPromo',
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
                    text: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCompetitorPromo', 'OperationType'),
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
        model: 'App.model.tpm.historicalcompetitorpromo.HistoricalCompetitorPromo',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCompetitorPromo', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorPromo').value('_Operation')
        },  {
            xtype: 'singlelinedisplayfield',
            name: 'Number',
            fieldLabel: l10n.ns('tpm', 'Promo').value('Number'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorName',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('CompetitorName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorBrandTechBrandTech',
            fieldLabel: l10n.ns('tpm', 'Promo').value('BrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicType',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('MechanicType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
        }]
    }]
});
