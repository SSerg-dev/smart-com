Ext.define('App.view.tpm.nonpromosupport.DeletedNonPromoSupport', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletednonpromosupport',
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
            model: 'App.model.tpm.nonpromosupport.DeletedNonPromoSupport',
            storeId: 'deletednonpromosupportstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonpromosupport.DeletedNonPromoSupport',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
                direction: 'DESC'
            }],
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
                text: l10n.ns('tpm', 'NonPromoSupport').value('Number'),
                dataIndex: 'Number'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('ClientTreeFullPathName'),
                dataIndex: 'ClientTreeFullPathName',
                minWidth: 200,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'FullPathName',
                    displayField: 'FullPathName',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },
                },
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('BrandTechName'),
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
                text: l10n.ns('tpm', 'NonPromoSupport').value('NonPromoEquipmentEquipmentType'),
                dataIndex: 'NonPromoEquipmentEquipmentType',
                filter: {
                    type: 'search',
					selectorWidget: 'nonpromoequipment',
					valueField: 'EquipmentType',
                    store: {
                        type: 'directorystore',
						model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
								model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('PlanQuantity'),
                dataIndex: 'PlanQuantity'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('ActualQuantity'),
                dataIndex: 'ActualQuantity'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('PlanCostTE'),
                dataIndex: 'PlanCostTE'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('ActualCostTE'),
                dataIndex: 'ActualCostTE'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'NonPromoSupport').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        }
    }, {
            xtype: 'editabledetailform',
            itemId: 'detailform',
            model: 'App.model.tpm.nonpromosupport.DeletedNonPromoSupport',
            items: [ ]
        }]
});