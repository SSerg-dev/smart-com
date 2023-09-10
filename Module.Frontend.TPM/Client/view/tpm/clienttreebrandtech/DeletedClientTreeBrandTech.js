Ext.define('App.view.tpm.clienttreebrandtech.DeletedClientTreeBrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedclienttreebrandtech',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientTreeBrandTech'),

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
            model: 'App.model.tpm.clienttreebrandtech.DeletedClientTreeBrandTech',
            storeId: 'deletedclienttreebrandtechesstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.clienttreebrandtech.DeletedClientTreeBrandTech',
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
            },
            {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('ParentClientTreeDemandCode'),
                dataIndex: 'ParentClientTreeDemandCode',
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'DemandCode',
                    displayField: 'DemandCode',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },
                }
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId',
                minWidth: 200
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeName'),
                dataIndex: 'ClientTreeName',
                minWidth: 200,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'Name',
                    displayField: 'Name',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },
                }
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('BrandTechName'),
                dataIndex: 'BrandTechName',
                filter: {
                    xtype: 'fsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'brandtech',
                    valueField: 'BrandsegTechsub',
                    displayField: 'BrandsegTechsub',
                    multiSelect: true,
                    operator: 'conts',
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
                    },
                }
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('Share'),
                dataIndex: 'Share',
                renderer: function (value, meta) {
                    return value.toString().replace('.', ',')
                },
                filter: {
                    xtype: 'numberfield',
                    submitLocaleSeparator: false,
                    decimalPrecision: 15,
                    allowDecimals: true,
                    decimalSeparator: ',',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    operator: 'eq'
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.clienttreebrandtech.DeletedClientTreeBrandTech',
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandName'),
            name: 'BrandName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('TechnologyName'),
            name: 'TechnologyName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrandName'),
            name: 'SubBrandName',
        }]
    }]
});
