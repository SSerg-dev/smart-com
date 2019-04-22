Ext.define('App.view.tpm.techhighlevel.TechHighLevel', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.techhighlevel',
    title: l10n.ns('tpm', 'compositePanelTitles').value('TechHighLevel'),

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
            model: 'App.model.tpm.techhighlevel.TechHighLevel',
            storeId: 'techhighlevelstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.techhighlevel.TechHighLevel',
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
                text: l10n.ns('tpm', 'TechHighLevel').value('Name'),
                dataIndex: 'Name'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.techhighlevel.TechHighLevel',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'TechHighLevel').value('Name'),
        }]
    }]
});
