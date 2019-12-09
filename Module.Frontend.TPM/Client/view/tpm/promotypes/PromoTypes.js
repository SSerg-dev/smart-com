Ext.define('App.view.tpm.promotypes.PromoTypes', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promotypes',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoTypes'),

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
            model: 'App.model.tpm.promotypes.PromoTypes',
            storeId: 'promotypesstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promotypes.PromoTypes',
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
                text: l10n.ns('tpm', 'PromoTypes').value('Name'),
                dataIndex: 'Name'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
            model: 'App.model.tpm.promotypes.PromoTypes',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'PromoTypes').value('Name')
        }]
    }]
});
