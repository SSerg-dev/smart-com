Ext.define('App.view.tpm.budgetsubitem.BudgetSubItemEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.budgetsubitemeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    afterWindowShow: function (scope, isCreating) {
        scope.down('searchfield[name=BudgetItemId]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetName'),
            name: 'BudgetName',
            select: function () { return false; }
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetItemName'),
            name: 'BudgetItemId',
            selectorWidget: 'budgetitem',
            valueField: 'Id',
            displayField: 'Name',
            onTrigger2Click: function () {
                var budget = this.up().down('[name=BudgetName]');

                this.clearValue();
                budget.setValue(null);
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    var budget = field.up().down('[name=BudgetName]');
                    var budgetValue = newValue != undefined ? field.record.get('BudgetName') : null;

                    budget.setValue(budgetValue);
                }
            },
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
        }, {
            xtype: 'textfield',
            name: 'Name',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Name'),
        }, {
            xtype: 'textfield',
            name: 'Description_ru',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Description_ru'),
            allowBlank: true,
            allowOnlyWhitespace: true
        }]
    }
});
