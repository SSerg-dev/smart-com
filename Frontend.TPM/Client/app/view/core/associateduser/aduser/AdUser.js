Ext.define('App.view.core.associateduser.aduser.AdUser', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.aduser',
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
            model: 'App.model.core.associateduser.user.AdUser',
            storeId: 'aduserstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associateduser.user.AdUser',
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
                dataIndex: 'Sid'
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
        model: 'App.model.core.associateduser.user.AdUser',
        items: [{
            name: 'Sid',
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