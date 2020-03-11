Ext.define('App.view.core.interface.Interface', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.interface',
    title: l10n.ns('core', 'compositePanelTitles').value('InterfaceTitle'),

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
            model: 'App.model.core.interface.Interface',
            storeId: 'interfacestore',
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

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1
            },
            items: [{
                text: l10n.ns('core', 'Interface').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('core', 'Interface').value('Direction'),
                dataIndex: 'Direction'
            }, {
                text: l10n.ns('core', 'Interface').value('Description'),
                dataIndex: 'Description'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.interface.Interface',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'Interface').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Direction',
            fieldLabel: l10n.ns('core', 'Interface').value('Direction')
        }, {
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'Interface').value('Description')
        }]
    }]

});