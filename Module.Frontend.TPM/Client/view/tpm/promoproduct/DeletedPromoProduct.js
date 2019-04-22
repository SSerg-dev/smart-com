Ext.define('App.view.tpm.promoproduct.DeletedPromoProduct', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedpromoproduct',
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
            model: 'App.model.tpm.promoproduct.DeletedPromoProduct',
            storeId: 'deletedactualstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoproduct.DeletedPromoProduct',
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
                text: l10n.ns('tpm', 'PromoProduct').value('EAN'),
                dataIndex: 'EAN'
            }, {
				xtype: 'numbercolumn',
				format: '0',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCQty'),
                dataIndex: 'ActualProductPCQty'
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductQty'),
                dataIndex: 'ActualProductQty'
			}, {
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductUOM'),
                dataIndex: 'ActualProductUOM'
            }, {
				xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductShelfPrice'),
                dataIndex: 'ActualProductShelfPrice'
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCLSV'),
                dataIndex: 'ActualProductPCLSV'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promoproduct.DeletedPromoProduct',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('EAN'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductPCQty',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCQty'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductQty',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductQty'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductUOM',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductUOM'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductShelfPrice',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductShelfPrice'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductPCLSV',
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCLSV'),
        }]
    }]
});