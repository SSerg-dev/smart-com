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
                allowBlank: false,
                allowOnlyWhitespace: false,
                valueField: 'Id',
                displayField: 'BrandsegTechsub',
                onTrigger2Click: function () {
                    var technology = this.up().down('[name=BrandTechId]');
    
                    this.clearValue();
                    technology.setValue(null);
                },
                listeners: {
                    afterrender: function (field) {
                        var comboSize = Ext.ComponentQuery.query('#SizeComboBox')[0];
                        if (!field.value) {
                            field.value = null;

                            comboSize.setDisabled(true);
                        } else {
                            comboSize.setDisabled(false);
                            
                            var planPostPromoEffectController = App.app.getController('tpm.planpostpromoeffect.PlanPostPromoEffect');
                            planPostPromoEffectController.getBrandTechCodeById(field.value)
                                .then(function (data) {
                                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                                    if (result.success == true) {
                                        result = Ext.JSON.decode(result.data);
                                        planPostPromoEffectController.getBrandTechSizes(result.BrandTech_code);
                                    }
                                })
                                .fail(function (data) {
                                    App.Notify.pushError(data.message);
                                });
                        }
                    },
                    change: function (field, newValue, oldValue) {
                        var brandtech = field.up().down('[name=BrandTechId]');
                        var brandtechValue = newValue ? field.record.get('BrandsegTechsub') : null;
                        brandtech.setValue(brandtechValue);
                        
                        var comboSize = Ext.ComponentQuery.query('#SizeComboBox')[0];
                        comboSize.setDisabled(Ext.isEmpty(brandtechValue));
                        
                        if (Ext.isEmpty(newValue)) {
                            comboSize.clearValue();
                        }

                        var brandtechCode = newValue ? field.record.get('BrandTech_code') : null;
                        if (brandtechCode != null) {
                            var planPostPromoEffectController = App.app.getController('tpm.planpostpromoeffect.PlanPostPromoEffect');
                            planPostPromoEffectController.getBrandTechSizes(brandtechCode);
                        }
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
                xtype: 'combobox',
                editable: false,
                name: 'Size',
                id: 'SizeComboBox',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('Size'),
                valueField: 'size',
                displayField: 'size',
                queryMode: 'local',
                selectOnFocus: true,
                allowBlank: false,
                allowOnlyWhitespace: false,
                store: Ext.create('Ext.data.Store', {
                    fields: ['size']
                }),
                mapping: [{
                    from: 'size',
                    to: 'size'
                }]
            }, {
                xtype: 'searchcombobox',
                editable: false,
                name: 'DiscountRangeId',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DiscountRangeName'),
                selectorWidget: 'discountrange',
                valueField: 'Id',
                displayField: 'Name',
                selectOnFocus: true,
                entityType: 'DiscountRange',
                allowBlank: false,
                allowOnlyWhitespace: false,
                store: {
                    type: 'simplestore',
                    autoLoad: true,
                    model: 'App.model.tpm.discountrange.DiscountRange',
                    sorters: [{
                        property: 'MinValue',
                        direction: 'ASC'
                    }, {
                        property: 'MaxValue',
                        direction: 'ASC'
                    }]
                },
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.discountrange.DiscountRange',
                        modelId: 'efselectionmodel'
                    }]
                },
                mapping: [{
                    from: 'Name',
                    to: 'DiscountRangeName'
                }]
            }, {
                xtype: 'searchcombobox',
                itemId: 'duration-range',
                editable: false,
                name: 'DurationRangeId',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DurationRangeName'),
                selectorWidget: 'durationrange',
                valueField: 'Id',
                displayField: 'Name',
                selectOnFocus: true,
                entityType: 'DurationRange',
                allowBlank: false,
                allowOnlyWhitespace: false,
                store: {
                    type: 'simplestore',
                    autoLoad: true,
                    model: 'App.model.tpm.durationrange.DurationRange',
                    sorters: [{
                        property: 'MinValue',
                        direction: 'ASC'
                    }, {
                        property: 'MaxValue',
                        direction: 'ASC'
                    }]
                },
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.durationrange.DurationRange',
                        modelId: 'efselectionmodel'
                    }]
                },
                mapping: [{
                    from: 'Name',
                    to: 'DurationRangeName'
                }]                
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
