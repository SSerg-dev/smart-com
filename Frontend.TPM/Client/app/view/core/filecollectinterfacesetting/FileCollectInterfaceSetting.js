Ext.define('App.view.core.filecollectinterfacesetting.FileCollectInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.filecollectinterfacesetting',
    title: l10n.ns('core', 'compositePanelTitles').value('FileCollectInterfaceSettingTitle'),

    dockedItems: [{
        xtype: 'standarddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',

        store: {
            type: 'directorystore',
            model: 'App.model.core.filecollectinterfacesetting.FileCollectInterfaceSetting',
            storeId: 'filecollectinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.filecollectinterfacesetting.FileCollectInterfaceSetting',
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
                flex: 1
            },
            items: [{
                text: l10n.ns('core', 'FileCollectInterfaceSetting').value('InterfaceName'),
                dataIndex: 'InterfaceName',
                filter: {
                    type: 'search',
                    selectorWidget: 'interface',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.core.interface.Interface',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.core.interface.Interface',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('core', 'FileCollectInterfaceSetting').value('SourcePath'),
                dataIndex: 'SourcePath'
            }, {
                text: l10n.ns('core', 'FileCollectInterfaceSetting').value('SourceFileMask'),
                dataIndex: 'SourceFileMask'
            }, {
                text: l10n.ns('core', 'FileCollectInterfaceSetting').value('CollectHandler'),
                dataIndex: 'CollectHandler'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('InterfaceName'),
            name: 'InterfaceName',
            type: 'search',
            selectorWidget: 'interface',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.core.interface.Interface',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.core.interface.Interface',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'InterfaceName'
            }]
        }, {
            xtype: 'textfield',
            name: 'SourcePath',
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('SourcePath')
        }, {
            xtype: 'textfield',
            name: 'SourceFileMask',
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('SourceFileMask')
        }, {
            xtype: 'textfield',
            name: 'CollectHandler',
            fieldLabel: l10n.ns('core', 'FileCollectInterfaceSetting').value('CollectHandler')
        }]
    }]

});