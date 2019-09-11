Ext.define('App.view.core.associateduser.user.User', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.user',
    title: l10n.ns('core', 'compositePanelTitles').value('AdUserTitle'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.user.User',
            storeId: 'aduserstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.user.User',
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
                text: l10n.ns('core', 'AdUser').value('Sid'),
                dataIndex: 'Id'
            }, {
                text: l10n.ns('core', 'AdUser').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('core', 'AdUser').value('Email'),
                dataIndex: 'Email'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.associateduser.user.User',
        items: [{
            name: 'Id',
            fieldLabel: l10n.ns('core', 'AdUser').value('Sid')
        }, {
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AdUser').value('Name')
        }, {
            name: 'Email',
            fieldLabel: l10n.ns('core', 'AdUser').value('Email')
        }]
    }]
});