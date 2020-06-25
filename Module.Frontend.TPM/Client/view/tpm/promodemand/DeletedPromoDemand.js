Ext.define('App.view.tpm.promodemand.DeletedPromoDemand', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedpromodemand',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promodemand.DeletedPromoDemand',
            storeId: 'deletedpromodemandstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promodemand.DeletedPromoDemand',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [{ 
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
				dataIndex: 'DeletedDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, {
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
					valueField: 'BrandsegTechsub',
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
				dataIndex: 'Week'
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
			}, {
				xtype: 'numbercolumn',
				format: '0.00',
				text: l10n.ns('tpm', 'PromoDemand').value('Activity'),
				dataIndex: 'Activity'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promodemand.DeletedPromoDemand',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Account',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Account'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Discount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Week',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Week'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Baseline',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Baseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Uplift',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Uplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Incremental',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Incremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Activity',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Activity'),
        }]
    }]
});
