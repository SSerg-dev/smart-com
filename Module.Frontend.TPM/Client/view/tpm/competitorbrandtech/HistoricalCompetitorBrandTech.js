Ext.define('App.view.tpm.competitorbrandtech.HistoricalCompetitorBrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalcompetitorbrandtech',
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
            model: 'App.model.tpm.competitorbrandtech.HistoricalCompetitorBrandTech',
            storeId: 'historicalcompetitorbrandtechstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.competitorbrandtech.HistoricalCompetitorBrandTech',
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
                    text: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCompetitorBrandTech', 'OperationType'),
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
        model: 'App.model.tpm.competitorbrandtech.HistoricalCompetitorBrandTech',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCompetitorBrandTech', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Color',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('Color'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorName',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('CompetitorName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('BrandTech'),
        }]
    }]
});
