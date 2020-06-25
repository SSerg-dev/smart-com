Ext.define('App.view.tpm.brandtech.HistoricalBrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalbrandtech',
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
            model: 'App.model.tpm.brandtech.HistoricalBrandTech',
            storeId: 'historicalbrandtechstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.brandtech.HistoricalBrandTech',
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
                text: l10n.ns('tpm', 'HistoricalBrandTech').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalBrandTech').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalBrandTech').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalBrandTech').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBrandTech', 'OperationType'),
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
        model: 'App.model.tpm.brandtech.HistoricalBrandTech',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBrandTech', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TechnologyName',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('TechnologyName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TechnologySubBrand',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('SubBrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech_code',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTechsub_code',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandsegTechsub_code'),
        }]
    }]
});
