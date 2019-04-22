Ext.define('App.view.tpm.tradeinvestment.TradeInvestmentEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.tradeinvestmenteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('StartDate'),
            readOnly: false,
            editable: true,
            format: 'd.m.Y',
            listeners: {
                change: function (newValue, oldValue) {
                    var toDate = this.up('form').down('[name=EndDate]');
                    toDate.setMinValue(newValue.getValue());
                }
            }
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('EndDate'),
            readOnly: false,
            editable: true,
            format: 'd.m.Y',
            listeners: {
                change: function (newValue, oldValue) {
                    var fromDate = this.up('form').down('[name=StartDate]');
                    fromDate.setMaxValue(newValue.getValue());
                }
            }
        }, {
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        },
        {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('BrandTechName'),
            name: 'BrandTechId',
            selectorWidget: 'brandtech',
            allowBlank: true,
            allowOnlyWhitespace: true,
            valueField: 'Id',
            displayField: 'Name',
            onTrigger2Click: function () {
                var technology = this.up().down('[name=BrandTechId]');

                this.clearValue();
                technology.setValue(null);
            },
            listeners: {
                afterrender: function (field) {
                    if (!field.value) {
                        field.value = null;
                    }
                },
                change: function (field, newValue, oldValue) {
                    var brandtech = field.up().down('[name=BrandTechId]');
                    var brandtechValue = newValue ? field.record.get('Name') : null;

                    brandtech.setValue(brandtechValue);
                }
            },
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.brandtech.BrandTech',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BrandTechName'
            }]
        }, {
            xtype: 'textfield',            name: 'TIType',            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('TIType'),
        }, {
            xtype: 'textfield',            name: 'TISubType',            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('TISubType'),
        }, {
            xtype: 'numberfield',
            name: 'SizePercent',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('SizePercent'),
            minValue: 0,
            maxValue: 100,
            readOnly: false,
            allowBlank: false,
            listeners: {
                change: function (newValue, oldValue) {
                    if (newValue > 100) {
                        this.setValue(oldValue);
                    }
                },
            }
        }, {
            xtype: 'booleancombobox',
            store: {
                type: 'booleannonemptystore'
            },
            name: 'MarcCalcROI',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('MarcCalcROI')
        }, {
            xtype: 'booleancombobox',
            store: {
                type: 'booleannonemptystore'
            },
            name: 'MarcCalcBudgets',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('MarcCalcBudgets')
        }]
    }
});


