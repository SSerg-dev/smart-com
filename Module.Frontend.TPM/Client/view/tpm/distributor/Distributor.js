Ext.define('App.view.tpm.distributor.Distributor', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.distributor',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Distributor'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.distributor.Distributor',
            storeId: 'distributorstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.distributor.Distributor',
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
            items: [			{
				text: l10n.ns('tpm', 'Distributor').value('Name'),
				dataIndex: 'Name'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.distributor.Distributor',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Distributor').value('Name'),
        }]
    }]
});