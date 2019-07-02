Ext.define('App.view.tpm.product.DeletedProduct', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedproduct',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.product.DeletedProduct',
            storeId: 'deletedproductstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.product.DeletedProduct',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
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
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                dataIndex: 'DeletedDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'Product').value('ZREP'),
                dataIndex: 'ZREP'
            }, {
                text: l10n.ns('tpm', 'Product').value('EAN_Case'),
                dataIndex: 'EAN_Case'
            }, {
                text: l10n.ns('tpm', 'Product').value('EAN_PC'),
                dataIndex: 'EAN_PC'
            }, {
                text: l10n.ns('tpm', 'Product').value('ProductRU'),
                dataIndex: 'ProductRU'
            }, {
                text: l10n.ns('tpm', 'Product').value('ProductEN'),
                dataIndex: 'ProductEN'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandFlagAbbr'),
                dataIndex: 'BrandFlagAbbr'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandFlag'),
                dataIndex: 'BrandFlag'
            }, {
                text: l10n.ns('tpm', 'Product').value('SubmarkFlag'),
                dataIndex: 'SubmarkFlag'
            }, {
                text: l10n.ns('tpm', 'Product').value('IngredientVariety'),
                dataIndex: 'IngredientVariety'
            }, {
                text: l10n.ns('tpm', 'Product').value('ProductCategory'),
                dataIndex: 'ProductCategory'
            }, {
                text: l10n.ns('tpm', 'Product').value('ProductType'),
                dataIndex: 'ProductType'
            }, {
                text: l10n.ns('tpm', 'Product').value('MarketSegment'),
                dataIndex: 'MarketSegment'
            }, {
                text: l10n.ns('tpm', 'Product').value('SupplySegment'),
                dataIndex: 'SupplySegment'
            }, {
                text: l10n.ns('tpm', 'Product').value('FunctionalVariety'),
                dataIndex: 'FunctionalVariety'
            }, {
                text: l10n.ns('tpm', 'Product').value('Size'),
                dataIndex: 'Size'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandEssence'),
                dataIndex: 'BrandEssence'
            }, {
                text: l10n.ns('tpm', 'Product').value('PackType'),
                dataIndex: 'PackType'
            }, {
                text: l10n.ns('tpm', 'Product').value('GroupSize'),
                dataIndex: 'GroupSize'
            }, {
                text: l10n.ns('tpm', 'Product').value('TradedUnitFormat'),
                dataIndex: 'TradedUnitFormat'
            }, {
                text: l10n.ns('tpm', 'Product').value('ConsumerPackFormat'),
                dataIndex: 'ConsumerPackFormat'
            }, {
                text: l10n.ns('tpm', 'Product').value('UOM_PC2Case'),
                dataIndex: 'UOM_PC2Case'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.product.DeletedProduct',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
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
            name: 'ProductRU',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductRU'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductEN'),
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
        }]
    }]
});
