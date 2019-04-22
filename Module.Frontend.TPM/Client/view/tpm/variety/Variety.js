Ext.define('App.view.tpm.variety.Variety', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.variety',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Variety'),

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
            model: 'App.model.tpm.variety.Variety',
            storeId: 'varietystore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.variety.Variety',
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
				text: l10n.ns('tpm', 'Variety').value('Name'),
				dataIndex: 'Name'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.variety.Variety',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Variety').value('Name'),
        }]
    }]
});