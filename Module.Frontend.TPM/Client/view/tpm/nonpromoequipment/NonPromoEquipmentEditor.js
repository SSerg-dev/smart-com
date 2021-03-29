Ext.define('App.view.tpm.nonpromoequipment.NonPromoEquipmentEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.nonpromoequipmenteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        id: 'nonpromoequipmenteditorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'EquipmentType',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
        }, {
            xtype: 'textfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
            allowBlank: true,
            allowOnlyWhitespace: true
        }, {
            //xtype: 'combobox',
            //name: 'BudgetItemId',
            //fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('BudgetItemName'),
            //valueField: 'Id',
            //displayField: 'Name',
            //entityType: 'BudgetItem',
            //store: {
            //    type: 'simplestore',
            //    autoLoad: true,
            //    model: 'App.model.tpm.budgetitem.BudgetItem',
            //},
            //listeners: {
            //    change: function (field, newValue, oldValue) {
            //        if (newValue != oldValue) {
            //            var editor = field.up();
            //            if (editor) {
            //                editor.nonPromoEquipmentId = newValue;
            //            }
            //        }
            //    }
            //},
            //mapping: [{
            //    from: 'Name',
            //    to: 'BudgetItemName'
            //}]
                xtype: 'searchfield',
                fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('BudgetItemName'),
                name: 'BudgetItemId',
                selectorWidget: 'budgetitemshort',
                valueField: 'Id',
                editable: false,
                displayField: 'Name',
                store: {
                    type: 'directorystore',
                    model: 'App.model.tpm.budgetitem.BudgetItem',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.budget.Budget',
                            modelId: 'efselectionmodel'
                        }]
                    }
                },
                mapping: [{
                    from: 'Name',
                    to: 'BudgetItemName'
                }]
        },
        ]
    }
});     