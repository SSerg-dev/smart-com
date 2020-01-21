Ext.define('App.view.tpm.promo.UserRolePromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.userrolepromo',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right',
        items: [{
            itemId: 'historybutton',
            resource: 'HistoricalPromoes',
            action: 'Get{0}',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText'),
        }],
   }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promo.UserRolePromo',
            storeId: 'userrolepromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promo.UserRolePromo',
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
                text: l10n.ns('core', 'AssociatedUser').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('core', 'AdUser').value('Email'),
                dataIndex: 'Email'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promo.UserRolePromo',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Name')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('core', 'AdUser').value('Email'),
            name: 'Email'
        }]
    }]

});