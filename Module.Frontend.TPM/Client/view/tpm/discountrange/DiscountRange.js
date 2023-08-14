Ext.define('App.view.tpm.discountrange.DiscountRange', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.discountrange',
    title: l10n.ns('tpm', 'compositePanelTitles').value('DiscountRange'),

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
            model: 'App.model.tpm.discountrange.DiscountRange',
            storeId: 'discountrangestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.discountrange.DiscountRange',
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
                text: l10n.ns('tpm', 'DiscountRange').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'DiscountRange').value('MinValue'),
                dataIndex: 'MinValue'
            }, {
                text: l10n.ns('tpm', 'DiscountRange').value('MaxValue'),
                dataIndex: 'MaxValue'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.discountrange.DiscountRange',
        items: [{
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'DiscountRange').value('Name'),
            name: 'Name',
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'DiscountRange').value('MinValue'),
            name: 'MinValue',
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'DiscountRange').value('MaxValue'),
            name: 'MaxValue',
        }]
    }]
});
