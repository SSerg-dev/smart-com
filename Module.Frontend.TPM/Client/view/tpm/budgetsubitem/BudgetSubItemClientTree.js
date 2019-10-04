Ext.define('App.view.tpm.budgetsubitem.BudgetSubItemClientTree', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.budgetsubitemclienttree',
    title: l10n.ns('tpm', 'BudgetSubItemClientTree').value('BudgetSubItemClientTree'),

    dockedItems: [{
        xtype: 'addonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'associateddirectorystore',
            model: 'App.model.tpm.budgetsubitem.BudgetSubItemClientTree',
            storeId: 'budgetsubitemclienttreestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.budgetsubitem.BudgetSubItemClientTree',
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
                text: l10n.ns('tpm', 'BudgetSubItemClientTree').value('ClientTreeFullPathName'),
                dataIndex: 'ClientTreeFullPathName',
                filter: {
                    type: 'string',
                    operator: 'conts',
                    onChange: function (value) {
                        if (value) {
                            this.setValue(value.replace(//g, '>'));
                        }
                    },
                },
            }, {
                text: l10n.ns('tpm', 'BudgetSubItemClientTree').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId',
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.budgetsubitem.BudgetSubItemClientTree',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItemClientTree').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItemClientTree').value('ClientTreeObjectId')
        }]
    }]

});