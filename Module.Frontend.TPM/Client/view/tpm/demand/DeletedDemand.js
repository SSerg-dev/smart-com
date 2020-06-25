Ext.define('App.view.tpm.demand.DeletedDemand', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deleteddemand',
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
            model: 'App.model.tpm.demand.DeletedDemand',
            storeId: 'deleteddemandstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.demand.DeletedDemand',
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
                text: l10n.ns('tpm', 'Demand').value('Number'),
                dataIndex: 'Number'
            }, {
                text: l10n.ns('tpm', 'Demand').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'Demand').value('ClientCommercialSubnetCommercialNetName'),
                dataIndex: 'ClientCommercialSubnetCommercialNetName',
                filter: {
                    type: 'search',
                    selectorWidget: 'client',
                    valueField: 'CommercialSubnetCommercialNetName',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.client.Client',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.client.Client',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'Demand').value('BrandName'),
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
                text: l10n.ns('tpm', 'Demand').value('BrandTechName'),
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
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Demand').value('StartDate'),
                dataIndex: 'StartDate'
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Demand').value('EndDate'),
                dataIndex: 'EndDate'
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Demand').value('DispatchesStart'),
                dataIndex: 'DispatchesStart'
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Demand').value('DispatchesEnd'),
                dataIndex: 'DispatchesEnd'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('PlanBaseline'),
                dataIndex: 'PlanBaseline'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('PlanDuration'),
                dataIndex: 'PlanDuration'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('PlanUplift'),
                dataIndex: 'PlanUplift'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('PlanIncremental'),
                dataIndex: 'PlanIncremental'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('PlanActivity'),
                dataIndex: 'PlanActivity'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('PlanSteal'),
                dataIndex: 'PlanSteal'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('FactBaseline'),
                dataIndex: 'FactBaseline'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('FactDuration'),
                dataIndex: 'FactDuration'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('FactUplift'),
                dataIndex: 'FactUplift'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('FactIncremental'),
                dataIndex: 'FactIncremental'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('FactActivity'),
                dataIndex: 'FactActivity'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Demand').value('FactSteal'),
                dataIndex: 'FactSteal'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.demand.HistoricalDemand',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Number'),
            name: 'Number'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientCommercialSubnetCommercialNetName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesStart',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesStart'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesEnd'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanBaseline',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanBaseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanDuration',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanDuration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanUplift',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanUplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanIncremental',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanIncremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanActivity',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanActivity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanSteal',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanSteal'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactBaseline',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactBaseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactDuration',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactDuration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactUplift',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactUplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactIncremental',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactIncremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactActivity',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactActivity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactSteal',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactSteal'),
        }]
    }]
});
