Ext.define('App.view.core.rpasetting.RPASetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.rpasetting',
    title: l10n.ns('core', 'compositePanelTitles').value('RPASettingTitle'),

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
            model: 'App.model.core.rpasetting.RPASetting',
            storeId: 'rpasettingstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.rpasetting.RPASetting',
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
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns('core', 'RPASetting').value('Json'),
                dataIndex: 'Json'
            }, {
                text: l10n.ns('core', 'RPASetting').value('Name'),
                dataIndex: 'Name'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.rpasetting.RPASetting',
        items: [{
            xtype: 'textfield',
            name: 'Json',
            fieldLabel: l10n.ns('core', 'RPASetting').value('Json')
        }, {
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'RPASetting').value('Name')
        }]
    }]

});