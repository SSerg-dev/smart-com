Ext.define('App.view.tpm.planpostpromoeffect.PlanPostPromoEffectEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.planpostpromoeffecteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('ClientTreeFullPathName'),
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
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('BrandTechName'),
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
                xtype: 'textfield',
                dataIndex: 'Size',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('Size')
            }, {
                xtype: 'combobox',
                editable: false,
                dataIndex: 'DiscountRangeId',
                displayField: 'Name',
                entityType: 'DiscountRange',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DiscountRangeName'),
                queryMode: 'local',
                valueField: 'Id',
                forceSelection: true,
                allowBlank: false,
                allowOnlyWhitespace: false,
                store: {
                    type: 'simplestore',
                    autoLoad: false,
                    model: 'App.model.tpm.discountrange.DiscountRange',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.discountrange.DiscountRange',
                            modelId: 'efselectionmodel'
                        }]
                    }
                }
            }, {
                xtype: 'combobox',
                editable: false,
                dataIndex: 'DurationRangeId',
                displayField: 'Name',
                entityType: 'DurationRange',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DurationRangeName'),
                queryMode: 'local',
                valueField: 'Id',
                forceSelection: true,
                allowBlank: false,
                allowOnlyWhitespace: false,
                store: {
                    type: 'simplestore',
                    autoLoad: false,
                    model: 'App.model.tpm.durationrange.DurationRange',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.durationrange.DurationRange',
                            modelId: 'efselectionmodel'
                        }]
                    }
                }
            }, {
                xtype: 'numberfield',
                name: 'PlanPostPromoEffectW1',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW1')
            }, {
                xtype: 'numberfield',
                name: 'PlanPostPromoEffectW2',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW2')
            }
        ]
    }
});
