Ext.define('App.view.tpm.product.HistoricalProduct', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalproduct',
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
            model: 'App.model.tpm.product.HistoricalProduct',
            storeId: 'historicalproductstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.product.HistoricalProduct',
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
                text: l10n.ns('tpm', 'HistoricalProduct').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalProduct').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalProduct').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalProduct').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalProduct', 'OperationType'),
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
        model: 'App.model.tpm.product.HistoricalProduct',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalProduct', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'Product').value('ZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN_Case',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_Case'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN_PC',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_PC'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductEN'),
        }, {
            //--
            xtype: 'singlelinedisplayfield',
            name: 'Brand',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Brand_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Technology',
            fieldLabel: l10n.ns('tpm', 'Product').value('Technology'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Tech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Tech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Segmen_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Segmen_code'),
        }, {
            //--
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Brandsegtech',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brandsegtech'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTechsub_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTechsub_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTechsub',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTechsub'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubBrand_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrand_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubBrand',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrand'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandFlagAbbr',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlagAbbr'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlag'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubmarkFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubmarkFlag'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IngredientVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('IngredientVariety'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductCategory',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductCategory'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductType',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketSegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('MarketSegment'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SupplySegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('SupplySegment'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FunctionalVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('FunctionalVariety'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'Product').value('Size'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandEssence',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandEssence'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PackType',
            fieldLabel: l10n.ns('tpm', 'Product').value('PackType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'GroupSize',
            fieldLabel: l10n.ns('tpm', 'Product').value('GroupSize'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TradedUnitFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('TradedUnitFormat'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ConsumerPackFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('ConsumerPackFormat'),
        }, {
            xtype: 'numberfield',
            name: 'UOM_PC2Case',
            fieldLabel: l10n.ns('tpm', 'Product').value('UOM_PC2Case'),
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
        }, {
            xtype: 'numberfield',
            name: 'Division',
            fieldLabel: l10n.ns('tpm', 'Product').value('Division'),
            allowDecimals: false,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
        }]
    }]
});
