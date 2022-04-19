Ext.define('App.view.tpm.plancogsTn.PlanCOGSEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.cogseditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'planCOGSTn').value('StartDate'),
            readOnly: false,
            editable: false,
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
            fieldLabel: l10n.ns('tpm', 'planCOGSTn').value('EndDate'),
            readOnly: false,
            editable: false,
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
            fieldLabel: l10n.ns('tpm', 'planCOGSTn').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            clientTreeIdValid: true,
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            listeners:
            {
                change: function (field, newValue, oldValue) {
                    if (field && field.record && field.record.data.ObjectId === 5000000) {
                        this.clientTreeIdValid = false;
                    } else {
                        this.clientTreeIdValid = true;
                    }
                }
            },
            validator: function () {
                if (!this.clientTreeIdValid) {
                    return l10n.ns('core', 'customValidators').value('clientTreeSelectRoot')
                }
                return true;
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        },
        {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'planCOGSTn').value('BrandTechName'),
            name: 'BrandTechId',
            selectorWidget: 'brandtech',
            allowBlank: true,
            allowOnlyWhitespace: true,
            valueField: 'Id',
            displayField: 'BrandsegTechsub',
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
                    var brandtechValue = newValue ? field.record.get('BrandsegTechsub') : null;

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
                from: 'BrandsegTechsub',
                to: 'BrandTechName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'LSVpercent',
            fieldLabel: l10n.ns('tpm', 'planCOGSTn').value('LSVpercent'),
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
        }
        ]
    }
});
