Ext.define('App.view.tpm.actualproductsview.ActualProductsView', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.actualproduct',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Product'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.actualproductsview.ActualProductsView',
            storeId: 'actualproductstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.actualproductsview.ActualProductsView',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            }
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
                text: l10n.ns('tpm', 'ActualProductsView').value('ZREP'),
                dataIndex: 'ZREP'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('EAN_Case'),
                dataIndex: 'EAN_Case'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('EAN_PC'),
                dataIndex: 'EAN_PC'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('ProductRU'),
                dataIndex: 'ProductRU'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('ProductEN'),
                dataIndex: 'ProductEN'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('BrandFlagAbbr'),
                dataIndex: 'BrandFlagAbbr'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('BrandFlag'),
                dataIndex: 'BrandFlag'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('SubmarkFlag'),
                dataIndex: 'SubmarkFlag'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('IngredientVariety'),
                dataIndex: 'IngredientVariety'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('ProductCategory'),
                dataIndex: 'ProductCategory'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('ProductType'),
                dataIndex: 'ProductType'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('MarketSegment'),
                dataIndex: 'MarketSegment'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('SupplySegment'),
                dataIndex: 'SupplySegment'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('FunctionalVariety'),
                dataIndex: 'FunctionalVariety'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('Size'),
                dataIndex: 'Size'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('BrandEssence'),
                dataIndex: 'BrandEssence'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('PackType'),
                dataIndex: 'PackType'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('GroupSize'),
                dataIndex: 'GroupSize'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('TradedUnitFormat'),
                dataIndex: 'TradedUnitFormat'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('ConsumerPackFormat'),
                dataIndex: 'ConsumerPackFormat'
            }, {
                text: l10n.ns('tpm', 'ActualProductsView').value('UOM_PC2Case'),
                dataIndex: 'UOM_PC2Case'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.product.Product',
        items: [{
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ZREP'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'EAN_Case',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('EAN_Case'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'EAN_PC',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('EAN_PC'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductRU',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ProductRU'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ProductEN'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandFlagAbbr',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('BrandFlagAbbr'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandFlag',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('BrandFlag'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SubmarkFlag',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('SubmarkFlag'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'IngredientVariety',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('IngredientVariety'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductCategory',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ProductCategory'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductType',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ProductType'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'MarketSegment',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('MarketSegment'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SupplySegment',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('SupplySegment'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'FunctionalVariety',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('FunctionalVariety'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('Size'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandEssence',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('BrandEssence'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'PackType',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('PackType'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'GroupSize',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('GroupSize'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'TradedUnitFormat',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('TradedUnitFormat'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ConsumerPackFormat',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ConsumerPackFormat'),
        }, {
            xtype: 'numberfield',
            name: 'UOM_PC2Case',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('UOM_PC2Case'),
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
        }]
    }]
});
