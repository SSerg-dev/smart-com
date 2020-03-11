Ext.define('App.view.core.csvprocessinterfacesetting.CSVProcessInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.csvprocessinterfacesetting',
    title: l10n.ns('core', 'compositePanelTitles').value('CSVProcessInterfaceSettingTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',

        store: {
            type: 'directorystore',
            model: 'App.model.core.csvprocessinterfacesetting.CSVProcessInterfaceSetting',
            storeId: 'csvprocessinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.csvprocessinterfacesetting.CSVProcessInterfaceSetting',
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
                text: l10n.ns('core', 'CSVProcessInterfaceSetting').value('InterfaceName'),
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
                text: l10n.ns('core', 'CSVProcessInterfaceSetting').value('Delimiter'),
                dataIndex: 'Delimiter'
            }, {
                text: l10n.ns('core', 'CSVProcessInterfaceSetting').value('UseQuoting'),
                dataIndex: 'UseQuoting'
            }, {
                text: l10n.ns('core', 'CSVProcessInterfaceSetting').value('QuoteChar'),
                dataIndex: 'QuoteChar'
            }, {
                text: l10n.ns('core', 'CSVProcessInterfaceSetting').value('ProcessHandler'),
                dataIndex: 'ProcessHandler'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        items: [{
            name: 'InterfaceName',
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('InterfaceName')
        }, {
            name: 'Delimiter',
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('Delimiter')
        }, {
            name: 'UseQuoting',
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('UseQuoting')
        }, {
            name: 'QuoteChar',
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('QuoteChar')
        }, {
            name: 'ProcessHandler',
            fieldLabel: l10n.ns('core', 'CSVProcessInterfaceSetting').value('ProcessHandler')
        }]
    }]

});