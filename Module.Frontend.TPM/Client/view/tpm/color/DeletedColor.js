Ext.define('App.view.tpm.color.DeletedColor', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedcolor',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Color'),

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
            model: 'App.model.tpm.color.DeletedColor',
            storeId: 'deletedcolorstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.color.DeletedColor',
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
                text: l10n.ns('tpm', 'Color').value('SystemName'),
                dataIndex: 'DisplayName',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div><div style="display:inline-block">{1}</div>', record.get('SystemName'), record.get('DisplayName'));
                }
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandName'),
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
                text: l10n.ns('tpm', 'Product').value('TechnologyName'),
                dataIndex: 'TechnologyName',
                filter: {
                    type: 'search',
                    selectorWidget: 'technology',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.technology.Technology',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.technology.Technology',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'Product').value('SubBrandName'),
                dataIndex: 'SubBrandName',
                filter: {
                    type: 'search',
                    selectorWidget: 'technology',
                    valueField: 'SubBrand',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.technology.Technology',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.technology.Technology',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.color.DeletedColor',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Color').value('SystemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DisplayName',
            fieldLabel: l10n.ns('tpm', 'Color').value('DisplayName')
        }, {
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
