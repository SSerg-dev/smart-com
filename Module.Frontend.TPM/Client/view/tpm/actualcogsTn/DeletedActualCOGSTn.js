Ext.define('App.view.tpm.actualcogsTn.DeletedActualCOGSTn', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedactualcogs',
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
            model: 'App.model.tpm.actualcogsTn.DeletedActualCOGSTn',
            storeId: 'deletedactualcogsstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.actualcogsTn.DeletedActualCOGSTn',
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
            items: [
                {
                    text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                    dataIndex: 'DeletedDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'ActualCOGSTn').value('StartDate'),
                    dataIndex: 'StartDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                }, {
                    text: l10n.ns('tpm', 'ActualCOGSTn').value('EndDate'),
                    dataIndex: 'EndDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                }, {
                    text: l10n.ns('tpm', 'ActualCOGSTn').value('ClientTreeFullPathName'),
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
                    text: l10n.ns('tpm', 'ActualCOGSTn').value('ClientTreeObjectId'),
                    dataIndex: 'ClientTreeObjectId'
                }, {
                    text: l10n.ns('tpm', 'ActualCOGSTn').value('BrandTechName'),
                    dataIndex: 'BrandTechName',
                    width: 120,
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
                    text: l10n.ns('tpm', 'ActualCOGSTn').value('Volume'),
                    dataIndex: 'Volume'
                }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.actualcogsTn.DeletedActualCOGSTn',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'ActualCOGSTn').value('StartDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'ActualCOGSTn').value('EndDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'ActualCOGSTn').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'ActualCOGSTn').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'ActualCOGSTn').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Volume',
            fieldLabel: l10n.ns('tpm', 'ActualCOGSTn').value('Volume')
        }]
    }]
});
