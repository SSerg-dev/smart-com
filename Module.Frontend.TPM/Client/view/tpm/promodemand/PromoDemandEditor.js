Ext.define('App.view.tpm.promodemand.PromoDemandEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promodemandeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandName'),
            name: 'BrandTechId',
            selectorWidget: 'brandtech',
            valueField: 'Id',
            displayField: 'BrandName',
            onTrigger2Click: function () {
                var technology = this.up().down('[name=BrandTechName]');

                this.clearValue();
                technology.setValue(null);
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    var brandtech = field.up().down('[name=BrandTechName]');
                    var brandtechValue = newValue != undefined ? field.record.get('Name') : null;

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
                from: 'BrandName',
                to: 'BrandName'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandTechName'),
            name: 'BrandTechName',
        }, {
            xtype: 'searchfield',
            name: 'BaseClientObjectId',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Account'),
            selectorWidget: 'baseclienttreeview',
            valueField: 'BOI',
            displayField: 'ShortName',
            selectValidate: false,
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'ShortName',
                to: 'Account'
            }],
            validate: function () {
                var value = this.getValue();
                var valid = value != undefined && value != null;// && value.trim().length != 0;

                if (!valid)
                    this.markInvalid(this.blankText);
                else
                    this.clearInvalid();

                return valid;
            }
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicName'),
            name: 'MechanicId',
            selectorWidget: 'mechanic',
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'Mechanic',
            store: {
                type: 'simplestore',
                autoLoad: false,
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
            onTrigger3Click: function () {
                var mechanicType = this.up().down('[name=MechanicTypeId]');
                var discount = this.up().down('[name=Discount]');

                this.clearValue();
                mechanicType.setValue(null);
                mechanicType.setReadOnly(true);
                discount.setValue(null);
                discount.setReadOnly(true);
            },
            mapping: [{
                from: 'Name',
                to: 'MechanicName'
            }]
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicTypeName'),
            name: 'MechanicTypeId',
            selectorWidget: 'mechanictype',
            valueField: 'Id',
            displayField: 'Name',           
            entityType: 'MechanicType',
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'simplestore',
                autoLoad: false,
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
            onTrigger3Click: function () {                
                var discount = this.up().down('[name=Discount]');

                this.clearValue();
                discount.setValue(null);
            },
            mapping: [{
                from: 'Name',
                to: 'MechanicTypeName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'Discount',
            allowDecimals: false,
            allowExponential: false,
            minValue: 0,
            maxValue: 100,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Discount'),
        }, {
            xtype: 'marsdatefield',
            name: 'Week',
            weekRequired: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Week'),
        }, {
            xtype: 'numberfield',
            name: 'Baseline',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Baseline'),
        }, {
            xtype: 'numberfield',
            name: 'Uplift',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 100,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Uplift'),
        }, {
            xtype: 'numberfield',
            name: 'Incremental',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Incremental'),
        }, {
            xtype: 'numberfield',
            name: 'Activity',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Activity'),
        }]
    }
});
