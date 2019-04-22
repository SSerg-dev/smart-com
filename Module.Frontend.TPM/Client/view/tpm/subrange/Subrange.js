Ext.define('App.view.tpm.subrange.Subrange', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.subrange',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Subrange'),

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
            model: 'App.model.tpm.subrange.Subrange',
            storeId: 'subrangestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.subrange.Subrange',
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
				text: l10n.ns('tpm', 'Subrange').value('Name'),
				dataIndex: 'Name'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.subrange.Subrange',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Subrange').value('Name'),
        }]
    }]
});