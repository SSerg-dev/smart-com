Ext.define('App.view.tpm.promosales.PromoSalesEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.promosaleseditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    afterWindowShow: function () {
        this.down('textfield[name=Name]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Number'),
            name: 'Number'
        }, {
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Name'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('MechanicName'),
            name: 'MechanicId',
            selectorWidget: 'mechanic',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.mechanic.Mechanic',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanic.Mechanic',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'MechanicName'
            }]
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('MechanicTypeName'),
            name: 'MechanicTypeId',
            selectorWidget: 'mechanictype',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.mechanictype.MechanicType',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanictype.MechanicType',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'MechanicTypeName'
            }]
        }, {
            xtype: 'numberfield',
            minValue: 0,
            maxValue: 100,
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('MechanicDiscount'),
            name: 'MechanicDiscount'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientId',
            selectorWidget: 'client',
            valueField: 'Id',
            displayField: 'CommercialSubnetCommercialNetName',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.client.Client',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.client.Client',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'CommercialSubnetCommercialNetName',
                to: 'ClientCommercialSubnetCommercialNetName'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('PromoStatusName'),
            name: 'PromoStatusId',
            selectorWidget: 'promostatus',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.promostatus.PromoStatus',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.promostatus.PromoStatus',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'PromoStatusName'
            }]
        }, {
            xtype: 'datefield', allowBlank: false, allowOnlyWhitespace: false,
            name: 'StartDate',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('StartDate'),
        }, {
            xtype: 'datefield', allowBlank: false, allowOnlyWhitespace: false,
            name: 'EndDate',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('EndDate'),
        }, {
            xtype: 'datefield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'DispatchesStart',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('DispatchesStart'),
        }, {
            xtype: 'datefield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'DispatchesEnd',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('DispatchesEnd'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BudgetItemBudgetName'),
            name: 'BudgetItemBudgetName'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BudgetItemName'),
            name: 'BudgetItemId',
            selectorWidget: 'budgetitem',
            valueField: 'Id',
            displayField: 'Name',
            needUpdateMappings: true,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budgetitem.BudgetItem',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.budgetitem.BudgetItem',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BudgetItemName'
            }, {
                from: 'BudgetName',
                to: 'BudgetItemBudgetName'
            }]
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: false, allowOnlyWhitespace: false,
            name: 'Plan',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Plan'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: false, allowOnlyWhitespace: false,
            name: 'Fact',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Fact'),
        }]
    }
});
