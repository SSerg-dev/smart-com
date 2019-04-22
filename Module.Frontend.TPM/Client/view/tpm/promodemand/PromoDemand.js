Ext.define('App.view.tpm.promodemand.PromoDemand', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promodemand',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoDemand'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promodemand.PromoDemand',
            storeId: 'promodemandstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promodemand.PromoDemand',
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
                minWidth: 110
            },
            items: [{
				text:  l10n.ns('tpm', 'PromoDemand').value('BrandName'),
				dataIndex: 'BrandName',
				filter: {
				    type: 'search',
				    selectorWidget: 'brand',
				    valueField: 'Name',
				    store: {
				        type: 'directorystore',
				        model: 'App.model.tpm.brand.Brand',
				        extendedFilter: {
				            xclass: 'App.ExtFilterContext',
				            supportedModels: [{
				                xclass: 'App.ExtSelectionFilterModel',
				                model: 'App.model.tpm.brand.Brand',
				                modelId: 'efselectionmodel'
				            }, {
				                xclass: 'App.ExtTextFilterModel',
				                modelId: 'eftextmodel'
				            }]
				        }
				    }
				}
			}, {
				text:  l10n.ns('tpm', 'PromoDemand').value('BrandTechName'),
				dataIndex: 'BrandTechName',
				filter: {
				    type: 'search',
				    selectorWidget: 'brandtech',
				    valueField: 'Name',
				    store: {
				        type: 'directorystore',
				        model: 'App.model.tpm.brandtech.BrandTech',
				        extendedFilter: {
				            xclass: 'App.ExtFilterContext',
				            supportedModels: [{
				                xclass: 'App.ExtSelectionFilterModel',
				                model: 'App.model.tpm.brandtech.BrandTech',
				                modelId: 'efselectionmodel'
				            }, {
				                xclass: 'App.ExtTextFilterModel',
				                modelId: 'eftextmodel'
				            }]
				        }
				    }
				}
			}, {
				text: l10n.ns('tpm', 'PromoDemand').value('Account'),
				dataIndex: 'Account',
                filter: {
				    type: 'search',
                    selectorWidget: 'baseclienttreeview',
                    valueField: 'ShortName',
				    store: {
				        type: 'directorystore',
                        model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
				        extendedFilter: {
				            xclass: 'App.ExtFilterContext',
				            supportedModels: [{
				                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
				                modelId: 'efselectionmodel'
				            }, {
				                xclass: 'App.ExtTextFilterModel',
				                modelId: 'eftextmodel'
				            }]
				        }
                    }
				}
			}, {
			    xtype: 'numbercolumn',
			        format: '0',
			        text: l10n.ns('tpm', 'PromoDemand').value('BaseClientObjectId'),
			        dataIndex: 'BaseClientObjectId'
			}, {
				text:  l10n.ns('tpm', 'PromoDemand').value('MechanicName'),
				dataIndex: 'MechanicName',
				filter: {
				    type: 'search',
				    selectorWidget: 'mechanic',
				    valueField: 'Name',
				    store: {
				        type: 'directorystore',
				        model: 'App.model.tpm.mechanic.Mechanic',
				        extendedFilter: {
				            xclass: 'App.ExtFilterContext',
				            supportedModels: [{
				                xclass: 'App.ExtSelectionFilterModel',
				                model: 'App.model.tpm.mechanic.Mechanic',
				                modelId: 'efselectionmodel'
				            }, {
				                xclass: 'App.ExtTextFilterModel',
				                modelId: 'eftextmodel'
				            }]
				        }
				    }
				}
			}, {
				text:  l10n.ns('tpm', 'PromoDemand').value('MechanicTypeName'),
				dataIndex: 'MechanicTypeName',
				filter: {
				    type: 'search',
				    selectorWidget: 'mechanictype',
				    valueField: 'Name',
				    store: {
				        type: 'directorystore',
                        model: 'App.model.tpm.mechanictype.MechanicType',
				        extendedFilter: {
				            xclass: 'App.ExtFilterContext',
				            supportedModels: [{
				                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.mechanictype.MechanicType',
				                modelId: 'efselectionmodel'
				            }, {
				                xclass: 'App.ExtTextFilterModel',
				                modelId: 'eftextmodel'
				            }]
				        }
				    }
				}
			}, {
				xtype: 'numbercolumn',
				format: '0',
				text: l10n.ns('tpm', 'PromoDemand').value('Discount'),
				dataIndex: 'Discount'
			}, {
				text: l10n.ns('tpm', 'PromoDemand').value('Week'),
				dataIndex: 'Week',
                filter: {
				    xtype: 'marsdatefield',
                    operator: 'like',
                    validator: function (value) {
                        // дает возможность фильтровать только по году
                        return true;
                    }
                }
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
				text: l10n.ns('tpm', 'PromoDemand').value('Baseline'),
				dataIndex: 'Baseline'
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
				text: l10n.ns('tpm', 'PromoDemand').value('Uplift'),
				dataIndex: 'Uplift'
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
				text: l10n.ns('tpm', 'PromoDemand').value('Incremental'),
				dataIndex: 'Incremental'
			}, 			{
				xtype: 'numbercolumn',
				format: '0.00',
				text: l10n.ns('tpm', 'PromoDemand').value('Activity'),
				dataIndex: 'Activity'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promodemand.PromoDemand',
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
            xtype: 'textfield',
            name: 'BaseClientBOI',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Account'),
            selectorWidget: 'baseclienttreeview',
            valueField: 'BOI',
            displayField: 'Account',
            entityType: 'BaseClientTreeView',
            store: {
                type: 'simplestore',
                autoLoad: false,
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
                from: 'ResultNameStr',
                to: 'Account'
            }]
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
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Incremental'),
        }, {
            xtype: 'numberfield',
            name: 'Activity',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Activity'),
        }]
    }]
});
