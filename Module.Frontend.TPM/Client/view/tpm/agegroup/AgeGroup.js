Ext.define('App.view.tpm.agegroup.AgeGroup', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.agegroup',
    title: l10n.ns('tpm', 'compositePanelTitles').value('AgeGroup'),

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
            model: 'App.model.tpm.agegroup.AgeGroup',
            storeId: 'agegroupstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.agegroup.AgeGroup',
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
				text: l10n.ns('tpm', 'AgeGroup').value('Name'),
				dataIndex: 'Name'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.agegroup.AgeGroup',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'AgeGroup').value('Name'),
        }]
    }]
});