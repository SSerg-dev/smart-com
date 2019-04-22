Ext.define('App.view.tpm.nonenego.DeletedNoneNego', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletednonenego',
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
            model: 'App.model.tpm.nonenego.DeletedNoneNego',
            storeId: 'deletednonenegostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonenego.DeletedNoneNego',
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
                text: l10n.ns('tpm', 'NoneNego').value('ClientTreeFullPathName'),
                dataIndex: 'ClientTreeFullPathName',
                filter: {
                    type: 'string',
                    operator: 'conts'
                },
                minWidth: 200,
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ProductTreeFullPathName'),
                dataIndex: 'ProductTreeFullPathName',
                filter: {
                    type: 'string',
                    operator: 'conts'
                },
                minWidth: 200,
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ProductTreeObjectId'),
                dataIndex: 'ProductTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('MechanicName'),
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
                text: l10n.ns('tpm', 'NoneNego').value('MechanicTypeName'),
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
                text: l10n.ns('tpm', 'NoneNego').value('Discount'),
                dataIndex: 'Discount'
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('FromDate'),
                dataIndex: 'FromDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ToDate'),
                dataIndex: 'ToDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('CreateDate'),
                dataIndex: 'CreateDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.nonenego.DeletedNoneNego',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientHierarchy',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientHierarchy')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductHierarchy',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ProductHierarchy')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicTypeName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicTypeName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('Discount')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FromDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('FromDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ToDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ToDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('CreateDate')
        }]
    }]
});