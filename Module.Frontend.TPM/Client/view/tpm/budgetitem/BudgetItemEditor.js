Ext.define('App.view.tpm.budgetitem.BudgetItemEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.budgetitemeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('BudgetName'),
            name: 'BudgetId',
            selectorWidget: 'budget',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budget.Budget',
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
                to: 'BudgetName'
            }]
        }, {
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('Name'),
        }, {
            xtype: 'circlecolorfield',
            name: 'ButtonColor',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('ButtonColor'),
            allowBlank: true,
            allowOnlyWhitespace: true,
            listeners: {
                select: function () {                    var a = document.getElementById(this.id + "-inputEl");
                    if (this.setOnChange == "background") {                        a.style.backgroundColor = this.getValue();                        a.style.color = this.contrastColor(this.getValue())                    } else {                        if (this.setOnChange == "color") {                            a.style.color = this.getValue()                        } else {                            if (typeof this.setOnChange == "function") {                                this.setOnChange()                            }                        }                    }                },
                change: function (trigger, newValue, oldValue) {                    var a = document.getElementById(this.id + "-inputEl");                    if (a) {                        a.style.backgroundColor = this.getValue();                        a.style.color = this.contrastColor(this.getValue())                    }                },                afterrender: function () {                    !this.getValue() && this.setValue(null);                    this.fireEvent("select")                }
            }
        }]
    }
});
